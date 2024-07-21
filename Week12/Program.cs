using CGPG;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Week11
{
    public static class Program
    {
        private static Texture? texture;
        private static Geometry? quad1;
        private static Geometry? quad2;
        private static int gridSize = 1;

        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1200, 600),
                Title = "CGPG - Week 11 - Image Manipulation",
                Flags = ContextFlags.ForwardCompatible,
            };

            var renderer = new Renderer(
                GameWindowSettings.Default, 
                nativeWindowSettings);

            //--------------------------------------------------------//
            // The original image.
            //--------------------------------------------------------//
            quad1 = Shapes.CreateQuad();
            quad1.SetTexture("Resources/building2.jpg");

            renderer.AddGeometry(quad1);

            Mat4 trans1 = new Mat4();
            trans1.MakeTranslate(-0.55f, -0.04f, 0);
            quad1.SetMatrix(trans1);
            //--------------------------------------------------------//

            //--------------------------------------------------------//
            // The manipulated image.
            //--------------------------------------------------------//
            quad2 = Shapes.CreateQuad();
            quad2.SetTexture("Resources/building2.jpg");

            renderer.AddGeometry(quad2);

            Mat4 trans = new Mat4();
            trans.MakeTranslate(0.55f, -0.04f, 0);
            quad2.SetMatrix(trans);
            //--------------------------------------------------------//

            renderer.OnUpdate = OnUpdateFrame;

            renderer.Run();            
        }
        public static (float x1, float y1) GenerateRandomNormalizedCoordinates()
        {
            Random random = new Random();
            float x1 = (float)random.NextDouble(); // Generates a random float between 0.0 and 1.0
            float y1 = (float)random.NextDouble(); // Generates a random float between 0.0 and 1.0
            return (x1, y1);
        }


        static void OnUpdateFrame(
            Renderer ren, 
            FrameEventArgs e, 
            KeyboardState keyboard)
        {
            int width = quad1.GetTexture().Width;
            int height = quad1.GetTexture().Height;
            byte[] original_data = quad1.GetTexture().GetRawData();
            byte[] changing_data = quad2.GetTexture().GetRawData();
            //if (keyboard.IsKeyPressed(Keys.R))
            //{
            //    // Set a red colour to the changing data and set it to quad2.
            //    byte[] data = ImageUtils.SetRedColor(
            //        changing_data, 
            //        width, 
            //        height);
            //    quad2.SetTexture(data, width, height);
            //}

            if (keyboard.IsKeyPressed(Keys.R))
            {
                // Set a red colour to the changing data and set it to quad2.
                byte[] data = ImageUtils.SwapRedWithGreen(
                    changing_data,
                    width,
                    height);
                quad2.SetTexture(data, width, height);
            }

            if (keyboard.IsKeyPressed(Keys.Q))
            {
                // Set a red colour to the changing data and set it to quad2.
                byte[] data = ImageUtils.Negate(
                    changing_data,
                    width,
                    height);
                quad2.SetTexture(data, width, height);
            }

            if (keyboard.IsKeyPressed(Keys.H))
            {
                // Set a red colour to the changing data and set it to quad2.
                byte[] data = ImageUtils.FlipImageHorizontally(
                    changing_data,
                    width,
                    height);
                quad2.SetTexture(data, width, height);
            }

            if (keyboard.IsKeyPressed(Keys.P))
            {
                // Set a red colour to the changing data and set it to quad2.
                byte[] data = ImageUtils.ApplyBoxBlur(
                    changing_data,
                    width,
                    height);
                quad2.SetTexture(data, width, height);
            }

            if (keyboard.IsKeyPressed(Keys.E))
            {
                // Set a red colour to the changing data and set it to quad2.
                byte[] data = ImageUtils.ApplySobelEdgeDetection(
                    changing_data,
                    width,
                    height);
                quad2.SetTexture(data, width, height);
            }
            if (keyboard.IsKeyPressed(Keys.C))
            {
                gridSize += 2;
                if (gridSize >= 64) gridSize = 64;
                byte[] data = ImageUtils.ApplyCheckerboardPattern(
                    original_data,
                    width,
                    height,
                    gridSize);
                quad2.SetTexture(data, width, height);
            }
            if (keyboard.IsKeyPressed(Keys.M))
            {
                (float x1, float y1) = GenerateRandomNormalizedCoordinates();
                (float x2, float y2) = GenerateRandomNormalizedCoordinates();
                (float x3, float y3) = GenerateRandomNormalizedCoordinates();
                byte[] data = ImageUtils.ApplyTriangleMask(
                    original_data,
                    width,
                    height,
                    x1, y1,
                    x2, y2,
                    x3, y3);
                quad2.SetTexture(data, width, height);
            }
            if (keyboard.IsKeyPressed(Keys.Space))
            {
                quad2.SetTexture(original_data, width, height);
                gridSize = 1;
            }
                
        }
    }
}
