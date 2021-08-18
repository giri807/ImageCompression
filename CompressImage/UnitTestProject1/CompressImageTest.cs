using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompressImage;
using CompressImage.service;
using System.Drawing;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class CompressImageTest
    {
        [TestMethod]
        public void TestCompressImage()
        {
            ICompressImage compressImage = new CompressImageIMPL();
            byte[] imageinByte;
            const string imageLocation1 = @"E:\Workspace\ImageCompression\CompressImage\CompressImage\bin\Debug\signature.jpg";
            Image image = Image.FromFile(imageLocation1);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                imageinByte = ms.ToArray();
            }

            byte[] compressedimage = compressImage.compressImagewithScaleFactor(0.7, imageinByte);

            MemoryStream outputms = new MemoryStream(compressedimage);
            Image returnImage = Image.FromStream(outputms);
            returnImage.Save(@"E:\Workspace\ImageCompression\CompressImage\CompressImage\bin\Debug\signature1.jpg");
        }
    }
}
