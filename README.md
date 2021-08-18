# ImageCompression
Library to compress byte array image data

# How to create instance in Application
ICompressImage compressImage = new CompressImageIMPL();
byte[] compressedimage = compressImage.compressImagewithScaleFactor(0.7, imageinByte);

#Test Output
![image](https://user-images.githubusercontent.com/65488806/129859295-f5e70168-e4a2-4c7a-ae9e-16ca8d84c341.png)


#NOTE: Sacle Factor value should be greater than 0.5. This to maintain image quaility
