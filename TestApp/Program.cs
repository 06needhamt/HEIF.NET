using HEIF.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> heifFiles = Directory.EnumerateFiles(Environment.CurrentDirectory).Where(x => Path.GetExtension(x).ToLower() == ".heic" || Path.GetExtension(x).ToLower() == ".heif").ToList();
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "newoutputs");
            string qualityDirectory = string.Empty;

            int quality = 100;

            if(args.Length != 1)
            {
                Console.WriteLine("Usage: [Quality]<0-100>");
                return;
            }

            if(!int.TryParse(args[0], out quality))
            {
                Console.WriteLine("A Decoding Quality Between 1 and 100 must be specified");
                return;
            }

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            qualityDirectory = Path.Combine(outputDirectory, quality.ToString());

            if (Directory.Exists(qualityDirectory))
                Directory.Delete(qualityDirectory, true);

            Directory.CreateDirectory(qualityDirectory);

            foreach(string inputFile in heifFiles)
            {
                if (inputFile.Contains("temp_file"))
                    continue;
                Console.WriteLine(string.Format("Converting Image {0} to JPG", Path.GetFileNameWithoutExtension(inputFile)));
                DecoderManager manager = new DecoderManager(inputFile);
                string error = string.Empty;

                int numImages = manager.GetNumberOfImagesInFile(out error);

                int[] imageIds = manager.GetImageIds(numImages, out error);

                for (int i = 0; i < imageIds.Length; i++)
                {
                    byte[] imageBytes = manager.DecodeImage(imageIds[i], out error);
                    MemoryStream ms = new MemoryStream(imageBytes);
                    Image img = Image.FromStream(ms);
                    string outputFile = Path.Combine(qualityDirectory, Path.GetFileNameWithoutExtension(inputFile) + $"_{i}.jpg");

                    img.Save(outputFile);
                    img.Dispose();
                    ms.Dispose();
                }

                Console.WriteLine(string.Format("Successfully Converted Image {0} to JPG", Path.GetFileNameWithoutExtension(inputFile)));
            }
        }
    }
}
