using System;
using System.Collections.Generic;
using System.Drawing;
using CellularAutomata.OneDimensionalCA;

namespace AutomataUserInterface.Tools
{
    class Downloader
    {
        private static DateTime EpochStart = new DateTime(1970, 1, 1);

        public static void SaveToFile(Bitmap theImageOutput, Rules1D theRule, Analysis1D theAnalysis, bool printSeedText, bool printAnalysisText)
        {
            if (theImageOutput == null)
            {
                throw new Exception("Program attempted to save an automata image before it was generated.\n");
            }
            TimeSpan t = DateTime.UtcNow - EpochStart;
            SaveImage(t, theImageOutput, theRule); //TODO remove parameters from save / saveInfo, use object variable instead? 
            if (printSeedText)
            {
                SaveInfo(t, theRule, theAnalysis, printAnalysisText);
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

        private static void SaveInfo(TimeSpan theTime, Rules1D theRule, Analysis1D theAnalysis, bool printAnalysisText)
        { //TODO rule.tostring concise and verbose (one has full rule array, other just has number)
            string location = AppDomain.CurrentDomain.BaseDirectory + theRule.ToString() + " -- " + (int)theTime.TotalSeconds + " -- Info.txt";
            List<string> lines = new List<string>();
            lines.Add(theRule.GetInfo());
            if(printAnalysisText)
            {
                lines.Add("================\nAnalysis:");
                lines.Add(theAnalysis.GetAnalysis());
            }
            System.IO.File.WriteAllLines(@location, lines);
        }
    }
}
