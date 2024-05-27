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

            // Testing - Translation.
            Mat4 tran = new Mat4();
            tran.MakeTranslate(2.0f, 0.0f, 0.0f);
            //cube.SetMatrix(tran);

            // Testing - Scale
            Mat4 scal = new Mat4();
            scal.MakeScale(1.0f, 2.0f, 1.0f);
            //cube.SetMatrix(scal);

            // Testing - Scale
            Mat4 rotz = new Mat4();
            rotz.MakeRotate(0.0f, 45.0f, 0.0f);
            cube.SetMatrix(rotz);

            renderer.AddGeometry(cube);

            renderer.Run();            
        }
    }
}
