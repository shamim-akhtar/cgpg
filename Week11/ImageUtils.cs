using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPG
{
    public static class ImageUtils
    {
        public static byte[] SetRedColor(
            byte[] input, 
            int width, 
            int height)
        {
            for(int i = 0; i < input.Length; i += 4)
            {
                input[i] = 0;
                input[i + 1] = 255;
                input[i + 2] = 0;
            }
            return input;
        }

        public static byte[] SetRedChannel(
            byte[] input,
            int width,
            int height)
        {
            for (int i = 0; i < input.Length; i += 4)
            {
                //input[i] = 0;
                input[i + 1] = 0;
                input[i + 2] = 0;
            }
            return input;
        }

        public static byte[] SwapRedWithGreen(
            byte[] input,
            int width,
            int height)
        {
            for (int i = 0; i < input.Length; i += 4)
            {
                byte r = input[i];
                byte g = input[i + 1];

                input[i] = g;
                input[i + 1] = r;
                //input[i + 2] = 0;
            }
            return input;
        }

        public static byte[] ConvertToGrayScale(
            byte[] input,
            int width,
            int height)
        {
            for (int i = 0; i < input.Length; i += 4)
            {
                byte r = input[i];
                byte g = input[i + 1];
                byte b = input[i + 2];

                byte gray = (byte)((r + g + b) / 3);

                input[i] = gray;
                input[i + 1] = gray;
                input[i + 2] = gray;
            }
            return input;
        }

        public static byte[] Negate(
            byte[] input,
            int width,
            int height)
        {
            for (int i = 0; i < input.Length; i += 4)
            {
                byte r = input[i];
                byte g = input[i + 1];
                byte b = input[i + 2];

                input[i] = (byte)(255 - r);
                input[i + 1] = (byte)(255 - g);
                input[i + 2] = (byte)(255 - b);
            }
            return input;
        }

    }
}
