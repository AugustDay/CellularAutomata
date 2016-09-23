using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomataUserInterface
{
    public static class ImageTools
    {
        public static System.Windows.Controls.Image ImageField;

        public static void ImageChanged(Bitmap theImage)
        {
            if (ImageField != null)
            {
                BitmapSource bSource = Convert(theImage);

                bSource.Freeze();

                ImageField.Source = bSource;
            }
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to a BitmapSource, usable in a WPF image panel.
        /// </summary>
        /// <param name="theBitmap"></param>
        /// <returns></returns>
        /// <author>Clemens</author>
        /// http://stackoverflow.com/a/30729291
        public static BitmapSource Convert(Bitmap theBitmap)
        {
            var bitmapData = theBitmap.LockBits(
                new Rectangle(0, 0, theBitmap.Width, theBitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, theBitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            theBitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
} 
