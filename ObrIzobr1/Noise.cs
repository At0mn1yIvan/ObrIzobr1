using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObrIzobr1
{
    public class Noise
    {
        public Bitmap originalImage { get; set; }
        public Bitmap updatedImage { get; set; }


        public Noise(string im)
        {
            originalImage = new Bitmap(im);
            updatedImage = new Bitmap(im);
        }


        public Bitmap RaylieghNoise(double a, double b)
        {
            double[] p = new double[256];

            int width = originalImage.Width;
            int height = originalImage.Height;


            for (int z = 0; z < 256; z++)
            {
                if (z < a)
                {
                    p[z] = 0;
                    Console.WriteLine(p[z]);
                }
                else
                {
                    p[z] = p[z - 1] + 2 / b * (z - a) * Math.Exp(-Math.Pow(z - a, 2) / b);
                    Console.WriteLine(2 / b * (z - a) * Math.Exp(-Math.Pow(z - a, 2) / b));
                }
            }
            Console.WriteLine();
            //метод Монте-Карло 
            Random random = new Random();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (random.Next(100) <= 5)
                    {
                        double noise = random.NextDouble();
                        for (int k = 0; k < 256; k++)
                        {
                            if (noise < p[k])
                            {
                                updatedImage.SetPixel(i, j, Color.FromArgb(k - 1, k - 1, k - 1));
                                break;
                            }
                        }
                    }
                    else
                        updatedImage.SetPixel(i, j, originalImage.GetPixel(i, j));
                }
            return updatedImage;
        }
    }
}
