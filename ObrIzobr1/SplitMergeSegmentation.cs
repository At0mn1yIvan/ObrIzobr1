using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;


namespace ObrIzobr1
{
    public class SplitMergeSegmentation
    {
        public static byte[] SplitMerge(byte[] buffer, double m, double s, int suite_min_q_len, int dont_suite_min_q_len)
        {
            byte[][] split_bytes = new byte[4][]; //массив для хранения четырех частей изображения.
            int quad_len = buffer.Length / 4; //размер(длина) квадранта изображения.
            int half = (int)Math.Sqrt(buffer.Length / 3) / 2; //половина размера стороны квадрата изображения.
            int stride = 6 * half; //шаг для обращения к пикселям в массиве buffer

            //Split
            for (int i = 0; i < 2; i++) // циклы по квадратнам
            {
                for (int j = 0; j < 2; j++)
                {
                    split_bytes[i + j * 2] = new byte[quad_len]; // массив для хранения данных текущего квадранта
                    for (int x = i * half; x < (i + 1) * half; x++) // циклы по гор. в квадранте
                    {
                        for (int y = j * half; y < (j + 1) * half; y++) // циклы по вер. в квадранте
                        {
                            int position = x * 3 + y * stride; //позиция текущего пикселя в массиве buffer.
                            int quad_position = (x - i * half) * 3 + (y - j * half) * half * 3; //позиция текущего пикселя в массиве текущего квадранта.
                            for (int c = 0; c < 3; c++) //Цикл по компонентам цвета (RGB).
                            {
                                split_bytes[i + j * 2][quad_position + c] = buffer[position + c]; //Копирование значений цветов текущего пикселя в массив текущего квадранта.
                            }
                        }
                    }

                    double mean = 0; //среднее значение цветов в текущем квадранте.
                    for (int k = 0; k < quad_len; k += 3) // Цикл по значениям цветов в текущем квадранте.
                    {
                        mean += split_bytes[i + j * 2][k];
                    }
                    mean /= Math.Pow(half, 2);

                    double stdColorVariance = 0; //стандартное отклонение цветов в текущем квадранте.
                    for (int k = 0; k < quad_len; k += 3)
                    {
                        stdColorVariance += Math.Pow(split_bytes[i + j * 2][k] - mean, 2);
                    }
                    stdColorVariance /= Math.Pow(half, 2);

                    if (stdColorVariance > s && mean > 0 && mean < m)
                    {
                        if (quad_len >= suite_min_q_len)
                        {
                            split_bytes[i + j * 2] = SplitMerge(split_bytes[i + j * 2], m, s, suite_min_q_len, dont_suite_min_q_len);
                        }
                        else
                        {
                            //split_bytes[i + j * 2] = split_bytes[i + j * 2].Select(x => (byte)Math.Abs(255 - mean)).ToArray();
                            split_bytes[i + j * 2] = split_bytes[i + j * 2].Select(x => (byte)(m - mean)).ToArray();
                        }
                    }
                    else
                    {
                        if (quad_len >= dont_suite_min_q_len)
                        {
                            split_bytes[i + j * 2] = SplitMerge(split_bytes[i + j * 2], m, s, suite_min_q_len, dont_suite_min_q_len);
                        }
                        else
                        {
                            //split_bytes[i + j * 2] = split_bytes[i + j * 2].Select(x => (byte)((mean > 0 && mean < m) ? (int)(m/2) : (mean > 0 ? Math.Abs(128 - mean) : mean + 10))).ToArray();
                            split_bytes[i + j * 2] = split_bytes[i + j * 2].Select(x => (byte)mean).ToArray();


                        }
                    }
                }
            }

            //Merge
            byte[] result = new byte[buffer.Length];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int x = i * half; x < (i + 1) * half; x++)
                    {
                        for (int y = j * half; y < (j + 1) * half; y++)
                        {
                            int position = x * 3 + y * stride;
                            int quad_position = (x - i * half) * 3 + (y - j * half) * half * 3;
                            for (int c = 0; c < 3; c++)
                            {
                                result[position + c] = split_bytes[i + j * 2][quad_position + c];
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static Bitmap SegmentSM(Bitmap image, double mean, double stdColorVariance, int suite_min_q_len, int dont_suite_min_q_len)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb); // Получение данных изображения для дальнейшей обработки.

            int bytes = image_data.Stride * image_data.Height; // общее число байтов в изобр.
            byte[] buffer = new byte[bytes]; // буфер для хранения данных изображения.

            Marshal.Copy(image_data.Scan0, buffer, 0, bytes); //  Копирование данных изображения в буфер.
            image.UnlockBits(image_data); //  Освобождение данных изображения

            // вычисление размера квадратного изображения
            int padded_sq_dim = new int();
            int n = 0;

            while (padded_sq_dim <= Math.Max(w, h))
            {
                padded_sq_dim = (int)Math.Pow(2, n);
                if (padded_sq_dim == Math.Max(w, h))
                {
                    break;
                }
                n++;
            }

            // Вычисление отступов для центрирования исходного изображения в квадратном
            int left_pad = (int)Math.Floor((double)padded_sq_dim - w) / 2;
            int top_pad = (int)Math.Floor((double)padded_sq_dim - h) / 2;

            Bitmap padded = new Bitmap(padded_sq_dim, padded_sq_dim); // Создание нового квадратного изображения.
            BitmapData padded_data = padded.LockBits(
                new Rectangle(0, 0, padded_sq_dim, padded_sq_dim),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            int pad_bytes = padded_data.Stride * padded_data.Height; // вычисление количества байтов в данных квадратного изображения.
            byte[] padded_result = new byte[pad_bytes]; // буфер для хранения данных квадратного изображения.

            // заполнение изображения фоном
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int image_position = x * 3 + y * image_data.Stride;
                    int padded_position = x * 3 + y * padded_data.Stride;
                    for (int c = 0; c < 3; c++)
                    {
                        padded_result[padded_position + 3 * left_pad + top_pad * padded_data.Stride + c] = buffer[image_position + c];
                    }
                }
            }

            padded_result = SplitMerge(padded_result, mean, stdColorVariance, suite_min_q_len, dont_suite_min_q_len);

            Marshal.Copy(padded_result, 0, padded_data.Scan0, pad_bytes); // Копирование результата обратно в данные квадратного изображения.
            padded.UnlockBits(padded_data); // Освобождение данных квадратного изображения

            return padded;
        }
    }
}
