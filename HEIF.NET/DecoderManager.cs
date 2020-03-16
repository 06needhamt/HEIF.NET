using System;
using System.Collections.Generic;
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
                IntPtr idPtr;
                int[] ids = new int[count];
                int idCount = DecoderWrapper.GetImageIds(FileName, out idPtr, count);
                Marshal.Copy(idPtr, ids, 0, idCount);

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
                IntPtr size = new IntPtr();
                IntPtr data = DecoderWrapper.DecodeImage(FileName, imageId, out size);
                byte[] imageBytes = new byte[size.ToInt32()];
                Marshal.Copy(data, imageBytes, 0, size.ToInt32());

                return imageBytes;
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
