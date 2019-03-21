using HEIF.NET;
using System;
using System.Collections.Generic;
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
            List<string> heifFiles = Directory.EnumerateFiles(Environment.CurrentDirectory).ToList();
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "outputs");
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
                string outputFile = Path.Combine(qualityDirectory, Path.GetFileNameWithoutExtension(inputFile) + ".jpg");
                HEIFConverter converter = new HEIFConverter(inputFile, outputFile, quality);
                if (!converter.ConvertHEIFToJPG())
                    Console.WriteLine(string.Format("Error Converting Image {0} to JPG", Path.GetFileNameWithoutExtension(inputFile)));
                Console.WriteLine(string.Format("Successfully Converted Image {0} to JPG", Path.GetFileNameWithoutExtension(inputFile)));
            }
        }
    }
}
