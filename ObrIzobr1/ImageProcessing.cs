using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObrIzobr1
{
    public class ImageProcessing
    {
        public Bitmap image1 { get; set; }
        public Bitmap image2 { get; set; }

        public ImageProcessing(string im1, string im2)
        {
            image1 = new Bitmap(im1);
            image2 = new Bitmap(im2);
        }

        public ImageProcessing(Bitmap im1, Bitmap im2)
        {
            image1 = im1;
            image2 = im2;
        }

        public double MSE(int x1, int x2, int y1, int y2)
        {
            double res = 0;
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    res += Math.Pow(image1.GetPixel(i, j).G - image2.GetPixel(i, j).G, 2);
                }
            }
            return res / (x2 - x1) / (y2 - y1);
        }

        public double UQI(int x1, int x2, int y1, int y2)
        {
            double avgImg1 = 0;
            double avgImg2 = 0;
            double Sx = 0;
            double Sy = 0;
            double c1 = 0.0001;
            double c2 = 0.0003;
            double Sxy = 0;
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    avgImg1 += image1.GetPixel(i, j).G;
                    avgImg2 += image2.GetPixel(i, j).G;
                }
            }
            avgImg1 /= (double)image1.Height * (double)image1.Width;
            avgImg2 /= (double)image2.Height * (double)image2.Width;

            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    Sx = Math.Pow(image1.GetPixel(i, j).G - avgImg1, 2);
                    Sy = Math.Pow(image2.GetPixel(i, j).G - avgImg2, 2);
                    Sxy = (image1.GetPixel(i, j).G - avgImg1) * (image2.GetPixel(i, j).G - avgImg2);
                }
            }
            Sx /= (double)image1.Height * (double)image1.Width;
            Sy /= (double)image2.Height * (double)image2.Width;
            Sxy /= (double)image2.Height * (double)image2.Width;

            return ((c2 + 4 * Sxy) * (avgImg1 * avgImg2 + c1)) / ((Sx + Sy + c2) * (Math.Pow(avgImg1, 2) + Math.Pow(avgImg2, 2) + c1));
        }

        public double UQI_Part(int x, int y)
        {
            double res = 0;
            int sectorX = 0, sectorY = 0;
            for (; sectorX < image1.Width / x; sectorX++)
                for (; sectorY < image1.Height / y; sectorY++)
                    res += UQI(sectorX * x, (sectorX + 1) * x, sectorY * y, (sectorY + 1) * y);
            return res / (sectorX * sectorY);
        }


        public double MSE_Part(int x, int y)
        {
            double res = 0;
            int sectorX = 0, sectorY = 0;
            for (; sectorX < image1.Width / x; sectorX++)
                for (; sectorY < image1.Height / y; sectorY++)
                    res += MSE(sectorX * x, (sectorX + 1) * x, sectorY * y, (sectorY + 1) * y);
            return res / (sectorX * sectorY);
        }
    }
}
