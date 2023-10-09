


using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;

namespace ObrIzobr1 {

    internal class Program
    {
           
        static void Main(string[] args)
        {
            Console.WriteLine("MSE разные:");
            ImageProcessing img1 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im2.jpg");
            Console.WriteLine(img1.MSE(0, img1.image1.Width, 0, img1.image1.Height));
            Console.WriteLine("MSE_Part разные:");
            ImageProcessing img1_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im2.jpg");
            Console.WriteLine(img1_p.MSE_Part(15, 15));


            Console.WriteLine("MSE одинаковые:");
            ImageProcessing img2 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im1.jpg");
            Console.WriteLine(img2.MSE(0, img2.image1.Width, 0, img2.image1.Height));
            Console.WriteLine("MSE_Part одинаковые:");
            ImageProcessing img2_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im1.jpg");
            Console.WriteLine(img2_p.MSE_Part(15,15));

            Console.WriteLine("MSE контраст:");
            ImageProcessing img3 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im3.jpg", "C:\\Users\\admin\\Downloads\\im4.jpg");
            Console.WriteLine(img3.MSE(0, img3.image1.Width, 0, img3.image1.Height));
            Console.WriteLine("MSE_Part контраст:");
            ImageProcessing img3_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im3.jpg", "C:\\Users\\admin\\Downloads\\im4.jpg");
            Console.WriteLine(img3_p.MSE_Part(15, 15));


            Console.WriteLine("UQI разные:");
            ImageProcessing img4 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im2.jpg");
            Console.WriteLine(img4.UQI(0, img4.image1.Width, 0, img4.image1.Height));
            Console.WriteLine("UQI_Part разные:");
            ImageProcessing img4_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im2.jpg");
            Console.WriteLine(img4_p.UQI_Part(15, 15));


            Console.WriteLine("UQI одинаковые:");
            ImageProcessing img5 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im1.jpg");
            Console.WriteLine(img5.UQI(0, img5.image1.Width, 0, img5.image1.Height));
            Console.WriteLine("UQI_Part одинаковые:");
            ImageProcessing img5_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im1.jpg", "C:\\Users\\admin\\Downloads\\im1.jpg");
            Console.WriteLine(img5_p.UQI_Part(15, 15));

            Console.WriteLine("UQI контраст:");
            ImageProcessing img6 = new ImageProcessing("C:\\Users\\admin\\Downloads\\im3.jpg", "C:\\Users\\admin\\Downloads\\im4.jpg");
            Console.WriteLine(img6.UQI(0, img6.image1.Width, 0, img6.image1.Height));
            Console.WriteLine("UQI_Part контраст:");
            ImageProcessing img6_p = new ImageProcessing("C:\\Users\\admin\\Downloads\\im3.jpg", "C:\\Users\\admin\\Downloads\\im4.jpg");
            Console.WriteLine(img6_p.UQI_Part(15, 15));

        }

    }

    public class ImageProcessing {
        public Bitmap image1 { get; set; }
        public Bitmap image2 { get; set; }

        public ImageProcessing(String im1, String im2)
        {
            image1 = new Bitmap(im1);
            image2 = new Bitmap(im2);
        }

        public double MSE(int x1, int x2, int y1, int y2)
        {
            double res = 0;
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    res += Math.Pow((image1.GetPixel(i, j).G - image2.GetPixel(i, j).G), 2);
                }
            }
            return res / ((double)image1.Height * (double)image2.Width);
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
    // для очищения - фильтр нелокального среднего
    public class Noise
    {
        public Bitmap originalImage { get; set; }
        public Bitmap updatedImage { get; set; }
        public Bitmap clearedImage { get; set; }

        public Noise(String im)
        {
            originalImage = new Bitmap(im);
        }

        public Bitmap RaylieghNoise(double a, double b)
        {
            double[] p = new double[256];

            for (int z = 0; z < 256; z++)
            {
                if (z < a)
                    p[z] = 0;
                else
                    p[z] = 2 / b * (z - a) * Math.Exp(-Math.Pow(z - a, 2) / b);
            }
            Random random = new Random();
            for (int i = 0; i < originalImage.Width; i++)
                for (int j = 0; j < originalImage.Height; j++)
                    if (random.Next(100) < 5)
                    {
                         
                    }
        }


    }
}