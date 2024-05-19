using System;

namespace CGPG
{
    public class Mat4
    {
        // 3x3 matrix
        public float[,] matrix =
        {
            {1.0f, 0.0f, 0.0f, 0.0f},
            {0.0f, 1.0f, 0.0f, 0.0f},
            {0.0f, 0.0f, 1.0f, 0.0f},
            {0.0f, 0.0f, 0.0f, 1.0f}
        };

        #region default Constructor
        public Mat4()
        {

        }
        #endregion

        #region Copy Constructor
        public Mat4(Mat4 other)
        {
            // Implement here
        }
        #endregion

        public override string ToString()
        {
            string result = "";
            // Implement here

            return result;
        }

        #region Scale
        public void MakeScale(float x, float y, float z)
        {
            // Implement here
        }
        #endregion

        #region Translate
        public void MakeTranslate(float x, float y, float z)
        {
            // Implement here
        }
        #endregion
    }
}
