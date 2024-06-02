using System;
using System.Numerics;

namespace CGPG
{
    public class Vec3
    {
        public float x, y, z;        

        ///** Constructor that sets all components of the vector to zero */
        public Vec3()
        {
            this.x = 0.0f; 
            this.y = 0.0f; 
            this.z = 0.0f;
        }
        public Vec3(float x, float y, float z) 
        { 
            this.x = x; 
            this.y = y; 
            this.z = z; 
        }

        // Existing properties and constructor

        public static bool operator == (Vec3 v1, Vec3 v2)
        {
            // Check for null on left side
            if (ReferenceEquals(v1, null))
            {
                return ReferenceEquals(v2, null);
            }

            // Check for actual equality
            return v1.Equals(v2);
        }

        public static bool operator !=(Vec3 v1, Vec3 v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            // Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Vec3 v = (Vec3)obj;
            return x == v.x && y == v.y && z == v.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public void Set( float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }
        public bool IsNaN()
        {
            return double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z);
        }

        /// <summary>
        /// Returns true if all components have values that are not NaN.
        /// </summary>
        public bool Valid()
        {
            return !IsNaN();
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        // Dot product
        public static float Dot(Vec3 lhs, Vec3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }
        // Cross product.
        public static Vec3 Cross(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(
                lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x
            );
        }
        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        public float LengthSquared()
        {
            return x * x + y * y + z * z;
        }
        public Vec3 Normalize()
        {
            float norm = Length();
            if (norm > 0.0f)
            {
                float inv = 1.0f / norm;
                x *= inv;
                y *= inv;
                z *= inv;
            }
            return this;
        }

        public static Vec3 operator *(float rhs, Vec3 lhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            lhs.z *= rhs;
            return lhs;
        }

        public static Vec3 operator *(Vec3 lhs, float rhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            lhs.z *= rhs;
            return lhs;
        }

        public static Vec3 operator *(Vec3 lhs, Vec3 rhs)
        {
            lhs.x *= rhs.x;
            lhs.y *= rhs.y;
            lhs.z *= rhs.z;
            return lhs;
        }

        // Negate.
        public static Vec3 operator -(Vec3 vector)
        {
            return new Vec3(-vector.x, -vector.y, -vector.z);
        }
        // Add
        public static Vec3 operator +(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        // subtract
        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }
        public static Vec3 operator /(Vec3 a, float b) => new Vec3(a.x / b, a.y / b, a.z / b);

    }
}
