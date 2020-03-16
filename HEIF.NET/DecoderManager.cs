using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HEIF.NET
{
    public class DecoderManager
    {
        public string FileName { get; private set; }

        public DecoderManager(string fileName)
        {
            this.FileName = fileName;
        }

        public int GetNumberOfImagesInFile(out string error)
        {
            try
            {
                error = string.Empty;
                return DecoderWrapper.GetNumberOfImagesInFile(FileName);
            }
            catch (AggregateException ae)
            {
                error = ae.InnerException.Message;
                return int.MinValue;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return int.MinValue;
            }
        }

        public int[] GetImageIds(int count, out string error)
        {
            try
            {
                error = string.Empty;
                int[] ids = new int[count];
                int idCount = DecoderWrapper.GetImageIds(FileName, ids, count);

                return ids;
            }
            catch (AggregateException ae)
            {
                error = ae.InnerException.Message;
                return null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public byte[] DecodeImage(int imageId, out string error)
        {
            try
            {
                error = string.Empty;

                FileInfo fi = new FileInfo(FileName);
                IntPtr buffer = new IntPtr();
                int size = DecoderWrapper.DecodeImage(FileName, imageId, buffer);
                byte[] data = new byte[size];
                Marshal.Copy(buffer, data, 0, size);

                return data;
            }
            catch (AggregateException ae)
            {
                error = ae.InnerException.Message;
                return null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
    }
}
