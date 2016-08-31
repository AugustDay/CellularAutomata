using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace CellularAutomata.OneDimensionalCA
{
    class Imager1D
    {
        private static readonly Size SIZE_DEFAULT = new Size(10, 10);

        private static readonly Color[] COLORS_DEFAULT = { Color.Transparent, Color.Blue, Color.Green,
                                   Color.Red, Color.Yellow, Color.Purple, Color.Aqua}; //TODO add more!

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
        public ImagerGridSettings1D GridType { get; set; }

        private int NumberFilesSaved = 0;

        public bool PrintInfoText = true;

        /// <summary>
        /// Constructs an instance of the Imager1D, with the given Rule object. 
        /// </summary>
        /// <param name="theRules"></param>
        public Imager1D(Rules1D theRules)
        {
            Rule = theRules;
            LinePen = new Pen(Color.Black);
            CellSize = SIZE_DEFAULT;
            GridType = ImagerGridSettings1D.GridOnLive;
            InitializeBrushes();
        }

        /// <summary>
        /// Sets the Brushes property with default colors.
        /// </summary>
        private void InitializeBrushes()
        {
            Brushes = new SolidBrush[Rule.PossibleStates];
            for(int i = 0; i < Brushes.Length; i++)
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
        public void SaveImage(List<Generation1D> theCA, int theLeftEdge)
        {
            //find dimensions
            int maxDistance = theCA[0].Cells.Count;
            NumberFilesSaved++;

            //foreach (Generation1D gen in theCA) //Don't need this as long as universe size is constant.
            //{
            //    maxDistance = Math.Max(maxDistance, gen.Cells.Count);
            //}

            //create image
            Bitmap output = new Bitmap(maxDistance * CellSize.Width, 1 + theCA.Count * CellSize.Height);
            Graphics g = Graphics.FromImage(output);
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(new Point(0, 0), output.Size));
            //image1.SetPixel(x, y, Color.Transparent);
            Point point = new Point();
            int padding;

            for (int generation = 0; generation < theCA.Count; generation++)
            {
                for (int c = 0; c < theCA[generation].Cells.Count; c++)
                {
                    if (theCA[generation].Cells[c].State > 0) //TODO have array of brushes, a color for each state
                    {
                        padding = theLeftEdge - Math.Abs(theCA[generation].LeftEdge);
                        point.X = (c + padding) * CellSize.Width; //this is where leftEdge comes in
                        point.Y = generation * CellSize.Height;
                        g.FillRectangle(Brushes[theCA[generation].Cells[c].State], point.X, point.Y, CellSize.Width, CellSize.Height);
                        if(GridType == ImagerGridSettings1D.GridOnLive)
                        {
                            g.DrawRectangle(LinePen, new Rectangle(point, CellSize));
                        }
                    }
                }
            }

            //Draws a grid.
            if (GridType == ImagerGridSettings1D.Grid)
            {
                DrawGrid(g, output.Size);
            }

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1); //TODO use hex base for time?
            Save(output, t);
            if (PrintInfoText)
            {
                SaveInfo(t);
            }
        }

        /// <summary>
        /// Draws a grid over the bitmap output. 
        /// </summary>
        /// <param name="theG"></param>
        /// <param name="theSize"></param>
        private void DrawGrid(Graphics theG, Size theSize)
        {
            for(int x = 0; x < theSize.Width; x += CellSize.Width)
            {
                theG.DrawLine(LinePen, x, 0, x, theSize.Height);
            }
            for (int y = 0; y < theSize.Height; y += CellSize.Height)
            {
                theG.DrawLine(LinePen, 0, y, theSize.Width, y);
            }
        }

        /// <summary>
        /// Writes image to a file in the local directory.
        /// </summary>
        /// <param name="theOutput"></param>
        private void Save(Bitmap theOutput, TimeSpan theT) //TODO sometimes still overwrites existing file if generating them fast enough!
        {
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.toString() + " -- " + (int)theT.TotalSeconds + "_" + NumberFilesSaved + ".bmp";
            theOutput.Save(@location);
            //output.Save(@location, ImageFormat.Png); //TODO any way to do compression? Bitmaps are large!
            //TODO create "debug" function to write rule specifics to text file
        }

        private void SaveInfo(TimeSpan theT)
        {
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.toString() + " -- " + (int)theT.TotalSeconds + " -- Info.txt";
            List<string> lines = new List<string>();
            lines.Add(Rule.GetInfo());
            System.IO.File.WriteAllLines(@location, lines);
        }

        public void printCA(List<Generation1D> theList)
        {
            List<string> lines = new List<string>();
            string s = "";
            foreach(Generation1D g in theList)
            {
                foreach(Cell1D c in g.Cells)
                {
                    s += c.State;
                }
                lines.Add(s);
                s = "";
            }
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string location = AppDomain.CurrentDomain.BaseDirectory + Rule.toString() + " -- " + (int)t.TotalSeconds + ".txt";
            System.IO.File.WriteAllLines(@location, lines);

        }
    }
}
