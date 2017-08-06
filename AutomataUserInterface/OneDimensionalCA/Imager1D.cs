using AutomataUserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CellularAutomata.OneDimensionalCA
{
    public class Imager1D 
    {
        public class ImageEventArgs : EventArgs
        {
            public Bitmap ImageOutput { get; set; }
        }

        public enum GridSettings1D
        {
            NoGrid,
            Grid,
            GridOnLive,
        }

        public enum ColorThemes
        {
            Default,
            Choice2,
            Choice3,
            NeighborhoodLookup
        }

        private static readonly Size SIZE_DEFAULT = new Size(10, 10);

        private static readonly Color[][] COLOR_THEMES = {
            new Color[] { Color.Transparent, Color.Blue, Color.Green, Color.Red, Color.Yellow, Color.Purple, Color.Aqua},
            new Color[] { Color.Transparent, Color.Blue, Color.Yellow},
            new Color[] { Color.Transparent, Color.Blue, Color.Red},
            new Color[] { Color.Transparent, Color.LightYellow, Color.Yellow, Color.GreenYellow, Color.LightGreen, Color.Green,
                          Color.DarkGreen, Color.SeaGreen, Color.MediumSeaGreen, Color.DeepSkyBlue, Color.Blue, Color.Navy,
                          Color.SlateBlue, Color.Purple, Color.MediumOrchid, Color.DarkMagenta, Color.Black, Color.Silver,
                          Color.SlateGray, Color.LightSteelBlue, Color.Crimson, Color.Firebrick, Color.DarkRed, Color.Brown,
                          Color.Goldenrod, Color.Gold, Color.Linen } };

        private static Color[] MyCurrentColorTheme = COLOR_THEMES[0];
                
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

        public static ColorThemes CurrentColorThemeEnum { get; set; }

        public delegate void ImageGeneratedEventHandler(object sender, ImageEventArgs args);

        public event ImageGeneratedEventHandler ImageGenerated;

        /// <summary>
        /// Constructs an instance of the Imager1D, with the given Rule object. 
        /// </summary>
        /// <param name="theRules"></param>
        public Imager1D()
        {
            LinePen = new Pen(Color.Black);
            CellSize = SIZE_DEFAULT;
            GridType = GridSettings1D.GridOnLive;
            CurrentColorThemeEnum = ColorThemes.Default;
            InitializeBrushes();
        }

        /// <summary>
        /// Sets the Brushes property with default colors.
        /// </summary>
        private void InitializeBrushes()
        {
            Brushes = new SolidBrush[MyCurrentColorTheme.Length];
            for (int i = 0; i < Brushes.Length; i++)
            {
                Brushes[i] = new SolidBrush(MyCurrentColorTheme[i]);
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

            //create image
            Bitmap ImageOutput = new Bitmap(1 + maxDistance * CellSize.Width, 1 + theCA.Count * CellSize.Height);
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
                //ImageTools.ChangeDisplayedImage(ImageOutput);
                OnImageGenerated(ImageOutput);
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

        public void ChooseColorTheme(ColorThemes theTheme)
        {
            MyCurrentColorTheme = COLOR_THEMES[(int) theTheme];
            InitializeBrushes();
        }

        /// <summary>
        /// To be called when a new automata is generated successfully (I.E. when the history list has changed).
        /// TODO change the name to HistoryChanged to be less ambiguous? But perhaps this is useful for other times when the new automata is made.
        /// </summary>
        protected virtual void OnImageGenerated(Bitmap theImage)
        {           
            ImageGenerated?.Invoke(this, new ImageEventArgs() { ImageOutput = theImage });
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

            return CellSize.Equals(otherImager.CellSize) &&
                LinePen.Width == otherImager.LinePen.Width && LinePen.Color == otherImager.LinePen.Color &&
                GridType == otherImager.GridType;
        }
    }
}
