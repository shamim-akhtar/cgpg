using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CGPG
{
    public static class Shapes
    {
        public static Geometry CreateQuad()
        {

            float[] vertices =
            {
                // Position         Texture coordinates
                0.5f, 0.5f, -0.5f, 1.0f, 1.0f, // top right
                0.5f, -0.5f,-0.5f, 1.0f, 0.0f, // bottom right
                -0.5f, -0.5f,-0.5f, 0.0f, 0.0f, // bottom left
                -0.5f, 0.5f, -0.5f, 0.0f, 1.0f  // top left
            };

            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };
            Geometry geometry = new Geometry();
            geometry.SetVertexArray(vertices);
            geometry.SetIndexArray(indices);
            geometry.SetShader("Shaders/Shader_PT0_MVP.vert", "Shaders/Shader_T0.frag");

            return geometry;
        }
        public static Geometry CreateCube()
        {
            float[] vertices = {
                // Positions          // Texture Coords
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f
            };


            uint[] indices =
            {
                // Front face
                0, 1, 2,
                2, 3, 0,
                // Back face
                4, 5, 6,
                6, 7, 4,
                // Left face
                4, 0, 3,
                3, 7, 4,
                // Right face
                1, 5, 6,
                6, 2, 1,
                // Top face
                3, 2, 6,
                6, 7, 3,
                // Bottom face
                4, 5, 1,
                1, 0, 4
            };
            Geometry geometry = new Geometry();
            geometry.SetVertexArray(vertices);
            geometry.SetIndexArray(indices);

            return geometry;
        }
    }
}
