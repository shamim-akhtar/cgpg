using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace CGPG
{
    // Be warned, there is a LOT of stuff here. It might seem complicated,
    // but just take it slow and you'll be fine.
    // OpenGL's initial hurdle is quite large,
    // but once you get past that, things will start making more sense.
    public class Renderer : GameWindow
    {
        List<Geometry> geometries = new List<Geometry>();
        Camera _camera;
        public Renderer(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _camera = new Camera(Vector3.UnitZ, Size.X / (float)Size.Y);
            _camera.Fov = 45.0f;
        }

        public void AddGeometry(Geometry geometry)
        {
            geometries.Add(geometry);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            for (int i = 0; i < geometries.Count; i++)
            {
                geometries[i].Draw(_camera);
            }
            SwapBuffers();
        }

        public delegate void DelegateOnUpdate(Renderer ren, FrameEventArgs e, KeyboardState keyboard);
        public DelegateOnUpdate? OnUpdate = null;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            OnUpdate?.Invoke(this, e, input);

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        // For image manipulations in a texture.
        public static void UpdateTextureData(Texture texture, byte[] pixelData, int width, int height)
        {
            texture.Use(TextureUnit.Texture0); // Bind the texture
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
        }
        //public static byte[] GeneratePixelData(int width, int height)
        //{
        //    byte[] pixels = new byte[width * height * 4];
        //    for (int i = 0; i < pixels.Length; i += 4)
        //    {
        //        pixels[i] = 255;     // R
        //        pixels[i + 1] = 0;   // G
        //        pixels[i + 2] = 255;   // B
        //        pixels[i + 3] = 255; // A
        //    }
        //    return pixels;
        //}
    }
}
