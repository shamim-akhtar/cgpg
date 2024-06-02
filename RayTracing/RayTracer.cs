using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RayTracing;
using System.Diagnostics;

public static class RayTracer
{
    public class Ray
    {
        public Vec3 Origin { get; private set; }
        public Vec3 Direction { get; private set; }

        public Ray(Vec3 Origin, Vec3 Direction)
        {
            this.Origin = new Vec3(Origin.x, Origin.y, Origin.z);
            this.Direction = new Vec3(Direction.x, Direction.y, Direction.z);
        }

        public Vec3 PointAt(float t)
        {
            return Origin + (Direction * t);
        }
    }

    public static (float r, float g, float b) ComputeColor(Ray ray)
    {
        float red = (ray.Direction.x + 1) / 2;
        float green = (ray.Direction.y + 1) / 2;

        // No blue component in this case
        float blue = 0;

        return (red, green, blue);
    }

    private static void Main()
    {

        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 600),
            Title = "CGPG - Ray Tracer",
            Flags = ContextFlags.ForwardCompatible,
        };

        int tx = 512;
        int ty = 512;
        var renderer = new Renderer(nativeWindowSettings, tx, ty);

        Vec3 cameraOrigin = new Vec3(0, 0, 0);

        float nearPlaneZ = 1.0f;
        float nearPlaneWidth = 2.0f;  // from -1 to 1
        float nearPlaneHeight = 2.0f; // from -1 to 1

        float dx = nearPlaneWidth / tx;
        float dy = nearPlaneHeight / ty;

        for (int j = 0; j < ty; j++)
        {
            for (int i = 0; i < tx; i++)
            {
                float px = -1 + (i + 0.5f) * dx;
                //float py = 1 - (j + 0.5f) * dy;
                float py = -1 + (j + 0.5f) * dy;

                float pz = nearPlaneZ;

                Vec3 pixelPoint = new Vec3(px, py, pz);
                Vec3 direction = pixelPoint - cameraOrigin;

                Ray ray = new Ray(cameraOrigin, direction);
                var (r, g, b) = ComputeColor(ray);

                int index = (j * tx + i) * 4; // Calculate the correct index in the byte array

                renderer.Pix[index] = (byte)(r * 255);
                renderer.Pix[index + 1] = (byte)(g * 255);
                renderer.Pix[index + 2] = (byte)(b * 255);
                renderer.Pix[index + 3] = 255;
            }
        }

        renderer.Run();
    }
}

