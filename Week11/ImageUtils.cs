using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPG
{
    public static class ImageUtils
    {
        public static byte[] NegateImage(byte[] pixelData)
        {
            byte[] negatedPixels = new byte[pixelData.Length];
            for (int i = 0; i < pixelData.Length; i += 4)
            {
                negatedPixels[i] = (byte)(255 - pixelData[i]);       // R
                negatedPixels[i + 1] = (byte)(255 - pixelData[i + 1]); // G
                negatedPixels[i + 2] = (byte)(255 - pixelData[i + 2]); // B
                negatedPixels[i + 3] = pixelData[i + 3];               // A (unchanged)
            }
            return negatedPixels;
        }
        public static byte[] ConvertToGrayscale(byte[] imageData, int width, int height)
        {
            byte[] grayscaleData = new byte[width * height * 4];

            for (int i = 0; i < imageData.Length; i += 4)
            {
                byte r = imageData[i];
                byte g = imageData[i + 1];
                byte b = imageData[i + 2];
                byte a = imageData[i + 3];

                byte gray = (byte)((r + g + b) / 3);

                grayscaleData[i] = gray;
                grayscaleData[i + 1] = gray;
                grayscaleData[i + 2] = gray;
                grayscaleData[i + 3] = a;
            }

            return grayscaleData;
        }
        public static byte[] AdjustBrightness(byte[] imageData, int width, int height, int brightness)
        {
            byte[] brightenedData = new byte[width * height * 4];

            for (int i = 0; i < imageData.Length; i += 4)
            {
                brightenedData[i] = ClampToByte(imageData[i] + brightness);
                brightenedData[i + 1] = ClampToByte(imageData[i + 1] + brightness);
                brightenedData[i + 2] = ClampToByte(imageData[i + 2] + brightness);
                brightenedData[i + 3] = imageData[i + 3]; // Alpha remains unchanged
            }

            return brightenedData;
        }

        private static byte ClampToByte(int value)
        {
            return (byte)Math.Max(0, Math.Min(255, value));
        }

        private static byte ClampToByte(float value)
        {
            return (byte)Math.Max(0, Math.Min(255, value));
        }
        public static byte[] ApplySobelEdgeDetection(byte[] imageData, int width, int height)
        {
            byte[] edgeData = new byte[width * height * 4];

            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int pixelIndex = (y * width + x) * 4;

                    int sumX = 0;
                    int sumY = 0;

                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int kernelIndex = ((y + ky) * width + (x + kx)) * 4;

                            byte r = imageData[kernelIndex];
                            byte g = imageData[kernelIndex + 1];
                            byte b = imageData[kernelIndex + 2];

                            int gray = (r + g + b) / 3;

                            sumX += gray * gx[ky + 1, kx + 1];
                            sumY += gray * gy[ky + 1, kx + 1];
                        }
                    }

                    int magnitude = (int)Math.Sqrt((sumX * sumX) + (sumY * sumY));
                    byte edgeValue = ClampToByte(magnitude);

                    edgeData[pixelIndex] = edgeValue;
                    edgeData[pixelIndex + 1] = edgeValue;
                    edgeData[pixelIndex + 2] = edgeValue;
                    edgeData[pixelIndex + 3] = imageData[pixelIndex + 3]; // Alpha remains unchanged
                }
            }

            return edgeData;
        }
        public static byte[] RotateImage90Degrees(byte[] imageData, int width, int height)
        {
            byte[] rotatedData = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int srcIndex = (y * width + x) * 4;
                    int destIndex = ((x * height) + (height - y - 1)) * 4;

                    rotatedData[destIndex] = imageData[srcIndex];
                    rotatedData[destIndex + 1] = imageData[srcIndex + 1];
                    rotatedData[destIndex + 2] = imageData[srcIndex + 2];
                    rotatedData[destIndex + 3] = imageData[srcIndex + 3];
                }
            }

            return rotatedData;
        }
        public static byte[] ApplyBoxBlur(byte[] imageData, int width, int height)
        {
            byte[] blurredData = new byte[width * height * 4];
            int kernelSize = 3;
            int kernelHalf = kernelSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = (y * width + x) * 4;
                    int[] rgba = new int[4];

                    for (int ky = -kernelHalf; ky <= kernelHalf; ky++)
                    {
                        for (int kx = -kernelHalf; kx <= kernelHalf; kx++)
                        {
                            int nx = Math.Min(Math.Max(x + kx, 0), width - 1);
                            int ny = Math.Min(Math.Max(y + ky, 0), height - 1);
                            int neighborIndex = (ny * width + nx) * 4;

                            rgba[0] += imageData[neighborIndex];
                            rgba[1] += imageData[neighborIndex + 1];
                            rgba[2] += imageData[neighborIndex + 2];
                            rgba[3] += imageData[neighborIndex + 3];
                        }
                    }

                    int numPixels = kernelSize * kernelSize;
                    blurredData[pixelIndex] = (byte)(rgba[0] / numPixels);
                    blurredData[pixelIndex + 1] = (byte)(rgba[1] / numPixels);
                    blurredData[pixelIndex + 2] = (byte)(rgba[2] / numPixels);
                    blurredData[pixelIndex + 3] = (byte)(rgba[3] / numPixels);
                }
            }

            return blurredData;
        }
        public static byte[] FlipImageHorizontally(byte[] imageData, int width, int height)
        {
            byte[] flippedData = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int srcIndex = (y * width + x) * 4;
                    int destIndex = (y * width + (width - x - 1)) * 4;

                    flippedData[destIndex] = imageData[srcIndex];
                    flippedData[destIndex + 1] = imageData[srcIndex + 1];
                    flippedData[destIndex + 2] = imageData[srcIndex + 2];
                    flippedData[destIndex + 3] = imageData[srcIndex + 3];
                }
            }

            return flippedData;
        }

        public static byte[] FlipImageVertically(byte[] imageData, int width, int height)
        {
            byte[] flippedData = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int srcIndex = (y * width + x) * 4;
                    int destIndex = ((height - y - 1) * width + x) * 4;

                    flippedData[destIndex] = imageData[srcIndex];
                    flippedData[destIndex + 1] = imageData[srcIndex + 1];
                    flippedData[destIndex + 2] = imageData[srcIndex + 2];
                    flippedData[destIndex + 3] = imageData[srcIndex + 3];
                }
            }

            return flippedData;
        }
        public static byte[] AdjustContrast(byte[] imageData, int width, int height, float contrast)
        {
            byte[] contrastData = new byte[width * height * 4];
            float factor = (259 * (contrast + 255)) / (255 * (259 - contrast));

            for (int i = 0; i < imageData.Length; i += 4)
            {
                contrastData[i] = ClampToByte(factor * (imageData[i] - 128) + 128);
                contrastData[i + 1] = ClampToByte(factor * (imageData[i + 1] - 128) + 128);
                contrastData[i + 2] = ClampToByte(factor * (imageData[i + 2] - 128) + 128);
                contrastData[i + 3] = imageData[i + 3]; // Alpha remains unchanged
            }

            return contrastData;
        }

        public static byte[] ApplyCheckerboardPattern(byte[] imageData, int width, int height, int gridSize)
        {
            byte[] checkerboardData = new byte[width * height * 4];

            // Calculate the size of each small cell in the original image
            int cellWidth = width / gridSize;
            int cellHeight = height / gridSize;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Determine the cell coordinates within the grid
                    int cellX = x / cellWidth;
                    int cellY = y / cellHeight;

                    // Determine if the pixel should be black or retain the original image
                    bool isOriginalPixel = ((cellX + cellY) % 2 == 0);

                    int srcIndex = (y * width + x) * 4;
                    int destIndex = srcIndex;

                    if (isOriginalPixel)
                    {
                        // Copy original pixel data (R, G, B, A)
                        checkerboardData[destIndex] = imageData[srcIndex];         // R
                        checkerboardData[destIndex + 1] = imageData[srcIndex + 1]; // G
                        checkerboardData[destIndex + 2] = imageData[srcIndex + 2]; // B
                        checkerboardData[destIndex + 3] = imageData[srcIndex + 3]; // A
                    }
                    else
                    {
                        // Fill with black
                        checkerboardData[destIndex] = 0;     // R
                        checkerboardData[destIndex + 1] = 0; // G
                        checkerboardData[destIndex + 2] = 0; // B
                        checkerboardData[destIndex + 3] = 255; // A (opaque)
                    }
                }
            }

            return checkerboardData;
        }
        public static byte[] ApplyCircleCheckerboardPattern(byte[] imageData, int width, int height, int circleRadius, int circleSpacing)
        {
            byte[] checkerboardData = new byte[width * height * 4];

            // Calculate the center positions of the circles
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isInCircle = IsInCircle(x, y, circleRadius, circleSpacing);

                    int srcIndex = (y * width + x) * 4;
                    int destIndex = srcIndex;

                    if (isInCircle)
                    {
                        // Copy original pixel data (R, G, B, A)
                        checkerboardData[destIndex] = imageData[srcIndex];         // R
                        checkerboardData[destIndex + 1] = imageData[srcIndex + 1]; // G
                        checkerboardData[destIndex + 2] = imageData[srcIndex + 2]; // B
                        checkerboardData[destIndex + 3] = imageData[srcIndex + 3]; // A
                    }
                    else
                    {
                        // Fill with black
                        checkerboardData[destIndex] = 0;     // R
                        checkerboardData[destIndex + 1] = 0; // G
                        checkerboardData[destIndex + 2] = 0; // B
                        checkerboardData[destIndex + 3] = 255; // A (opaque)
                    }
                }
            }

            return checkerboardData;
        }

        private static bool IsInCircle(int x, int y, int circleRadius, int circleSpacing)
        {
            // Calculate the center positions of the circles and check if the pixel is within any circle
            int circleX = (x / (circleRadius * 2 + circleSpacing)) * (circleRadius * 2 + circleSpacing) + circleRadius + circleSpacing / 2;
            int circleY = (y / (circleRadius * 2 + circleSpacing)) * (circleRadius * 2 + circleSpacing) + circleRadius + circleSpacing / 2;

            int deltaX = x - circleX;
            int deltaY = y - circleY;

            return (deltaX * deltaX + deltaY * deltaY <= circleRadius * circleRadius);
        }
    }
}
