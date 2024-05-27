using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Week06
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 450),
                Title = "CGPG - Week 6 - Creating a Window using OpenTK",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);
             
            Geometry cube = Shapes.CreateCube();
            cube.SetShader("Shaders/Shader_P_MVP.vert", "Shaders/Shader_Empty.frag");

            renderer.AddGeometry(cube);

            renderer.Run();            
        }
    }
}
