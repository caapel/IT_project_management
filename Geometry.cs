namespace Geometry
{
    public class Geometry
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Введите координаты первого вектора:");
            Vector v1 = ReadVector();

            Console.WriteLine("Введите координаты второго вектора:");
            Vector v2 = ReadVector();

            Console.WriteLine($"\nДлина первого вектора: {GetLength(v1):F2}");
            Console.WriteLine($"Длина второго вектора: {GetLength(v2):F2}");

            Vector sum = Add(v1, v2);
            Console.WriteLine($"Сумма векторов: ({sum.X:F2}, {sum.Y:F2})");
        }

        static Vector ReadVector()
        {
            Vector v = new Vector();

            Console.Write("X = ");
            while (!double.TryParse(Console.ReadLine(), out v.X))
            {
                Console.WriteLine("Ошибка ввода! Введите число:");
                Console.Write("X = ");
            }

            Console.Write("Y = ");
            while (!double.TryParse(Console.ReadLine(), out v.Y))
            {
                Console.WriteLine("Ошибка ввода! Введите число:");
                Console.Write("Y = ");
            }
            return v;
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector
            {
                X = vector1.X + vector2.X,
                Y = vector1.Y + vector2.Y
            };
        }
        public static double GetLength(Vector vector)
        {
            if (vector.X == 0 && vector.Y == 0)
            {

                Console.WriteLine("Задан нулевой вектор");
            }
             return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);      

        }

    }
}
