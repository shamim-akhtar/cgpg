using CGPG;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Week11
{
    public static class Program
    {
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
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1200, 800),
                Title = "CGPG - Week 11",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);
             
            Geometry quad = Shapes.CreateQuad();
            Texture texture = Texture.CreateEmptyRedTexture(256, 256);
            texture.Use(TextureUnit.Texture0);
            quad.SetTexture(texture);

            byte[] data = GeneratePixelData_Blue(256, 256);
            Renderer.UpdateTextureData(texture, data, 256, 256);

            renderer.AddGeometry(quad);

            renderer.Run();            
        }
    }
}
