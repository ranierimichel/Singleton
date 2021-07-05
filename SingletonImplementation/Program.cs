using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using MoreLinq;
using System.IO;

namespace SingletonImplementation
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;

        private SingletonDatabase()
        {
            WriteLine("Initializing database");

            capitals = File.ReadAllLines("capitals.txt").Batch(2).ToDictionary(
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
