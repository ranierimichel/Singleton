using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace TestabilityIssues
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

            capitals = File.ReadAllLines(Path.Combine( new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt"))
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
            //17700000 + 17800000 + 17500000
            var tp = 17700000 + 17800000 + 17500000;
            var rf = new SingletonRecordFinder();
            var names = new[] {"Sao Paulo", "New York", "Seoul"};
            var tp1 = rf.GetTotalPopulation(names);
            Assert.That((tp1.Equals(tp)));
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
