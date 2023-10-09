


using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Cryptography;

namespace ObrIzobr1
{

    internal class Program
    {

        static void Main(string[] args)
        {
            /*Console.WriteLine("MSE разные:");
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
            Console.WriteLine(img6_p.UQI_Part(15, 15));*/


            Noise rlNoise = new Noise("C:\\Users\\khram\\Downloads\\originalPic.jpg");
            rlNoise.RaylieghNoise(7, 9000).Save("C:\\Users\\khram\\Downloads\\rlNoisePic.jpg", ImageFormat.Jpeg);
            Bitmap nonLocalF = NonLocalMeansFilter.ApplyFilter(rlNoise.updatedImage, 3, 2, 500);
            nonLocalF.Save("C:\\Users\\khram\\Downloads\\rlNonLocalMeansFilterPic.jpg", ImageFormat.Jpeg);

            ImageProcessing compare = new ImageProcessing(rlNoise.originalImage, nonLocalF);
            Console.WriteLine(compare.MSE(0, compare.image1.Width, 0, compare.image1.Height));
            Console.WriteLine(compare.MSE_Part(15, 15));
            Console.WriteLine(compare.UQI(0, compare.image1.Width, 0, compare.image1.Height));
            Console.WriteLine(compare.UQI_Part(15, 15));
        }

    }

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
                    p[z] = 0;
                else
                    p[z] = p[z - 1] + 2 / b * (z - a) * Math.Exp(-Math.Pow(z - a, 2) / b);
            }
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



    public class NonLocalMeansFilter
    {
        public static Bitmap ApplyFilter(Bitmap inputImage, int searchWindowRadius, int comparisonWindowRadius, double h)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap resultImage = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double sumWeights = 0; // сумма весов для текущего пикселя
                    double sumWeightedValues = 0; // сумма взвешенных значений для текущего пикселя

                    for (int i = -searchWindowRadius; i <= searchWindowRadius; i++)
                    {
                        for (int j = -searchWindowRadius; j <= searchWindowRadius; j++)
                        {
                            int neighborX = x + i;
                            int neighborY = y + j;

                            //Проверяется, что пиксель находится в пределах изображения
                            if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                            {
                                double similarity = ComputeSimilarity(inputImage, x, y, neighborX, neighborY, comparisonWindowRadius);

                                double weight = Math.Exp(-similarity / (h * h));
                                sumWeightedValues += weight * inputImage.GetPixel(neighborX, neighborY).R;
                                sumWeights += weight;
                            }
                        }
                    }

                    int newValue = (int)(sumWeightedValues / sumWeights);
                    resultImage.SetPixel(x, y, Color.FromArgb(newValue, newValue, newValue));
                }
            }

            return resultImage;
        }

        private static double ComputeSimilarity(Bitmap image, int x1, int y1, int x2, int y2, int windowRadius)
        {
            // вычисляет сходство между двумя окнами вокруг 2 пикселей 
            double sumSquaredDiff = 0;

            // проходится по каждому пикселю в окне сравнения и вычисляет сумму квадратов разницы интенсивности пикселей в окнах
            for (int i = -windowRadius; i <= windowRadius; i++)
            {
                for (int j = -windowRadius; j <= windowRadius; j++)
                {
                    int pixelX1 = x1 + i;
                    int pixelY1 = y1 + j;
                    int pixelX2 = x2 + i;
                    int pixelY2 = y2 + j;

                    if (pixelX1 >= 0 && pixelX1 < image.Width && pixelY1 >= 0 && pixelY1 < image.Height &&
                pixelX2 >= 0 && pixelX2 < image.Width && pixelY2 >= 0 && pixelY2 < image.Height)
                    {
                        Color color1 = image.GetPixel(pixelX1, pixelY1);
                        Color color2 = image.GetPixel(pixelX2, pixelY2);

                        int diff = color1.R - color2.R;
                        sumSquaredDiff += diff * diff;
                    }
                }
            }

            return sumSquaredDiff;
        }
    }
}