using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Week05
{
    public static class Program
    {
        private static void TestMat3()
        {
            Mat4 mat1 = new Mat4();
            Console.Write(mat1.ToString());
        }
        private static void Main()
        {
            TestMat3();
            return;

            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 800),
                Title = "CGPG - Creating a Window using OpenTK",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);
            float[] vertices =
            {
                // Position         Texture coordinates
                0.2f, 0.2f, 0.0f, 1.0f, 1.0f, // top right
                0.2f, -0.2f, 0.0f, 1.0f, 0.0f, // bottom right
                -0.2f, -0.2f, 0.0f, 0.0f, 0.0f, // bottom left
                -0.2f, 0.2f, 0.0f, 0.0f, 1.0f  // top left
            };

            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            //Set the vertex array to the renderer.
            renderer.SetVertexArray(vertices);
            renderer.SetIndexArray(indices);

            renderer.Run();
            
        }
    }
}
