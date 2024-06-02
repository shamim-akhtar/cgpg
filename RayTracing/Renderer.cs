using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace CGPG
{
    // Be warned, there is a LOT of stuff here. It might seem complicated, but just take it slow and you'll be fine.
    // OpenGL's initial hurdle is quite large, but once you get past that, things will start making more sense.
    public class Renderer : GameWindow
    {
        private float[]? _vertices = null;
        private uint[]? _indices = null;

        public void SetVertexArray(float[] vertices)
        {
            _vertices = vertices;
        }
        public void SetIndexArray(uint[] indices) 
        { 
            _indices = indices; 
        }
        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;

        //private Texture _texture2;
        Matrix4 transform = Matrix4.Identity;

        public Renderer(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // shader.frag has been modified yet again, take a look at it as well.
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.CreateEmptyRedTexture(512, 512);// Texture.LoadFromFile("Resources/cartoon-house.jpg");
            // Texture units are explained in Texture.cs, at the Use function.
            // First texture goes in texture unit 0.
            _texture.Use(TextureUnit.Texture0);

            // Next, we must setup the samplers in the shaders to use the right textures.
            // The int we send to the uniform indicates which texture unit the sampler should use.
            _shader.SetInt("texture0", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if (_indices == null)
            {
                Console.WriteLine("Index array is null");
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vertexArrayObject);

            _texture.Use(TextureUnit.Texture0);
            _shader.Use();

            // Create the transformation matrix.
            _shader.SetMatrix4("transform", transform);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        public delegate void DelegateOnUpdate(Renderer ren, FrameEventArgs e, KeyboardState keyboard);
        public DelegateOnUpdate? OnUpdate = null;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            OnUpdate?.Invoke(this, e, input);

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            //byte[] newPixelData = GeneratePixelData(256, 256);
            //UpdateTextureData(newPixelData, 256, 256);

            //byte[] newPixelData = GeneratePixelData(_texture.Width, _texture.Height);
            //UpdateTextureData(newPixelData, _texture.Width, _texture.Height);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }
        public void UpdateTextureData(byte[] pixelData, int width, int height)
        {
            _texture.Use(TextureUnit.Texture0); // Bind the texture
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
        }

        // Example method to generate pixel data
        private byte[] GeneratePixelData(int width, int height)
        {
            byte[] pixels = new byte[width * height * 4];
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 255;     // R
                pixels[i + 1] = 0;   // G
                pixels[i + 2] = 255;   // B
                pixels[i + 3] = 255; // A
            }
            return pixels;
        }


    }
}
