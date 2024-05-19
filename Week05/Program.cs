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

            var window = new Renderer(GameWindowSettings.Default, nativeWindowSettings);

            // Create the vertices for our triangle.
            // These are listed in normalized device coordinates (NDC)
            // In NDC, (0, 0) is the center of the screen.
            // Negative X coordinates move to the left, positive X move to the right.
            // Negative Y coordinates move to the bottom, positive Y move to the top.
            // OpenGL only supports rendering in 3D, so to create a flat triangle,
            // the Z coordinate will be kept as 0.
            float[] vertices =
            {
                -0.5f, -0.5f, 0.0f, // Bottom-left vertex
                0.5f, -0.5f, 0.0f, // Bottom-right vertex
                0.0f,  0.5f, 0.0f  // Top vertex
            };

            //Set the vertex array to the renderer.
            window.SetVertexArray(vertices);

            window.Run();
            
        }
    }
}
