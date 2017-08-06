using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CellularAutomata.OneDimensionalCA;

namespace AutomataUserInterface.Utilities
{
    class Downloader
    {
        private static DateTime EpochStart = new DateTime(1970, 1, 1);

        public static void SaveToFile(Bitmap theImageOutput, Rules1D theRule, bool printInfoText)
        {
            if (theImageOutput == null)
            {
                throw new Exception("Program attempted to save an automata image before it was generated.\n");
            }
            TimeSpan t = DateTime.UtcNow - EpochStart;
            SaveImage(t, theImageOutput, theRule); //TODO remove parameters from save / saveInfo, use object variable instead? 
            if (printInfoText)
            {
                SaveInfo(t, theRule);
            }
        }

        /// <summary>
        /// Writes image to a file in the local directory.
        /// </summary>
        private static void SaveImage(TimeSpan theTime, Bitmap theImageOutput, Rules1D theRule) //TODO sometimes still overwrites existing file if generating them fast enough!
        {
            string location = AppDomain.CurrentDomain.BaseDirectory + theRule.ToString() + " -- " + (int)theTime.TotalSeconds + ".bmp";
            theImageOutput.Save(@location);
            //output.Save(@location, ImageFormat.Png); //TODO any way to do compression? Bitmaps are large!
        }

        private static void SaveInfo(TimeSpan theTime, Rules1D theRule)
        { //TODO rule.tostring concise and verbose (one has full rule array, other just has number)
            string location = AppDomain.CurrentDomain.BaseDirectory + theRule.ToString() + " -- " + (int)theTime.TotalSeconds + " -- Info.txt";
            List<string> lines = new List<string>();
            lines.Add(theRule.GetInfo());
            System.IO.File.WriteAllLines(@location, lines);
        }
    }
}
