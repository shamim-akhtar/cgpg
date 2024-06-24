using CGPG;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Week11
{
    public static class Program
    {
        public static int IMAGE_X = 512;
        public static int IMAGE_Y = 512;
        
        public static Texture texture;
        public static Geometry quad;
        public static Geometry quad1;

        private static int brightness = 0;
        private static float contrast = 0;

        private static int gridSize = 1;
        private static int circleRadiusFactor = 2;

        public static byte[] GeneratePixelData_Blue(int width, int height)
        {
            byte[] pixels = new byte[width * height * 4];
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 0;     // R
                pixels[i + 1] = 0;   // G
                pixels[i + 2] = 255;   // B
                pixels[i + 3] = 255; // A
            }
            return pixels;
        }
        public static byte[] GeneratePixelData_Red(int width, int height)
        {
            byte[] pixels = new byte[width * height * 4];
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 255;     // R
                pixels[i + 1] = 0;   // G
                pixels[i + 2] = 0;   // B
                pixels[i + 3] = 255; // A
            }
            return pixels;
        }
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1200, 600),
                Title = "CGPG - Week 11",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);

            //--------------------------------------------------------//
            // The manipulated image.
            //--------------------------------------------------------//
            quad = Shapes.CreateQuad();
            quad.SetTexture("Resources/cartoon-house_512.jpg", TextureUnit.Texture0);

            renderer.AddGeometry(quad);

            Mat4 trans = new Mat4();
            trans.MakeTranslate(0.75f, 0, 0);
            quad.SetMatrix(trans);
            //--------------------------------------------------------//

            //--------------------------------------------------------//
            // The original image.
            //--------------------------------------------------------//
            quad1 = Shapes.CreateQuad();
            quad1.SetTexture("Resources/cartoon-house_512.jpg", TextureUnit.Texture0);

            renderer.AddGeometry(quad1);

            Mat4 trans1 = new Mat4();
            trans1.MakeTranslate(-0.75f, 0, 0);
            quad1.SetMatrix(trans1);
            //--------------------------------------------------------//

            renderer.OnUpdate = OnUpdateFrame;

            renderer.Run();            
        }

        static void OnUpdateFrame(Renderer ren, FrameEventArgs e, KeyboardState keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.U))
            {
                byte[] data = GeneratePixelData_Red(IMAGE_X, IMAGE_Y);
                quad.UpdateTextureData(data, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.N))
            {
                // Retrieve the current texture data
                byte[] originalData = quad.GetTexture().GetRawData();
                byte[] negatedData = ImageUtils.NegateImage(originalData);
                quad.UpdateTextureData(negatedData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.G))
            {
                // Retrieve the current texture data
                byte[] originalData = quad.GetTexture().GetRawData();
                byte[] gray = ImageUtils.ConvertToGrayscale(originalData, IMAGE_X, IMAGE_Y);
                quad.UpdateTextureData(gray, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                brightness += 1;
                if (brightness >= 255) brightness = 255;
                // Retrieve the current texture data
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.AdjustBrightness(originalData, IMAGE_X, IMAGE_Y, brightness);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                brightness -= 1;
                if (brightness <= 0) brightness = 0;
                // Retrieve the current texture data
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.AdjustBrightness(originalData, IMAGE_X, IMAGE_Y, brightness);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                contrast += 1.0f;
                if (contrast >= 255) contrast = 255;
                // Retrieve the current texture data
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.AdjustContrast(originalData, IMAGE_X, IMAGE_Y, contrast);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                contrast -= 1;
                if (contrast <= -255) contrast = -255;
                // Retrieve the current texture data
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.AdjustContrast(originalData, IMAGE_X, IMAGE_Y, contrast);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.E))
            {
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.ApplySobelEdgeDetection(
                    originalData, IMAGE_X, IMAGE_Y);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.R))
            {
                byte[] originalData = quad.GetTexture().GetRawData();
                byte[] newData = ImageUtils.RotateImage90Degrees(
                    originalData, IMAGE_X, IMAGE_Y);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.B))
            {
                byte[] originalData = quad.GetTexture().GetRawData();
                byte[] newData = ImageUtils.ApplyBoxBlur(
                    originalData, IMAGE_X, IMAGE_Y);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
            }
            else if (keyboard.IsKeyPressed(Keys.C))
            {
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.ApplyCheckerboardPattern(
                    originalData, IMAGE_X, IMAGE_Y, gridSize);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
                gridSize += 2;
                if (gridSize >= 64) gridSize = 64;
            }
            else if (keyboard.IsKeyPressed(Keys.S))
            {
                byte[] originalData = quad1.GetTexture().GetRawData();
                byte[] newData = ImageUtils.ApplyCircleCheckerboardPattern(
                    originalData, IMAGE_X, IMAGE_Y, IMAGE_X/ circleRadiusFactor, 5);
                quad.UpdateTextureData(newData, IMAGE_X, IMAGE_Y);
                circleRadiusFactor += 2;
                if(circleRadiusFactor >= IMAGE_X) circleRadiusFactor = IMAGE_X;
            }
            else if (keyboard.IsKeyPressed(Keys.Space))
            {
                // Reset
                byte[] originalData = quad1.GetTexture().GetRawData();
                quad.UpdateTextureData(originalData, IMAGE_X, IMAGE_Y);
                circleRadiusFactor = 2;
                gridSize = 1;
                contrast = 0;
                brightness = 0;
            }

        }
    }
}
