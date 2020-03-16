using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HEIF.NET
{
    public static class DecoderWrapper
    {
        [DllImport("HEIF.NET.Wrapper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetNumberOfImagesInFile(string fileName);

        [DllImport("HEIF.NET.Wrapper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetImageIds(string fileName, out IntPtr ids, int count);

        [DllImport("HEIF.NET.Wrapper.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DecodeImage(string fileName, int imageId, out IntPtr size);
    }
}
