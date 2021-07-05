using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientContext
{
    class Program
    {
        public sealed class BuildingContext : IDisposable
        {
            public int WallHeight;
            private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

            static BuildingContext()
            {
                stack.Push(new BuildingContext(0));
            }

            public BuildingContext(int wallHeight)
            {
                WallHeight = wallHeight;
                stack.Push(this);
            }

            public static BuildingContext Current => stack.Peek();

            public void Dispose()
            {
                if (stack.Count > 1)
                    stack.Pop();
            }
        }

        public class Building
        {
            public List<Wall> Walls = new List<Wall>();

            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var wall in Walls) 
                    sb.AppendLine(wall.ToString());
                return sb.ToString();

            }
        }

        public struct Point
        {
            private int X, Y;

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}"; 
            }
        }

        public class Wall
        {
            public Point Start, End;
            public int Height;

            public Wall(Point start, Point end)
            {
                Start = start;
                End = end;
                Height = BuildingContext.Current.WallHeight;
            }

            public override string ToString()
            {
                return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Height)}: {Height}, "; 
            }
        }

        static void Main(string[] args)
        {
            var house = new Building();

            // gnd floor 3000
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                // 1st floor 3500
                using (new BuildingContext(3500))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
                }
                // gnd floor 3000
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }

            Console.WriteLine(house);

        }
    }
}
