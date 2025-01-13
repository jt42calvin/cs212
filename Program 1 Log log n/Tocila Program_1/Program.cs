/*****************************************
 * Simple C# program that computes the floor log of a number.
 * 
 * Purpose: Floor log --> lg lg (n) n being the input value.
 * We take the input, divide it by 2 until we are left with 1.
 * During this, we count how many times we divided by 2, the
 * program returns that count as the floor log.
 * 
 * Example: 13 / 2 = 6; / 2 = 3; / 2 = 1; so floor lg(13) = 3.
 * 
 * CS 212-A, Jacob Tocila, September 7, 2023
 * 
*****************************************/

/*
Console.WriteLine("Welcome to the lg lg(n) solver!\n");
while (true)
{
    Console.WriteLine("Enter n:");

}
Console.WriteLine("You inputted ",);
*/

using System.Diagnostics.Metrics;

namespace LgLgN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Luxurious lg lg n solver!");
            while (true)
            {
                Console.Write("\nEnter N: ");
                long n = long.Parse(Console.ReadLine());
                long lglg = LgLg(n);

                Console.WriteLine("Floor Log({0}) = {1}.", n, lglg);
            }
        }
        static long LgLg(long n)
        {
            long counter = 0;
            while (n != 1)
            {
                n = n / 2;
                counter++;
            }
            return counter;
        }
    }
}
