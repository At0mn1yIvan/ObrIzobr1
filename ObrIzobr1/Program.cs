

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;


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


            //Хафф и квадраты
            //Bitmap originalImage = new Bitmap("C:\\Users\\khram\\Downloads\\1.jpg");
           
 
            // Split&Merge
            // Загрузка изображения
            Bitmap originalImage = new Bitmap("C:\\Users\\khram\\Downloads\\ocean.jpg");


            // Применение алгоритма Split & Merge
            // 1) Среднее значение цветов в регионе (средняя яркость)
            // 2) Стандартное отклонение цветов в регионе (разброс значений яркости в регионе)

            int[] suiteMaxQArr = new int[] { 40, 120, 360, 720, 1100 };
            int[] dontSuiteMaxQArr = new int[] { 10, 30, 90, 180, 310 };
            for (int i = 0; i < suiteMaxQArr.Length; i++)
            {
                Bitmap segmentedImage = SplitMergeSegmentation.SegmentSM(originalImage, 100, 25, suiteMaxQArr[i], dontSuiteMaxQArr[i]); 
                segmentedImage.Save($"C:\\Users\\khram\\Downloads\\SMocean{suiteMaxQArr[i]}-{dontSuiteMaxQArr[i]}.jpg", ImageFormat.Jpeg);
            }
        }
    }
}


// Выделение границ и поиск объектов при помощи методов преобразования Хафа. Метод Щарра и поиск квадрата. 

