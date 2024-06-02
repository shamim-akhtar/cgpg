using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RayTracing;
using System;
using System.Diagnostics;
using static RayTracer;

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
    public class Sphere
    {
        public Vec3 Center { get; private set; }
        public float Radius { get; private set; }

        public Sphere(Vec3 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public bool Intersect(Ray ray, out float t)
        {
            Vec3 oc = ray.Origin - Center;
            float a = Vec3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vec3.Dot(oc, ray.Direction);
            float c = Vec3.Dot(oc, oc) - Radius * Radius;
            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                t = float.MaxValue;
                return false;
            }
            t = (-b - MathF.Sqrt(discriminant)) / (2.0f * a);
            return true;
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

    public static (float r, float g, float b) ComputeBlueShadeInY(Ray ray)
    {
        float yInterpolation = (ray.Direction.y + 1) / 2;
        float blueValue = 1.0f - 0.25f * yInterpolation; // Start with 1.0 at bottom and reduce to 0.75 at top

        // Return the interpolated color
        return (0.0f, 0.0f, Math.Max(blueValue, 0)); // Ensure blueValue is non-negative
    }

    private static void Main()
    {

        Sphere sphere = new Sphere(new Vec3(0, 0, 2), 0.5f);

        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(800, 800),
            Title = "CGPG - Ray Tracer",
            Flags = ContextFlags.ForwardCompatible,
        };

        int tx = 800;
        int ty = 800;
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
                //var (r, g, b) = ComputeBlueShadeInY(ray);

                int index = (j * tx + i) * 4; // Calculate the correct index in the byte array

                // Check for intersection with the sphere
                float t;
                if (sphere.Intersect(ray, out t))
                {
                    // Intersection occurred, set color to red
                    renderer.Pix[index] = 255;   // Red component
                    renderer.Pix[index + 1] = 0; // Green component
                    renderer.Pix[index + 2] = 0; // Blue component
                    renderer.Pix[index + 3] = 255; // Alpha component
                }
                else
                {
                    // No intersection, continue with your existing logic
                    var (r, g, b) = ComputeBlueShadeInY(ray);
                    renderer.Pix[index] = (byte)(r * 255);
                    renderer.Pix[index + 1] = (byte)(g * 255);
                    renderer.Pix[index + 2] = (byte)(b * 255);
                    renderer.Pix[index + 3] = 255;
                }
            }
        }

        renderer.Run();
    }
}

