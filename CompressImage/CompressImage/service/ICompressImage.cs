using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressImage.service
{
    public interface ICompressImage
    {
        //Method to compress image with ScaleFactor value.
        byte[] compressImagewithScaleFactor(double scaleFactor, byte[] imagebyte);

    }
}
