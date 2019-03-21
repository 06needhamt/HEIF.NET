using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEIF.NET
{
    public class HEIFConverter
    {
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }
        public int Quality { get; private set; }

        public HEIFConverter(string inputFile, string outputFile, int quality)
        {
            quality = quality.Clamp(1, 100);
            this.InputFile = inputFile;
            this.OutputFile = outputFile;
            this.Quality = quality;
        }

        public bool ConvertHEIFToJPG()
        {
            try
            {
                string tempFile = "temp_file.hief";
                int copySize = 0;
                byte[] heifImage = NativeBridge.read_heif(InputFile);
                byte[] buffer;

                buffer = NativeBridge.invoke_heif2jpg(heifImage, Quality, tempFile, ref copySize, true, true);
                FileStream fs = new FileStream(OutputFile, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(fs);

                try
                {
                    writer.Write(buffer, 0, copySize);
                    writer.Close();
                    fs.Close();
                    File.Delete(tempFile);
                }
                catch (Exception ex)
                {
                    try
                    {
                        Console.WriteLine(string.Format("Error Occurred While writing HEIF Buffer {0}", ex));
                        writer.Close();
                        fs.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("Error Occurred While Closing HEIF Buffer {0}", e));
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error Occurred While Converting HEIF To JPG {0}", ex));
                return false;
            }

            return true;
        }
    }
}
