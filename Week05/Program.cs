using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Week05
{
    public static class Program
    {
        private static void TestMat3()
        {
            Mat4 mat1 = new Mat4();
            Console.Write(mat1.ToString());
        }

        public static void Update(
            Renderer renderer, 
            FrameEventArgs e, 
            KeyboardState input)
        {
            if (input.IsKeyDown(Keys.S))
            {
                Mat4 scale = new Mat4();
                scale.MakeScale(2.0f, 1.0f, 1.0f);
                Console.Write(scale.ToString());
                renderer.SetMatrix(scale);
            }
            if (input.IsKeyDown(Keys.T))
            {
                Mat4 trans = new Mat4();
                trans.MakeTranslate(0.5f, 0.0f, 0.0f);
                Console.Write(trans.ToString());
                renderer.SetMatrix(trans);
            }
        }

        private static void Main()
        {
            TestMat3();
            

            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 800),
                Title = "CGPG - Creating a Window using OpenTK",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);
            renderer.OnUpdate = Update;

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

            Mat4 trans = new Mat4();
            trans.MakeTranslate(0.5f, 0.0f, 0.0f);
            Console.Write(trans.ToString());
            renderer.SetMatrix(trans);

            renderer.Run();
            
        }
    }
}
