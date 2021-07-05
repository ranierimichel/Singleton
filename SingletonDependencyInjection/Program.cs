using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using Autofac;
using MoreLinq;
using NUnit.Framework;

namespace SingletonDependencyInjection
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;
        private static int instanceCount; //0
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            instanceCount++;
            WriteLine("Initializing database");

            capitals = File.ReadAllLines(Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt"))
                .Batch(2).ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
        private static readonly Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        public static SingletonDatabase Instance => instance.Value;
    }

    public class OrdinaryDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;
        public OrdinaryDatabase()
        {
            WriteLine("Initializing database");

            capitals = File.ReadAllLines(Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt"))
                .Batch(2).ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

    }

    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += SingletonDatabase.Instance.GetPopulation(name);
            return result;
        }
    }

    public class ConfigurableRecordFinder
    {
        private readonly IDatabase database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(paramName: nameof(database));   
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += database.GetPopulation(name);
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["charlie"] = 3,
            }[name];
        }
    }


    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            var tp = 17700000 + 17800000 + 17500000;
            var rf = new SingletonRecordFinder();
            var names = new[] { "Sao Paulo", "New York", "Seoul" };
            var tp1 = rf.GetTotalPopulation(names);
            Assert.That((tp1.Equals(tp)));
        }
        [Test]
        public void ConfigurablePopulationTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] {"alpha", "beta"};
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(3));
        }
        [Test]
        public void DIPopulationTest()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<OrdinaryDatabase>().As<IDatabase>().SingleInstance();
            cb.RegisterType<ConfigurableRecordFinder>();
            var names = new[] { "Sao Paulo", "Tokyo" };
            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurableRecordFinder>();
                var tp = rf.GetTotalPopulation(names);
                Assert.That(tp, Is.EqualTo(17700000 + 33200000));
            }

        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.Instance;
            var city = "Sao Paulo";
            var population = db.GetPopulation(city);
            WriteLine($"{city} has a population: {population}");

        }
    }
}
