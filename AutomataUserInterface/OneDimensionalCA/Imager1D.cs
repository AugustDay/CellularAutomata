using AutomataUserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CellularAutomata.OneDimensionalCA
{
    public class Imager1D 
    {
        public enum GridSettings1D
        {
            NoGrid,
            Grid,
            GridOnLive,
        }

        private static readonly Size SIZE_DEFAULT = new Size(10, 10);

        private static readonly Color[] COLORS_DEFAULT = { Color.Transparent, Color.Blue, Color.Green,
                                   Color.Red, Color.Yellow, Color.Purple, Color.Aqua}; //TODO add more!

        private static DateTime EpochStart = new DateTime(1970, 1, 1);

        private Rules1D Rule;

        /// <summary> Colors for each Cell. </summary>
        public SolidBrush[] Brushes { get; set; }

        /// <summary> Size of each Cell drawn on the bitmap. </summary>
        public Size CellSize { get; set; }

        public Pen LinePen { get; }

        /// <summary>
        /// Determines how to draw the grid on bitmaps. 
        /// Set using ImagerGridSettings1D enum list.
        /// </summary>
        public GridSettings1D GridType { get; set; }

        private int NumberFilesSaved = 0;

        public bool PrintInfoText = true;

        public Bitmap ImageOutput;

        /// <summary>
        /// Constructs an instance of the Imager1D, with the given Rule object. 
        /// </summary>
        /// <param name="theRules"></param>
        public Imager1D(Rules1D theRules)
        {
            Rule = theRules;
            LinePen = new Pen(Color.Black);
            CellSize = SIZE_DEFAULT;
            GridType = GridSettings1D.GridOnLive;
            InitializeBrushes();
        }

        /// <summary>
        /// Sets the Brushes property with default colors.
        /// </summary>
        private void InitializeBrushes()
        {
            Brushes = new SolidBrush[Rule.PossibleStates];
            for (int i = 0; i < Brushes.Length; i++)
            {
                Brushes[i] = new SolidBrush(COLORS_DEFAULT[i]);
            }
        }

        /// <summary>
        /// Generates and saves a bitmap of the current CA board state. 
        /// Image is saved in local directory. 
        /// </summary>
        /// <param name="theCA"></param>
        /// <param name="theLeftEdge"></param>
        public void GenerateImage(List<int[]> theCA)
        {
            //find dimensions
            int maxDistance = theCA[0].Length;
            

            //foreach (Generation1D gen in theCA) //Don't need this as long as universe size is constant.
            //{ 
            //    maxDistance = Math.Max(maxDistance, gen.Cells.Count);
            //}

            //create image
            ImageOutput = new Bitmap(1 + maxDistance * CellSize.Width, 1 + theCA.Count * CellSize.Height);
            using (Graphics g = Graphics.FromImage(ImageOutput))
            {
                g.FillRectangle(new SolidBrush(Brushes[0].Color), new Rectangle(new Point(0, 0), ImageOutput.Size));
                //image1.SetPixel(x, y, Color.Transparent);
                Point point = new Point();

                for (int generation = 0; generation < theCA.Count; generation++)
                {
                    for (int c = 0; c < theCA[generation].Length; c++)
                    {
                        if (theCA[generation][c] > 0) //TODO have array of brushes, a color for each state
                        {
                            point.X = c * CellSize.Width; //this is where leftEdge comes in
                            point.Y = generation * CellSize.Height;
                            g.FillRectangle(Brushes[theCA[generation][c]], point.X, point.Y, CellSize.Width, CellSize.Height);
                            //TODO would having a single rectangle representing every cell, and changing its x and y values be faster? 
                            if (GridType == GridSettings1D.GridOnLive)
                            {
                                g.DrawRectangle(LinePen, new Rectangle(point, CellSize));
                            }
                        }
                    }
                }

                //Draws a grid.
                if (GridType == GridSettings1D.Grid)
                {
                    DrawGrid(g, ImageOutput.Size);
                }
                ImageTools.ImageChanged(ImageOutput);
            }
        }

        /// <summary>
        /// Draws a grid over the bitmap output. 
        /// </summary>
        /// <param name="theG"></param>
        /// <param name="theSize"></param>
        private void DrawGrid(Graphics theG, Size theSize)
        {
            for (int x = 0; x < theSize.Width; x += CellSize.Width)
            {
                theG.DrawLine(LinePen, x, 0, x, theSize.Height);
            }
            for (int y = 0; y < theSize.Height; y += CellSize.Height)
            {
                theG.DrawLine(LinePen, 0, y, theSize.Width, y);
            }
        }

        public void SaveToFile()
        {
            if(ImageOutput == null)
            {
                throw new Exception("Program attempted to save an automata image before it was generated.\n");
            }
            NumberFilesSaved++;
            TimeSpan t = DateTime.UtcNow - EpochStart;
            SaveImage(t); //TODO remove parameters from save / saveInfo, use object variable instead? 
            if (PrintInfoText)
            {
                SaveInfo(t);
            }
        }

        /// <summary>
        /// Writes image to a file in the local directory.
        /// </summary>
        private void SaveImage(TimeSpan theTime) //TODO sometimes still overwrites existing file if generating them fast enough!
        {            
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.ToString() + " -- " + (int)theTime.TotalSeconds + "_" + NumberFilesSaved + ".bmp";
            ImageOutput.Save(@location);
            //output.Save(@location, ImageFormat.Png); //TODO any way to do compression? Bitmaps are large!
            //TODO create "debug" function to write rule specifics to text file
        }

        private void SaveInfo(TimeSpan theTime)
        { //TODO rule.tostring concise and verbose (one has full rule array, other just has number)
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.ToString() + " -- " + (int)theTime.TotalSeconds + " -- Info.txt";
            List<string> lines = new List<string>();
            lines.Add(Rule.GetInfo());
            System.IO.File.WriteAllLines(@location, lines);
        }

        public void printCA(List<int[]> theList)
        {
            List<string> lines = new List<string>();
            string s = "";
            foreach (int[] g in theList)
            {
                foreach (int c in g)
                {
                    s += c;
                }
                lines.Add(s);
                s = "";
            }
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.ToString() + " -- " + (int)t.TotalSeconds + ".txt";
            System.IO.File.WriteAllLines(@location, lines);
        }

        public List<string> displayCA(List<int[]> theList)
        {
            List<string> lines = new List<string>();
            string s = "";
            foreach (int[] g in theList)
            {
                foreach (int c in g)
                {
                    s += c;
                }
                lines.Add(s);
                s = "";
            }
            return lines;
        }

        public override bool Equals(Object theOther)
        {
            // Check for null values and compare run-time types.
            if (theOther == null || GetType() != theOther.GetType())
            {
                return false;
            }
            Imager1D otherImager = (Imager1D)theOther;
            if (Brushes.Length != otherImager.Brushes.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Brushes.Length; i++)
                {
                    bool equalBrush = Brushes[i].Color == otherImager.Brushes[i].Color;
                    if (!equalBrush)
                    {
                        return false;
                    }
                }
            }

            return Rule.Equals(otherImager.Rule) && CellSize.Equals(otherImager.CellSize) &&
                LinePen.Width == otherImager.LinePen.Width && LinePen.Color == otherImager.LinePen.Color &&
                GridType == otherImager.GridType && NumberFilesSaved == otherImager.NumberFilesSaved &&
                PrintInfoText == otherImager.PrintInfoText;
        }
    }
}
