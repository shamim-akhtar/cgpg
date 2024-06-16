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
        public static int IMAGE_X = 256;
        public static int IMAGE_Y = 256;
        public static Texture texture;
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
                ClientSize = new Vector2i(1200, 800),
                Title = "CGPG - Week 11",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);

            // Create the raw image data.
            byte[] data = GeneratePixelData_Blue(IMAGE_X, IMAGE_Y);
            //

            Geometry quad = Shapes.CreateQuad();
            texture = Texture.CreatTextureFromRawData(IMAGE_X, IMAGE_Y, data);
            quad.SetTexture(texture);

            renderer.AddGeometry(quad);

            renderer.OnUpdate = OnUpdateFrame;

            renderer.Run();            
        }

        static void OnUpdateFrame(Renderer ren, FrameEventArgs e, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.U))
            {
                byte[] data = GeneratePixelData_Red(IMAGE_X, IMAGE_Y);
                Renderer.UpdateTextureData(texture, data, IMAGE_X, IMAGE_Y);
            }
        }
    }
}
