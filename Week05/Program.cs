using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Week05
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "CGPG - Creating a Window using OpenTK",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);
            float[] vertices =
            {
                -0.5f, -0.5f, 0.0f, // Bottom-left vertex
                0.5f, -0.5f, 0.0f, // Bottom-right vertex
                0.0f,  0.5f, 0.0f  // Top vertex
            };

            //Set the vertex array to the renderer.
            renderer.SetVertexArray(vertices);

            renderer.Run();
            
        }
    }
}
