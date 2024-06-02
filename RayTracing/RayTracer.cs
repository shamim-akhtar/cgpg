using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

public static class RayTracer
{
    private static void Main()
    {

        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "CGPG - Ray Tracer",
            Flags = ContextFlags.ForwardCompatible,
        };

        var renderer = new Renderer(GameWindowSettings.Default, nativeWindowSettings);

        float[] vertices =
        {
            1f,  1f, 0.0f, 1.0f, 1.0f, // top right
            1f, -1f, 0.0f, 1.0f, 0.0f, // bottom right
            -1f, -1f, 0.0f, 0.0f, 0.0f, // bottom left
            -1f,  1f, 0.0f, 0.0f, 1.0f  // top left
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

