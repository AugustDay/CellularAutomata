using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomataUserInterface
{
    public static class ImageChangedListener
    {
        public static System.Windows.Controls.Image ImageField;

        public static void ImageChanged(Bitmap theImage)
        {
            if (ImageField != null)
            {
                ImageField.Source = BasicConvertBitmapToBitmapSource(theImage);
            }
        }

        public static BitmapSource MakeBitmapImageFromDrawing()
        {
            Bitmap pic = new Bitmap(500, 600);
            Graphics g = Graphics.FromImage(pic);
            g.FillRectangle(new SolidBrush(System.Drawing.Color.Transparent), new Rectangle(new Point(0, 0), pic.Size));
            g.FillRectangle(new SolidBrush(System.Drawing.Color.Blue), new Rectangle(0, 0, 60, 80));
            g.FillRectangle(new SolidBrush(System.Drawing.Color.Green), new Rectangle(440, 520, 60, 80));

            //blah blah get a bitmap
            return BasicConvertBitmapToBitmapSource(pic);
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        //works, but causes a memory leak each time a new image is loaded.
        //not severe enough to be a problem for now, but need to find a better solution soon
        public static BitmapSource BasicConvertBitmapToBitmapSource(Bitmap src)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                              src.GetHbitmap(), //leaks!
                                              IntPtr.Zero,
                                              System.Windows.Int32Rect.Empty,
                                              BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
