using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObrIzobr1
{
    public class HoughDetectSquares2
    {

        // Предобработка изображения
        public static Bitmap TurnToGrayUpContrast(Bitmap originalImage)
        {
            int imWidth = originalImage.Width;
            int imHeight = originalImage.Height;

            Bitmap grayImage = new Bitmap(imWidth, imHeight);

            int minBrightness = int.MaxValue;
            int maxBrightness = int.MinValue;

            for (int x = 0; x < imWidth; x++)
            {
                for (int y = 0; y < imHeight; y++)
                {
                    Color pixColor = originalImage.GetPixel(x, y);

                    int grayValue = (int)(0.299 * pixColor.R + 0.587 * pixColor.G + 0.114 * pixColor.B);

                    minBrightness = Math.Min(minBrightness, grayValue);
                    maxBrightness = Math.Max(maxBrightness, grayValue);
                }
            }

            for (int x = 0; x < imWidth; x++)
            {
                for (int y = 0; y < imHeight; y++)
                {
                    Color pixColor = originalImage.GetPixel(x, y);

                    int grayValue = (int)(0.299 * pixColor.R + 0.587 * pixColor.G + 0.114 * pixColor.B);

                    int stretchedBrightness = (int)(255.0 * (grayValue - minBrightness) / (maxBrightness - minBrightness));

                    grayImage.SetPixel(x, y, Color.FromArgb(stretchedBrightness, stretchedBrightness, stretchedBrightness));
                }
            }

            return grayImage;
        }

        public static Bitmap ApplySobelFilter(Bitmap originalImage, int[,] filter)
        {
            int imWidth = originalImage.Width;
            int imHeight = originalImage.Height;

            int filterSize = filter.GetLength(0);
            int radius = filterSize / 2;

            Bitmap result = new Bitmap(imWidth, imHeight);

            for (int x = radius; x < imWidth - radius; x++)
            {
                for (int y = radius; y < imHeight - radius; y++)
                {
                    int newValue = 0;

                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            Color pixel = originalImage.GetPixel(x + i, y + j);
                            int grayValue = pixel.R;

                            newValue += grayValue * filter[i + radius, j + radius];
                        }
                    }

                    newValue = Math.Min(Math.Max(newValue, 0), 255);
                    result.SetPixel(x, y, Color.FromArgb(newValue, newValue, newValue));
                }
            }

            return result;
        }

        public static Bitmap CombineEdgeImages(Bitmap horizontalEdgesImage, Bitmap verticalEdgesImage)
        {
            int imWidth = horizontalEdgesImage.Width;
            int imHeight = horizontalEdgesImage.Height;

            Bitmap result = new Bitmap(imWidth, imHeight);

            for (int x = 0; x < imWidth; x++)
            {
                for (int y = 0; y < imHeight; y++)
                {
                    Color horizontalColor = horizontalEdgesImage.GetPixel(x, y);
                    Color verticalColor = verticalEdgesImage.GetPixel(x, y);

                    int combinedValue = Math.Min(horizontalColor.R + verticalColor.R, 255);

                    result.SetPixel(x, y, Color.FromArgb(combinedValue, combinedValue, combinedValue));
                }
            }

            return result;
        }

        public static Bitmap ApplySobelOperator(Bitmap originalImage)
        {
            int[,] xKernel = new int[,] {
                {-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}
            };

            int[,] yKernel = new int[,] {
                { 1, 2, 1},
                { 0, 0, 0},
                {-1,-2,-1}
            };

            Bitmap grayImage = TurnToGrayUpContrast(originalImage);

            Bitmap horizontalEdges = ApplySobelFilter(grayImage, xKernel);
            Bitmap verticalEdges = ApplySobelFilter(grayImage, yKernel);

            Bitmap resultImage = CombineEdgeImages(horizontalEdges, verticalEdges);

            return resultImage;
        }

    }
}
