using CompressImage.commonutils;
using CompressImage.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressImage.service.unit
{
    /// <summary>
    /// CompressImagewithScaleFactorUnit will provide the compressed image based in the scale factor value
    /// </summary>
    public class CompressImagewithScaleFactorUnit
    {
        private static string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"servicelog{DateTime.Now.ToString("yyyyMMDD")}.log");
        StringBuilder _logger = new StringBuilder();
        /// <summary>
        /// Steps are follows
        /// 1. Check the Image Type.
        ///    a. if Image type is other than jpeg then convert the byte array to jpeg.
        ///    b. else keep the same image
        /// 2. Resize the image byte array to the given scale factor value.
        /// 3. Get the jpeg Encoder.
        /// 4. Get image from byte array stream.
        /// 5. Compress the image.
        /// 6. Finally convert the compressed image into byte array.
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] execute(double scaleFactor, byte[] data)
        {
            byte[] outputbyte = null;
            try
            {
                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: execution started");
                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-1: Check the Image Format.");
                ImageType imageFormat = Imageutil.GetImageType(data);
                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: Image Format is {imageFormat} ");
                if (!imageFormat.ToString().ToUpper().Equals("JPEG"))
                {
                    _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-2: If Image is not JPEG CONVERT THE IMAGE TO JPG.");
                }
                else
                {
                    _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-2: is Skipped.");
                }
                byte[] jpegImage = !imageFormat.ToString().ToUpper().Equals("JPEG") ? AsJpeg(data) : data;
                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-3: Resize the Image to the input scale factor");
                byte[] resizeImage = Resize(scaleFactor, jpegImage);


                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-4: Get JPEG Encoder");
                var jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                using (var inStream = new MemoryStream(resizeImage))
                using (var outStream = new MemoryStream())
                {
                    var image = Image.FromStream(inStream);

                    // if we aren't able to retrieve our encoder
                    // we should just save the current image and
                    // return to prevent any exceptions from happening
                    _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-5: Check Jpeg Encoder is there.");
                    if (jpgEncoder == null)
                    {
                        image.Save(outStream, ImageFormat.Jpeg);
                    }
                    else
                    {
                        _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: STEP-5: Compress the image");
                        var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, 50L);
                        image.Save(outStream, jpgEncoder, encoderParameters);
                    }

                    outputbyte =  outStream.ToArray();
                }

            }
            catch (Exception exception)
            {
                _logger.AppendLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}: ERROR : {exception.ToString()}");
            }
            finally
            {
                if (_logger.ToString().Length > 0)
                {

                    File.WriteAllText(strPath, _logger.ToString());
                }
            }

            return outputbyte;

        }


        /// <summary>
        /// Resize the image based on the scale factor
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] Resize(double scaleFactor, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var image = Image.FromStream(stream);

                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);

                var thumbnail = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

                using (var thumbnailStream = new MemoryStream())
                {
                    thumbnail.Save(thumbnailStream, ImageFormat.Jpeg);
                    return thumbnailStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Get the Jepg encoder
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert the any image to Jpeg
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] AsJpeg(byte[] data)
        {
            using (var inStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            {
                var imageStream = Image.FromStream(inStream);
                imageStream.Save(outStream, ImageFormat.Jpeg);
                return outStream.ToArray();
            }
        }

    }
}
