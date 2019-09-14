using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageComperssor
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "c":
                case "compress":
                    List<String> paths = Directory.GetFiles(args[1]).ToList<String>();
                    Compress(paths);
                    break;

                case "d":
                case "decompress":

                default:
                    return;
            }
        }

        static void Compress(List<String> paths)
        {
            Bitmap averageMap = null;
            long currentImages = 0;

            foreach (String path in paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                Bitmap bitmap = new Bitmap(path);

                if (averageMap == null)
                {
                    averageMap = new Bitmap(path);
                }
                else
                {
                    for (int y = 0; bitmap.Height > y; y++)
                    {
                        for (int x = 0; bitmap.Width > x; x++)
                        {
                            Color bitmapColor = bitmap.GetPixel(x, y);
                            Color averageColor = averageMap.GetPixel(x, y);

                            Byte r = Convert.ToByte((bitmapColor.R + averageColor.R * currentImages) / (currentImages + 1));
                            Byte g = Convert.ToByte((bitmapColor.G + averageColor.G * currentImages) / (currentImages + 1));
                            Byte b = Convert.ToByte((bitmapColor.B + averageColor.B * currentImages) / (currentImages + 1));

                            averageMap.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }
                }

                currentImages++;
            }

            foreach (String path in paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                Bitmap bitmap = new Bitmap(path);
                Bitmap newMap = new Bitmap(bitmap.Width, bitmap.Height);

                for (int y = 0; bitmap.Height > y; y++)
                {
                    for (int x = 0; bitmap.Width > x; x++)
                    {
                        if (!ColorCompare(bitmap.GetPixel(x, y), averageMap.GetPixel(x, y)))
                        {
                            newMap.SetPixel(x, y, bitmap.GetPixel(x, y));
                        }
                        else
                        {
                            newMap.SetPixel(x, y, Color.FromArgb(0));
                        }
                    }
                }

                newMap.Save(@"D:\Images\" + Path.GetFileName(path) + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }

            averageMap.Save(@"D:\Images\average.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        static bool ColorCompare (Color color1, Color color2)
        {
            const int percentage = 80;

            int singleValue = 255 * (100 - percentage) / 100;
            int rgbValue = 764 * (100 - percentage) / 100;

            bool retval = true;

            if(Math.Abs(color1.R - color2.R) >= singleValue)
            {
                retval = false;
            }

            if (Math.Abs(color1.G - color2.G) >= singleValue)
            {
                retval = false;
            }

            if (Math.Abs(color1.B - color2.B) >= singleValue)
            {
                retval = false;
            }

            if (Math.Abs(color1.R + color1.G + color1.B - color2.R - color2.G - color2.B) >= rgbValue)
            {
                retval = false;
            }

            return retval;
        }
    }
}
