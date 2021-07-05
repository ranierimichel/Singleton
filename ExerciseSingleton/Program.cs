using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseSingleton
{
    public static class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var a = func.Invoke();
            var b = func.Invoke();
            
            if (a.Equals(b))
                return true;
            return false;
        }
    }

    class Program
    {
        public static object x = new object();
        
        public static object funcSingleton()
        {
            return x;
        }

        public static object funcNoSinleton()
        {
            return new object();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(SingletonTester.IsSingleton(funcSingleton));
            Console.WriteLine(SingletonTester.IsSingleton(funcNoSinleton)); 
            Console.WriteLine(SingletonTester.IsSingleton(funcSingleton));
            Console.WriteLine(SingletonTester.IsSingleton(funcNoSinleton));
        }
    }
}
