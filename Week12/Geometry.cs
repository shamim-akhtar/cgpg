using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace CGPG
{
    public class Geometry : IDisposable
    {
        private float[]? _vertices = null;
        private uint[]? _indices = null;
        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader? _shader = null;
        private Texture? _texture = null;

        Matrix4 transform = Matrix4.Identity;

        private bool dirty = true;

        public Geometry()
        {
        }

        public void SetVertexArray(float[] vertices)
        {
            _vertices = vertices;
            dirty = true;
        }

        public void SetIndexArray(uint[] indices)
        {
            _indices = indices;
            dirty = true;
        }

        public void SetShader(string vertShaderFilename, string fragShaderFilename)
        {
            _shader = new Shader(vertShaderFilename, fragShaderFilename);
            dirty = true;
        }

        public void SetTexture(string textureFilename, TextureUnit unit = TextureUnit.Texture0)
        {
            _texture = Texture.LoadFromFile(textureFilename);
            _texture.Use(TextureUnit.Texture0);
            dirty = true;
        }

        public void SetTexture(Texture tex, TextureUnit unit = TextureUnit.Texture0)
        {
            _texture = tex;
            _texture.Use(TextureUnit.Texture0);
            dirty = true;
        }

        public Texture GetTexture()
        {
            return _texture;
        }

        public void SetTexture(byte[] data, int width, int height, TextureUnit unit = TextureUnit.Texture0)
        {
            Texture texture = Texture.CreatTextureFromRawData(width, height, data);
            SetTexture(texture, unit);
        }

        // For image manipulations in a texture.
        public void UpdateTexture(byte[] pixelData, int width, int height)
        {
            _texture.Use(TextureUnit.Texture0); // Bind the texture
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
        }

        public void Compile()
        {
            if(!dirty) return;

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(
                BufferTarget.ArrayBuffer, 
                _vertexBufferObject);

            GL.BufferData(
                BufferTarget.ArrayBuffer, 
                _vertices.Length * sizeof(float), 
                _vertices, 
                BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(
                BufferTarget.ElementArrayBuffer, 
                _elementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer, 
                _indices.Length * sizeof(uint), 
                _indices, 
                BufferUsageHint.StaticDraw);

            if(_shader != null) 
            {
                _shader.Use();

                var vertexLocation = _shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(
                    vertexLocation,
                    3,
                    VertexAttribPointerType.Float,
                    false,
                    5 * sizeof(float),
                    0);
            }

            if(_texture != null && _shader != null)
            {
                var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(
                    texCoordLocation, 
                    2, 
                    VertexAttribPointerType.Float, 
                    false, 
                    5 * sizeof(float), 
                    3 * sizeof(float));

                _texture.Use(TextureUnit.Texture0);
                _shader.SetInt("texture0", 0);
            }

            dirty = false;
        }

        public void SetMatrix(Mat4 m)
        {
            transform.M11 = m.matrix[0, 0];
            transform.M12 = m.matrix[0, 1];
            transform.M13 = m.matrix[0, 2];
            transform.M14 = m.matrix[0, 3];

            transform.M21 = m.matrix[1, 0];
            transform.M22 = m.matrix[1, 1];
            transform.M23 = m.matrix[1, 2];
            transform.M24 = m.matrix[1, 3];

            transform.M31 = m.matrix[2, 0];
            transform.M32 = m.matrix[2, 1];
            transform.M33 = m.matrix[2, 2];
            transform.M34 = m.matrix[2, 3];

            transform.M41 = m.matrix[3, 0];
            transform.M42 = m.matrix[3, 1];
            transform.M43 = m.matrix[3, 2];
            transform.M44 = m.matrix[3, 3];
        }

        public void Draw(Camera camera)
        {
            if (dirty) Compile();

            GL.BindVertexArray(_vertexArrayObject);

            if (_texture!=null)
            {
                _texture.Use(TextureUnit.Texture0);
            }
            if (_shader != null)
            {
                _shader.Use();
                _shader.SetMatrix4("model", transform);

                _shader.SetMatrix4("view", camera.GetViewMatrix());
                _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            }

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
        }
    }
}
