using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public static class RayTracer
{
    public static class Utils
    {
        private static readonly Random random = new Random();

        public static Vec3 RandomInUnitSphere()
        {
            Vec3 p;
            do
            {
                p = 2.0f * new Vec3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) - new Vec3(1, 1, 1);
            } while (p.LengthSquared() >= 1.0f);
            return p;
        }
    }

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
    public abstract class Material
    {
        public abstract bool Scatter(Ray ray, HitRecord hitRecord, out Vec3 attenuation, out Ray scattered);
    }
    public class Lambertian : Material
    {
        public Vec3 Albedo { get; }

        public Lambertian(Vec3 albedo)
        {
            Albedo = albedo;
        }

        public override bool Scatter(Ray ray, HitRecord hitRecord, out Vec3 attenuation, out Ray scattered)
        {
            // Generate a random point in a unit sphere
            Vec3 target = hitRecord.HitPoint + hitRecord.Normal + Utils.RandomInUnitSphere();

            // Create a scattered ray pointing in the direction of the target
            scattered = new Ray(hitRecord.HitPoint, target - hitRecord.HitPoint);

            // Set attenuation to the albedo of the material
            attenuation = Albedo;

            return true;
        }
    }
    public class Metal : Material
    {
        public Vec3 Albedo { get; }
        public float Fuzziness { get; }

        public Metal(Vec3 albedo, float fuzziness)
        {
            Albedo = albedo;
            Fuzziness = fuzziness < 1 ? fuzziness : 1; // Ensure fuzziness is between 0 and 1
        }

        public override bool Scatter(Ray ray, HitRecord hitRecord, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 reflected = Reflect(ray.Direction.Normalize(), hitRecord.Normal);
            scattered = new Ray(hitRecord.HitPoint, reflected + Fuzziness * Utils.RandomInUnitSphere());
            attenuation = Albedo;
            return Vec3.Dot(scattered.Direction, hitRecord.Normal) > 0;
        }

        private Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * Vec3.Dot(v, n) * n;
        }
    }


    public class Dielectric : Material
    {
        public float RefractiveIndex { get; }

        public Dielectric(float refractiveIndex)
        {
            RefractiveIndex = refractiveIndex;
        }

        public override bool Scatter(Ray ray, HitRecord hitRecord, out Vec3 attenuation, out Ray scattered)
        {
            // Implement scattering logic for Dielectric material
            // For example, you can calculate the refraction and reflection
            // based on Snell's law
            scattered = new Ray(new Vec3(0.0f, 0.0f, 0.0f), new Vec3(0.0f, 0.0f, 0.0f));
            attenuation = new Vec3(0.0f, 0.0f, 0.0f); // Placeholder, replace with actual logic
            return false; // Placeholder, replace with actual logic
        }
    }


    public class HitRecord
    {
        public float T { get; set; }
        public Vec3? HitPoint { get; set; }
        public Vec3? Normal { get; set; }
        public Material? Material { get; set; }
    }

    public abstract class Intersectable
    {
        public Material? Material { get; set; } = null;
        public abstract bool Intersect(Ray ray, float minT, float maxT, out HitRecord? hitRecord);
    }

    public class Sphere : Intersectable
    {
        public Vec3 Center { get; private set; }
        public float Radius { get; private set; }

        public Sphere(Vec3 center, float radius, Material mat)
        {
            this.Center = center;
            this.Radius = radius;
            this.Material = mat;
        }

        public override bool Intersect(Ray ray, float minT, float maxT, out HitRecord? hitRecord)
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
                    hitRecord.Material = Material;
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
                    hitRecord.Material = Material;
                    return true;
                }
            }

            hitRecord = null;
            return false;
        }
    }

    public class Scene : Intersectable
    {
        List<Intersectable> objects = new List<Intersectable>();
        public Scene()
        { }

        public void Add(Intersectable obj)
        {
            objects.Add(obj);
        }
        public override bool Intersect(Ray ray, float minT, float maxT, out HitRecord? hitRecord)
        {
            HitRecord? hit;
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
        Sphere sphere1 = new Sphere(new Vec3(0, 0, 1.5f), 
            0.5f, 
            new Metal(new Vec3(1.0f, 1.0f, 1.0f), 0.5f));
        scene.Add(sphere1);
        Sphere sphere2 = new Sphere(
            new Vec3(0, -101.5f, 1), 
            100.0f, 
            new Lambertian(new Vec3(1.0f, 1.0f, 1.0f)));
        scene.Add(sphere2);

        int sx = 800;
        int sy = 600;
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(sx, sy),
            Title = "CGPG - Ray Tracer",
            Flags = ContextFlags.ForwardCompatible,
        };

        int tx = sx;
        int ty = sy;

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

        int numSamples = 100;

        for (int j = 0; j < ty; j++)
        {
            for (int i = 0; i < tx; i++)
            {
                Vec3 colorAccumulator = new Vec3(0, 0, 0); // Reset accumulator for each pixel

                for (int s = 0; s < numSamples; s++)
                {
                    // Generate random offsets within dx and dy region
                    float randomOffsetX = (float)(new Random().NextDouble() * dx - dx / 2.0f);
                    float randomOffsetY = (float)(new Random().NextDouble() * dy - dy / 2.0f);

                    float px = -1 + (i + 0.5f) * dx + randomOffsetX;
                    float py = -halfNearPlaneHeight + (j + 0.5f) * dy + randomOffsetY;

                    Vec3 pixelPoint = new Vec3(px, py, nearPlaneZ);
                    Vec3 direction = pixelPoint - cameraOrigin;

                    Ray ray = new Ray(cameraOrigin, direction);
                    colorAccumulator += Colorise(ray, scene, 0);
                }

                int index = (j * tx + i) * 4; // Calculate the correct index in the byte array

                // Average the color by dividing the accumulated color by the number of samples
                Vec3 finalColor = colorAccumulator / (float)numSamples;

                // Set color to the pixel
                renderer.Pix[index] = (byte)(Math.Sqrt(finalColor.x) * 255);
                renderer.Pix[index + 1] = (byte)(Math.Sqrt(finalColor.y) * 255);
                renderer.Pix[index + 2] = (byte)(Math.Sqrt(finalColor.z) * 255);
                renderer.Pix[index + 3] = 255;
            }
        }

        static Vec3 Colorise(Ray ray, Scene scene, int depth)
        {
            Vec3 colorAccumulator = new Vec3(0, 0, 0); // Reset accumulator for each pixel
            // Check for intersection with the scene
            HitRecord? hit;
            if (scene.Intersect(ray, 0.001f, float.MaxValue, out hit))
            {
                Ray scatteredRay;
                Vec3 attenuation;
                if(depth < 50 && 
                    hit != null &&
                    hit.Material.Scatter(ray, hit, out attenuation, out scatteredRay))
                {
                    return attenuation * Colorise(scatteredRay, scene, depth + 1);
                }

                Vec3? normal = hit?.Normal;

                // Calculate color based on normal
                float r = (normal.x + 1) / 2;
                float g = (normal.y + 1) / 2;
                float b = (normal.z + 1) / 2;

                // Accumulate color for this random ray
                colorAccumulator += new Vec3(r, g, b);
            }
            else
            {
                // No intersection, continue with your existing logic
                var (r, g, b) = ComputeBlueShadeInY(ray);
                colorAccumulator += new Vec3(r, g, b);
            }
            return colorAccumulator;
        }

        renderer.Run();
    }

}

