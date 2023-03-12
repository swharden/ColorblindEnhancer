﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ColorblindEnhancer;

internal static class Process
{
    private static byte[,,] ImageToArray(Bitmap bmp)
    {
        int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
        Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
        int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
        byte[] bytes = new byte[byteCount];
        Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
        bmp.UnlockBits(bmpData);

        byte[,,] pixelValues = new byte[bmp.Height, bmp.Width, 3];
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = (y * bmpData.Stride) + x * bytesPerPixel;
                pixelValues[y, x, 0] = bytes[offset + 2]; // red
                pixelValues[y, x, 1] = bytes[offset + 1]; // green
                pixelValues[y, x, 2] = bytes[offset + 0]; // blue
            }
        }

        return pixelValues;
    }

    private static Bitmap ArrayToImage(byte[,,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;
        int bytesPerPixel = 3;

        byte[] bytes = new byte[stride * height * bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * stride + x) * bytesPerPixel;
                bytes[offset + 0] = pixelArray[y, x, 2]; // blue
                bytes[offset + 1] = pixelArray[y, x, 1]; // green
                bytes[offset + 2] = pixelArray[y, x, 0]; // red
            }
        }

        PixelFormat formatOutput = PixelFormat.Format24bppRgb;
        Rectangle rect = new(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        Bitmap bmp2 = new(width, height, PixelFormat.Format32bppPArgb);
        Graphics gfx2 = Graphics.FromImage(bmp2);
        gfx2.DrawImage(bmp, 0, 0);

        return bmp2;
    }

    public static Bitmap Grayscale(Bitmap bmp)
    {
        byte[,,] bytes = ImageToArray(bmp);
        for (int y = 0; y < bytes.GetLength(0); y++)
        {
            for (int x = 0; x < bytes.GetLength(1); x++)
            {
                byte val = (byte)((bytes[y, x, 0] + bytes[y, x, 1] + bytes[y, x, 2]) / 3);
                bytes[y, x, 0] = val;
                bytes[y, x, 1] = val;
                bytes[y, x, 2] = val;
            }
        }
        return ArrayToImage(bytes);
    }
}
