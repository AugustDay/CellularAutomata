using CellularAutomata.OneDimensionalCA;
using System;
using System.Numerics;
using AutomataUserInterface;
using System.Windows.Media;

namespace AutomataUserInterface.Tools
{
    class Printer
    {
        public static Brush PositiveColor = Brushes.Blue;

        public static Brush NeutralColor = Brushes.Black;

        public static Brush ErrorColor = Brushes.Red;

        public static Brush GreenColor = Brushes.Green;

        public static MainWindow OutputWindow;

        public static void DisplayMessageLine(string theMessage)
        {
            OutputWindow.addSomeColoredText("\n" + theMessage, NeutralColor);
        }

        public static void DisplayMessageLine(string theMessage, Brush theColor)
        {
            OutputWindow.addSomeColoredText("\n" + theMessage, theColor);
        }

        public static void DisplayMessage(string theMessage)
        {
            OutputWindow.addSomeColoredText(theMessage, NeutralColor);
        }

        public static void DisplayMessage(string theMessage, Brush theColor)
        {
            OutputWindow.addSomeColoredText(theMessage, theColor);
        }
    }
}
