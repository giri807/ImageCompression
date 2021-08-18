using CompressImage.service.unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressImage.service
{
    public class CompressImageIMPL : ICompressImage
    {
        public byte[] compressImagewithScaleFactor(double scaleFactor, byte[] imagebyte)
        {
            CompressImagewithScaleFactorUnit unit = new CompressImagewithScaleFactorUnit();
            return unit.execute(scaleFactor, imagebyte);
        }
    }
}
