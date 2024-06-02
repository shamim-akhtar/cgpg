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
    public class HitRecord
    {
        public float T { get; set; }
        public Vec3 HitPoint { get; set; }
        public Vec3 Normal { get; set; }
    }

    public interface IIntersectable
    {
        bool Intersect(Ray ray, float minT, float maxT, out HitRecord hitRecord);
    }

    public class Sphere : IIntersectable
    {
        public Vec3 Center { get; private set; }
        public float Radius { get; private set; }

        public Sphere(Vec3 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public bool Intersect(Ray ray, float minT, float maxT, out HitRecord hitRecord)
        {
            Vec3 oc = ray.Origin - Center;
            float a = Vec3.Dot(ray.Direction, ray.Direction);
            float b = Vec3.Dot(oc, ray.Direction);
            float c = Vec3.Dot(oc, oc) - Radius * Radius;
            float discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                float temp = (-b - MathF.Sqrt(discriminant)) / a;
                if (temp < maxT && temp > minT)
                {
                    hitRecord = new HitRecord();
                    hitRecord.T = temp;
                    hitRecord.HitPoint = ray.PointAt(hitRecord.T);
                    hitRecord.Normal = (hitRecord.HitPoint - Center);// / Radius;
                    hitRecord.Normal.Normalize();
                    return true;
                }
                temp = (-b + MathF.Sqrt(discriminant)) / a;
                if (temp < maxT && temp > minT)
                {
                    hitRecord = new HitRecord();
                    hitRecord.T = temp;
                    hitRecord.HitPoint = ray.PointAt(hitRecord.T);
                    hitRecord.Normal = (hitRecord.HitPoint - Center);// / Radius;
                    hitRecord.Normal.Normalize();
                    return true;
                }
            }

            hitRecord = null;
            return false;
        }
    }

    public class Scene : IIntersectable
    {
        List<IIntersectable> objects = new List<IIntersectable>();
        public Scene()
        { }

        public void Add(IIntersectable obj)
        {
            objects.Add(obj);
        }
        public bool Intersect(Ray ray, float minT, float maxT, out HitRecord hitRecord)
        {
            HitRecord hit;
            bool anyHit = false;
            float closest = maxT;
            hitRecord = null;

            for(int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Intersect(ray, minT, closest, out hit))
                {
                    anyHit = true;
                    closest = hit.T;
                    hitRecord = hit;
                }
            }
            return anyHit;
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
        Scene scene = new Scene();
        Sphere sphere1 = new Sphere(new Vec3(0, 0, 1.5f), 0.5f);
        scene.Add(sphere1);
        Sphere sphere2 = new Sphere(new Vec3(0, -101.5f, 1), 100.0f);
        scene.Add(sphere2);

        int tx = 800;
        int ty = 600;
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(tx, ty),
            Title = "CGPG - Ray Tracer",
            Flags = ContextFlags.ForwardCompatible,
        };


        var renderer = new Renderer(nativeWindowSettings, tx, ty);

        Vec3 cameraOrigin = new Vec3(0, 0, 0);

        float nearPlaneZ = 1.0f;

        // Define the aspect ratio
        float aspectRatio = (float)tx / ty;

        // Calculate the near plane width and height based on the aspect ratio
        float nearPlaneWidth = 2.0f;
        float nearPlaneHeight = nearPlaneWidth / aspectRatio;

        // Adjust near plane height symmetrically around the center
        float halfNearPlaneHeight = nearPlaneHeight / 2.0f;

        // Calculate dx and dy based on the adjusted near plane dimensions
        float dx = nearPlaneWidth / tx;
        float dy = nearPlaneHeight / ty;

        for (int j = 0; j < ty; j++)
        {
            for (int i = 0; i < tx; i++)
            {
                float px = -1 + (i + 0.5f) * dx;
                float py = -halfNearPlaneHeight + (j + 0.5f) * dy; // Adjust py

                float pz = nearPlaneZ;

                Vec3 pixelPoint = new Vec3(px, py, pz);
                Vec3 direction = pixelPoint - cameraOrigin;

                Ray ray = new Ray(cameraOrigin, direction);

                int index = (j * tx + i) * 4; // Calculate the correct index in the byte array

                // Check for intersection with the sphere
                float t;
                Vec3 normal;
                HitRecord hit;
                if (scene.Intersect(ray, 0.0f, 9999.0f, out hit))
                {
                    normal = hit.Normal;
                    // Calculate color based on normal
                    float r = (normal.x + 1) / 2;
                    float g = (normal.y + 1) / 2;
                    float b = (normal.z + 1) / 2;

                    // Set color to the pixel
                    renderer.Pix[index] = (byte)(r * 255);
                    renderer.Pix[index + 1] = (byte)(g * 255);
                    renderer.Pix[index + 2] = (byte)(b * 255);
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

