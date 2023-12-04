using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObrIzobr1
{
    public class HoughDetectSquares
    {
        public static Bitmap ApplySobelOperator(Bitmap inputImage)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap outputImage = new Bitmap(width, height);

            int[,] kernelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] kernelY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int pixelX = Convolve(inputImage, x, y, kernelX);
                    int pixelY = Convolve(inputImage, x, y, kernelY);

                    int gradient = (int)Math.Sqrt(pixelX * pixelX + pixelY * pixelY);
                    gradient = Math.Min(255, Math.Max(0, gradient));

                    outputImage.SetPixel(x, y, Color.FromArgb(gradient, gradient, gradient));
                }
            }

            return outputImage;
        }

        private static int Convolve(Bitmap image, int x, int y, int[,] kernel)
        {
            int result = 0;
            int kernelSize = kernel.GetLength(0);

            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    result += kernel[i, j] * image.GetPixel(x + j - 1, y + i - 1).R;
                }
            }

            return result;
        }

        public static Bitmap ApplyThreshold(Bitmap inputImage, int threshold)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap binaryImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = inputImage.GetPixel(x, y);
                    int grayscaleValue = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                    // Применение пороговой обработки
                    int binaryValue = (grayscaleValue > threshold) ? 255 : 0;

                    binaryImage.SetPixel(x, y, Color.FromArgb(binaryValue, binaryValue, binaryValue));
                }
            }

            return binaryImage;
        }

        public static int[,] ApplyHoughTransform(Bitmap binaryImage)
        {
            int width = binaryImage.Width;
            int height = binaryImage.Height;

            double maxDistance = Math.Sqrt(width * width + height * height);
            int numAngles = 180;
            int[,] accumulator = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (binaryImage.GetPixel(x, y).R == 255)
                    {
                        for (int theta = 0; theta < numAngles; theta++)
                        {
                            double angleRadians = Math.PI * theta / numAngles;
                            double distance = x * Math.Cos(angleRadians) + y * Math.Sin(angleRadians);
                            int normalizedDistance = (int)((distance + maxDistance) / (2 * maxDistance) * (width - 1));

                            accumulator[normalizedDistance, theta]++;
                        }
                    }
                }
            }

            return accumulator;
        }

        public static List<Tuple<int, int>> FindMaxima(int[,] accumulator, int threshold)
        {
            List<Tuple<int, int>> peaks = new List<Tuple<int, int>>();

            int width = accumulator.GetLength(0);
            int height = accumulator.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (accumulator[x, y] > threshold)
                    {
                        peaks.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            return peaks;
        }

        public static Bitmap VisualizePeaks(Bitmap binaryImage, List<Tuple<int, int>> peaks)
        {
            Bitmap resultImage = new Bitmap(binaryImage);

            foreach (var peak in peaks)
            {
                int x = peak.Item1;
                int y = peak.Item2;

                // Визуализация максимальных пиков на исходном изображении (красные пиксели)
                resultImage.SetPixel(x, y, Color.Red);
            }

            return resultImage;
        }

        public static List<Line> ConvertParametersToLines(List<Tuple<int, int>> peaks, Bitmap binaryImage)
        {
            List<Line> lines = new List<Line>();

            int width = binaryImage.Width;
            int height = binaryImage.Height;
            double maxDistance = Math.Sqrt(width * width + height * height);
            int numAngles = 180;

            foreach (var peak in peaks)
            {
                int distance = peak.Item1;
                int theta = peak.Item2;

                double angleRadians = Math.PI * theta / numAngles;

                int startX = 0;
                int endX = width - 1;

                int startY = (int)((distance - startX * Math.Cos(angleRadians)) / Math.Sin(angleRadians));
                int endY = (int)((distance - endX * Math.Cos(angleRadians)) / Math.Sin(angleRadians));

                lines.Add(new Line(startX, startY, endX, endY));
            }

            return lines;
        }

        public static Bitmap VisualizeLines(Bitmap binaryImage, List<Line> lines)
        {
            Bitmap resultImage = new Bitmap(binaryImage);

            foreach (var line in lines)
            {
                // Визуализация прямых линий на исходном изображении (зеленые пиксели)
                DrawLine(resultImage, line, Color.Black);
            }

            return resultImage;
        }

        public static void DrawLine(Bitmap image, Line line, Color color)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(color, 5);
                g.DrawLine(pen, line.StartX, line.StartY, line.EndX, line.EndY);
            }
        }

        public static List<Line> FilterLines(List<Line> lines)
        {
            List<Line> filteredLines = new List<Line>();

            // Критерии для фильтрации линий (вы можете настроить их в соответствии с вашими требованиями)
            int minLength = 20; // Минимальная длина линии
            int maxLength = 400; // Максимальная длина линии
            double maxAngleDifference = 10.0; // Максимальное различие углов между линиями

            // Фильтрация линий
            for (int i = 0; i < lines.Count; i++)
            {
                Line line1 = lines[i];

                if (line1.Length >= minLength && line1.Length <= maxLength)
                {
                    bool isSquareCandidate = true;

                    for (int j = 0; j < lines.Count; j++)
                    {
                        if (i != j)
                        {
                            Line line2 = lines[j];

                            // Различие углов между линиями
                            double angleDifference = Math.Abs(line1.Angle - line2.Angle);

                            if (angleDifference < maxAngleDifference)
                            {
                                isSquareCandidate = false;
                                break;
                            }
                        }
                    }

                    if (isSquareCandidate)
                    {
                        filteredLines.Add(line1);
                    }
                }
            }

            return filteredLines;
        }

        public static Bitmap HighlightSquares(Bitmap originalImage, List<Line> lines)
        {
            Bitmap resultImage = new Bitmap(originalImage);

            using (Graphics g = Graphics.FromImage(resultImage))
            {
                foreach (var line in lines)
                {
                    // Рисование прямоугольника вокруг найденного квадрата (синие грани)
                    g.DrawRectangle(new Pen(Color.Black, 10), line.StartX, line.StartY, line.Length, line.Length);
                }
            }

            return resultImage;
        }
    }

    public class Line
    {
        public int StartX { get; }
        public int StartY { get; }
        public int EndX { get; }
        public int EndY { get; }

        public int Length => (int)Math.Sqrt(Math.Pow(EndX - StartX, 2) + Math.Pow(EndY - StartY, 2));
        public double Angle => Math.Atan2(EndY - StartY, EndX - StartX);

        public Line(int startX, int startY, int endX, int endY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }
    }
}
