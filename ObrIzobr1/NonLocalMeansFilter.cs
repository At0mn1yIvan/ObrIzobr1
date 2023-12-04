using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObrIzobr1
{
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
