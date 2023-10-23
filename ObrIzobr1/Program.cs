


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


            /*Noise rlNoise = new Noise("C:\\Users\\admin\\Downloads\\originalPic.jpg");
            rlNoise.RaylieghNoise(7, 11000).Save("C:\\Users\\admin\\Downloads\\rlNoisePic.jpg", ImageFormat.Jpeg);
            Bitmap nonLocalF = NonLocalMeansFilter.ApplyFilter(rlNoise.updatedImage, 2, 1, 250);
            nonLocalF.Save("C:\\Users\\admin\\Downloads\\rlNonLocalMeansFilterPic.jpg", ImageFormat.Jpeg);

            Console.WriteLine("Оригинал vs Шум:\n");
            ImageProcessing compare1 = new ImageProcessing(rlNoise.originalImage, rlNoise.updatedImage);
            Console.WriteLine(compare1.MSE(0, compare1.image1.Width, 0, compare1.image1.Height));
            Console.WriteLine(compare1.MSE_Part(15, 15));
            Console.WriteLine(compare1.UQI(0, compare1.image1.Width, 0, compare1.image1.Height));
            Console.WriteLine(compare1.UQI_Part(15, 15));


            Console.WriteLine("Оригинал vs Шум + Фильтр:\n");
            ImageProcessing compare2 = new ImageProcessing(rlNoise.originalImage, nonLocalF);
            Console.WriteLine(compare2.MSE(0, compare2.image1.Width, 0, compare2.image1.Height));
            Console.WriteLine(compare2.MSE_Part(15, 15));
            Console.WriteLine(compare2.UQI(0, compare2.image1.Width, 0, compare2.image1.Height));
            Console.WriteLine(compare2.UQI_Part(15, 15));*/


            // 3, 2, 500
            //517,1475089064019
            //0,04702436215768828
            //0,9924007558731298
            //0,06507364927652855

            //4, 2, 250
            /* 375,4538756342438
             11,193792592592594
             0,9860089167691518
             0,06601543576756154*/


            Bitmap originalImage = new Bitmap("C:\\Users\\khram\\Downloads\\Cat2.jpg");

            HoughSquareDetection h = new HoughSquareDetection(originalImage);

            Bitmap h2 = h.DetectSquares();

            h2.Save("C:\\Users\\khram\\Downloads\\CatH.jpg", ImageFormat.Jpeg);
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



    public class HoughSquareDetection
    {
        private Bitmap inputImage;
        private int[,,] houghSpace;

        public HoughSquareDetection(Bitmap image)
        {
            inputImage = image;
        }

        public Bitmap DetectSquares()
        {
            // Шаг 1: Преобразование в оттенки серого и поиск горизонтальных и вертикальных краев
            Bitmap grayImage = ConvertToGray(inputImage);
            Bitmap edgesImage = DetectEdges(grayImage);

            // Шаг 2: Построение пространства параметров
            BuildHoughSpace(edgesImage);

            // Шаг 3: Голосование
            VoteInHoughSpace(edgesImage);

            // Шаг 4: Поиск максимумов
            List<Point> detectedSquares = FindMax();

            // Шаг 5: Фильтрация результатов
            int minSize = 20; // Установите минимальный размер квадрата // 20
            int minVotes = 200; // Установите минимальное количество голосов // 200
            List<Point> filteredSquares = FilterSquares(detectedSquares, minSize, minVotes);

            // Шаг 6: Отрисовка квадратов на исходном изображении
            Bitmap resultImage = DrawSquares(inputImage, filteredSquares, minSize * 2); // Умножьте минимальный размер на 2, чтобы получить сторону квадрата

            return resultImage;
        }

        private Bitmap ConvertToGray(Bitmap image)
        {
            // Реализация преобразования изображения в оттенки серого

            int width = image.Width;
            int height = image.Height;

            // Создайте новое изображение в оттенках серого
            Bitmap grayImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Получите цвет пикселя
                    Color color = image.GetPixel(x, y);

                    // Вычислите среднее значение RGB для получения оттенка серого
                    int grayValue = (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);

                    // Создайте цвет оттенка серого
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);

                    // Установите цвет пикселя в оттенок серого
                    grayImage.SetPixel(x, y, grayColor);
                }
            }

            return grayImage;
        }

        private Bitmap DetectEdges(Bitmap grayImage)
        {
            // Реализация поиска горизонтальных и вертикальных краев
            int width = grayImage.Width;
            int height = grayImage.Height;

            Bitmap edgesImage = new Bitmap(width, height);

            int[,] horizontalSobel = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] verticalSobel = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int horizontalGradient = 0;
                    int verticalGradient = 0;

                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            Color pixel = grayImage.GetPixel(x + i, y + j);
                            int grayValue = pixel.R; // Используем красную компоненту изображения в оттенках серого

                            horizontalGradient += grayValue * horizontalSobel[i + 1, j + 1];
                            verticalGradient += grayValue * verticalSobel[i + 1, j + 1];
                        }
                    }

                    int gradientMagnitude = (int)Math.Sqrt(horizontalGradient * horizontalGradient + verticalGradient * verticalGradient);
                    gradientMagnitude = Math.Min(255, gradientMagnitude); // Ограничиваем значение до 255

                    Color edgeColor = Color.FromArgb(gradientMagnitude, gradientMagnitude, gradientMagnitude);
                    edgesImage.SetPixel(x, y, edgeColor);
                }
            }

            return edgesImage;
        }

        private void BuildHoughSpace(Bitmap edgesImage)
        {
            // Реализация построения пространства параметров

            int width = edgesImage.Width;
            int height = edgesImage.Height;

            int maxSquareSize = Math.Min(width, height) / 4; // Максимальный размер квадрата
            int minSquareSize = 20; // Минимальный размер квадрата

            int xCenter, yCenter, squareSize; // Параметры квадрата

            houghSpace = new int[maxSquareSize - minSquareSize, width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (edgesImage.GetPixel(x, y).R == 255) // Если пиксель находится на крае
                    {
                        for (int size = minSquareSize; size < maxSquareSize; size++)
                        {
                            // Вычисляем возможные координаты центра квадрата
                            for (int angle = 0; angle < 360; angle++)
                            {
                                double angleInRadians = angle * Math.PI / 180.0;
                                xCenter = (int)(x - size * Math.Cos(angleInRadians));
                                yCenter = (int)(y - size * Math.Sin(angleInRadians));

                                // Увеличиваем счетчик в соответствующей ячейке в Hough Space
                                if (xCenter >= 0 && xCenter < width && yCenter >= 0 && yCenter < height)
                                {
                                    houghSpace[size - minSquareSize, xCenter, yCenter]++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void VoteInHoughSpace(Bitmap edgesImage)
        {
            // Реализация голосования
            int width = edgesImage.Width;
            int height = edgesImage.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (edgesImage.GetPixel(x, y).R == 255) // Если пиксель находится на крае
                    {
                        for (int size = 0; size < houghSpace.GetLength(0); size++)
                        {
                            for (int angle = 0; angle < 360; angle++)
                            {
                                double angleInRadians = angle * Math.PI / 180.0;
                                int xCenter = (int)(x - size * Math.Cos(angleInRadians));
                                int yCenter = (int)(y - size * Math.Sin(angleInRadians));

                                if (xCenter >= 0 && xCenter < width && yCenter >= 0 && yCenter < height)
                                {
                                    houghSpace[size, xCenter, yCenter]++;
                                }
                            }
                        }
                    }
                }
            }
        }


        private List<Point> FindMax()
        {
            // Реализация поиска максимумов в пространстве параметров

            List<Point> maxima = new List<Point>();
            int size = houghSpace.GetLength(0);
            int width = houghSpace.GetLength(1);
            int height = houghSpace.GetLength(2);

            for (int s = 0; s < size; s++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int currentValue = houghSpace[s, x, y];
                        bool isMaximum = true;

                        // Проверяем окружающие ячейки на предмет меньших значений
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                for (int k = -1; k <= 1; k++)
                                {
                                    int neighborX = x + i;
                                    int neighborY = y + j;
                                    int neighborSize = s + k;

                                    if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height && neighborSize >= 0 && neighborSize < size)
                                    {
                                        if (houghSpace[neighborSize, neighborX, neighborY] > currentValue)
                                        {
                                            isMaximum = false;
                                            break;
                                        }
                                    }
                                }
                                if (!isMaximum) break;
                            }
                            if (!isMaximum) break;
                        }

                        if (isMaximum)
                        {
                            maxima.Add(new Point(x, y));
                        }
                    }
                }
            }

            return maxima;
        }

        private Bitmap DrawSquares(Bitmap image, List<Point> squares, int squareSize)
        {
            // Реализация отрисовки квадратов на исходном изображении

            Bitmap resultImage = new Bitmap(image);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            {
                Pen pen = new Pen(Color.Red, 1); // Цвет и ширина линии для отрисовки квадратов

                foreach (Point squareCenter in squares)
                {
                    int halfSize = squareSize / 2;
                    int x = squareCenter.X - halfSize;
                    int y = squareCenter.Y - halfSize;

                    // Рисуем квадрат на изображении
                    graphics.DrawRectangle(pen, x, y, squareSize, squareSize);
                }
            }

            return resultImage;
        }

        private int GetSquareSizeFromHoughSpace(Point square, int minSquareSize)
        {
            // Предполагаем, что размер квадрата хранится в первом измерении пространства параметров Hough
            int sizeIndex = 0;

            // Извлекаем размер квадрата из пространства параметров Hough
            int size = sizeIndex + minSquareSize;

            return size;
        }

        private List<Point> FilterSquares(List<Point> detectedSquares, int minSize, int minVotes)
        {
            List<Point> filteredSquares = new List<Point>();

            foreach (Point square in detectedSquares)
            {
                int x = square.X;
                int y = square.Y;
                int size = GetSquareSizeFromHoughSpace(square, minSize); // Реализуйте этот метод для получения размера квадрата из пространства параметров Hough

                if (size >= minSize && houghSpace[size, x, y] >= minVotes)
                {
                    filteredSquares.Add(square);
                }
            }

            return filteredSquares;
        }
    }

}


// Выделение границ и поиск объектов при помощи методов преобразования Хаффа. Метод Щара и поиск квадрата. 