using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace CellularAutomata.OneDimensionalCA
{
    class Imager1D
    {
        private Color[] myDefaultColors = { Color.Transparent, Color.Blue, Color.Green, Color.Red, Color.Yellow}; //TODO add more!

        public Rules1D myRules;

        public SolidBrush[] myBrushes;

        public Size mySize { get; }

        public Pen myPen { get; }
        

        public Imager1D(Rules1D theRules)
        {
            myRules = theRules;
            myPen = new Pen(Color.Black);
            mySize = new Size(20, 20);
            
            InitializeBrushes();
        }

        public void InitializeBrushes()
        {
            myBrushes = new SolidBrush[myRules.myPossibleStates];
            for(int i = 0; i < myBrushes.Length; i++)
            {
                myBrushes[i] = new SolidBrush(myDefaultColors[i]);
            }
        }

        public void GenerateAndSaveImage(List<Generation1D> ca, int left)
        {
            //find dimensions
            int maxDistance = 0;
            foreach (Generation1D gen in ca)
            {
                maxDistance = Math.Max(maxDistance, gen.Cells.Count);
            }
            //Console.WriteLine("Largest number of cells = " + maxDistance);
            //Console.WriteLine("Number of generations = " + ca.Count);

            Bitmap output = new Bitmap(maxDistance * 20, 1 + ca.Count * 20);
            Graphics g = Graphics.FromImage(output);
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(new Point(0, 0), output.Size));
            //image1.SetPixel(x, y, Color.Transparent);
            Point point = new Point();
            int padding;

            for (int generation = 0; generation < ca.Count; generation++)
            {
                for (int c = 0; c < ca[generation].Cells.Count; c++)
                {
                    if (ca[generation].Cells[c].State > 0) //TODO have array of brushes, a color for each state
                    {
                        padding = left - Math.Abs(ca[generation].LeftEdge);
                        point.X = (c + padding) * 20; //this is where leftEdge comes in
                        point.Y = generation * 20;
                        g.FillRectangle(myBrushes[ca[generation].Cells[c].State], point.X, point.Y, mySize.Width, mySize.Height);
                        g.DrawRectangle(myPen, new Rectangle(point, mySize));
                    }
                }
            }
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string location = AppDomain.CurrentDomain.BaseDirectory + myRules.toString() + " -- " + (int)t.TotalSeconds + ".bmp";
            output.Save(@location);
            //output.Save(@"G:\Personal\newImage.png", ImageFormat.Png); //doesn't compress?
        }

        public void printCA(List<Generation1D> list, int left)
        {
            List<string> lines = new List<string>();
            foreach (Generation1D g in list)
            {
                int padding = left - Math.Abs(g.LeftEdge);
                string s = "";
                for (; padding > 0; padding--)
                {
                    s += " ";
                }

                foreach (Cell1D c in g.Cells)
                {
                    if (c.State > 0) //TODO Needs symbol for each state
                    {
                        s += "\u2588";
                    }
                    else
                    {
                        s += " ";
                    }
                }
                lines.Add(s);
            }

            System.IO.File.WriteAllLines(@"C:\Users\Austin\Documents\visual studio 2015\Projects\CellularAutomata\CA.txt", lines);
        }

        public void printCANew(List<Generation1D> list)
        {
            List<string> lines = new List<string>();
            string s = "";
            foreach(Generation1D g in list)
            {
                foreach(Cell1D c in g.Cells)
                {
                    s += c.State;
                }
                lines.Add(s);
                s = "";
            }
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string location = AppDomain.CurrentDomain.BaseDirectory + myRules.toString() + " -- " + (int)t.TotalSeconds + ".txt";
            System.IO.File.WriteAllLines(@location, lines);

        }
    }
}
