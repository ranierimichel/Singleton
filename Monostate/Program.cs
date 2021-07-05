using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Monostate
{
    public class CEO
    {
        private static string name;
        private static int age;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ceo = new CEO();
            ceo.Name = "Maria Antonia";
            ceo.Age = 1;

            var ceo2 = new CEO();
            Console.WriteLine(ceo2);
        }
    }
}
