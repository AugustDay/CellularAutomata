﻿using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class Program
    {
        static void Main(string[] args)
        {
            Automata1D theAutomata = new Automata1D();
            for(int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    for (int c = 0; c <= 1; c++)
                    {
                        for (int d = 0; d <= 1; d++)
                        {
                            for (int e = 0; e <= 1; e++)
                            {
                                for (int f = 0; f <= 1; f++)
                                {
                                    for (int g = 0; g <= 1; g++)
                                    {
                                        for (int h = 0; h <= 1; h++)
                                        {
                                            int[] rule = new int[] { a, b, c, d, e, f, g, h };
                                            theAutomata.myRules.myRule = rule;
                                            theAutomata.Go();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Program complete.");
            Console.ReadKey();
        }
    }
}