using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace CGPG
{
    // Be warned, there is a LOT of stuff here. It might seem complicated, but just take it slow and you'll be fine.
    // OpenGL's initial hurdle is quite large, but once you get past that, things will start making more sense.
    public class Renderer : GameWindow
    {
        List<Geometry> geometries = new List<Geometry>();
        public Renderer(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public void AddGeometry(Geometry geometry)
        {
            geometries.Add(geometry);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //for (int i = 0; i < geometries.Count; i++)
            //{
            //    geometries[i].Compile();
            //}
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            for (int i = 0;i < geometries.Count;i++)
            {
                geometries[i].Draw();
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
        }
    }
}
