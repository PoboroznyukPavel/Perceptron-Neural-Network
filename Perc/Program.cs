using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Perc
{


    class Perceptron
    {
        int res;
        public int[] Weight { get; }
        public int Learning_rate { get; }
        public int Bias { get; }
        public Random rnd = new Random();


        public Perceptron()
        {
            res = 0;
            Learning_rate = 1;
            Bias = 2;
            Weight = new int[15];
            for (int i = 0; i < 15; i++)
                Weight[i] = rnd.Next(5);
        }

        public Perceptron(int f)
        {
            Weight = new int[++f];
            for (int i = 0; i < f; i++)
                Weight[i] = rnd.Next(6);
            res = 0;
            Learning_rate = rnd.Next(6)+1;
            Bias = rnd.Next(20);
        }

        override public string ToString()
        {
            string f = "";
            for (int i = 0; i < Weight.Length; i++)
            {
                f += Weight[i].ToString();
            }
            return f;
        }


        public void IncreaseWhereNeeded(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if (s[i] != '1')
                    Weight[i] += Learning_rate;
        }

        public void DecreaseWhereNeeded(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if (s[i] == '1')
                    Weight[i] -= Learning_rate;
        }

        public bool Feed(string arr)
        {
            double res = 0;
            for (int i = 0; i < arr.Length; i++)
                res += Weight[i] * arr[i];
            if (res >= Bias)
                return true;
            else
                return false;
        }
    }


    class Program
    {


        static void WriteP(char c)
        {
            if (c == '1')
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (c == '2')
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (c == '0')
                Console.ForegroundColor = ConsoleColor.Black;
            else
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WritePyramid(string s, bool b)
        {
            for (int j = 0; j < 5; j++)
                WriteP(s[j]);
            Console.WriteLine();
            for (int j = 0; j < 5; j++)
                WriteP(s[5 + j]);
            Console.WriteLine("    -    это пирамидка?   " + b);
            for (int j = 0; j < 5; j++)
                WriteP(s[10 + j]);
            Console.WriteLine("\n");
        }

        public static void WritePyramid(string s)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                    Console.Write(s[5 * i + j]);
                Console.WriteLine();
            }

            Console.WriteLine("\n");
        }

        public static void WritePerc(Perceptron p)
        {
            Console.WriteLine("\nВеса:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                    if (p.Weight[5 * i + j] > 0)
                        Console.Write(" " + p.Weight[5 * i + j] + " ");
                    else
                        Console.Write(p.Weight[5 * i + j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("\nBias = " + p.Bias + "      learning rate = " + p.Learning_rate + "\n");
        }

        public static string Train(Perceptron p, int t, string[] tr, string[] fal)
        {
            int wrongfalse = 0;
            int wrongpos = 0;
            int right = 0;
            int rightFirst = 0;
            int rightSec = 0;
            int f = 0;
            bool x;
            for (int i = 0; i < t; i++)
            {
                if (i % 2 != 0)
                {
                    f = p.rnd.Next(fal.Length);
                    x = p.Feed(fal[f]);
                    WritePyramid(fal[f], x);
                    if (p.Feed(fal[f]))
                    {
                        Console.WriteLine("Неправильно - ложное срабатывание\n\n");
                        wrongpos++;
                        p.DecreaseWhereNeeded(fal[f]);
                    }
                    else
                    {
                        if (i > t / 2)
                            rightSec++;
                        else
                            rightFirst++;
                        right++;
                    }
                }
                else
                {
                    f = p.rnd.Next(tr.Length);
                    x = p.Feed(tr[f]);
                    WritePyramid(tr[f], x);
                    if (!p.Feed(tr[f]))
                    {
                        Console.WriteLine("Неправильно - не узнал\n\n");
                        wrongfalse++;
                        p.IncreaseWhereNeeded(tr[f]);
                    }
                    else
                    {
                        if (i > t / 2)
                            rightSec++;
                        else
                            rightFirst++;
                        right++;
                    }
                }
            }
            string res = "Результаты тренировки:\nПравильных = " + right + "\nЛожных срабатываний = " + wrongpos + "\nНе распознано = " + wrongfalse + "\n%Правильных = " + (right * 100 / t) + "%" + "\n%Правильных в  1 половине = " + (rightFirst * 100 / (t / 2)) + "%" + "\n%Правильных во 2 половине = " + (rightSec * 100 / (t / 2)) + "%";
            return res;
        }


        static void Main(string[] args)
        {
            string[] tr = { "112111222123332", "112111222123332", "112111222122322" };
            string[] fal = { "221222111211111", "111111222123432", "111111111111111","222222222222222","222222111222222",
                "122212111212221","112112111221112", "111111222212222", "222112221122211","222112221122211",
                "122211222112221","112112111212121"};

            Console.Write("Введите количество эпох = ");
            int epoch = Convert.ToInt32(Console.ReadLine());
            string trainingRes;
            Perceptron p = new Perceptron(15);
            trainingRes = Train(p, epoch, tr, fal);
            for (int j = 0; j < epoch / 5; j++)
            {
                string f = "";
                if (j % 10 != 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        f += p.rnd.Next(0, 9);
                    }
                }
                else
                {
                    if (j % 20 == 0)
                    {
                        Console.WriteLine("Это false");
                        f = fal[p.rnd.Next(fal.Length)];
                    }
                    else
                    {
                        Console.WriteLine("Это true");
                        f = tr[p.rnd.Next(tr.Length)];
                    }
                }
                WritePyramid(f, p.Feed(f));
            }
            Console.WriteLine("\n" + trainingRes + "\n");
            WritePerc(p);
            Console.ReadKey();
        }
    }
}
