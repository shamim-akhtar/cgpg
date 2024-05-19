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

        public Mat4()
        {

        }

        public Mat4(Mat4 other)
        {
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[i, j] = other.matrix[i, j];
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Format each element with 2 decimal places
                    result += matrix[i, j].ToString("F2") + "\t";
                }
                // Add a new line after each row
                result += "\n";
            }
            return result;
        }

        #region Multiply with a point
        public float[] MultiplyPoint(float x, float y, float z)
        {
            float[] res = new float[4];

            // Perform matrix multiplication
            res[0] = matrix[0, 0] * x + matrix[0, 1] * y + matrix[0, 2] * z + matrix[0, 3] * 1.0f;
            res[1] = matrix[1, 0] * x + matrix[1, 1] * y + matrix[1, 2] * z + matrix[1, 3] * 1.0f;
            res[2] = matrix[2, 0] * x + matrix[2, 1] * y + matrix[2, 2] * z + matrix[2, 3] * 1.0f;
            res[3] = matrix[3, 0] * x + matrix[3, 1] * y + matrix[3, 2] * z + matrix[3, 3] * 1.0f;

            return res;
        }
        #endregion

        #region Multiply with a point
        public float[] MultiplyVector(float x, float y, float z)
        {
            float[] res = new float[4];

            // Perform matrix multiplication
            res[0] = matrix[0, 0] * x + matrix[0, 1] * y + matrix[0, 2] * z;
            res[1] = matrix[1, 0] * x + matrix[1, 1] * y + matrix[1, 2] * z;
            res[2] = matrix[2, 0] * x + matrix[2, 1] * y + matrix[2, 2] * z;
            res[3] = matrix[3, 0] * x + matrix[3, 1] * y + matrix[3, 2] * z;

            return res;
        }
        #endregion

        #region Scale
        public void MakeScale(float x, float y, float z)
        {
            matrix[0, 0] = x;    matrix[0, 1] = 0.0f; matrix[0, 2] = 0.0f; matrix[0, 3] = 0.0f;
            matrix[1, 0] = 0.0f; matrix[1, 1] = y;    matrix[1, 2] = 0.0f; matrix[1, 3] = 0.0f;
            matrix[2, 0] = 0.0f; matrix[2, 1] = 0.0f; matrix[2, 2] = z;    matrix[2, 3] = 0.0f;
            matrix[3, 0] = 0.0f; matrix[3, 1] = 0.0f; matrix[3, 2] = 0.0f; matrix[3, 3] = 1.0f;
        }
        #endregion

        #region Scale
        public void MakeTranslate(float x, float y, float z)
        {
            matrix[0, 0] = 1.0f; matrix[0, 1] = 0.0f; matrix[0, 2] = 0.0f; matrix[0, 3] = 0.0f;
            matrix[1, 0] = 0.0f; matrix[1, 1] = 1.0f; matrix[1, 2] = 0.0f; matrix[1, 3] = 0.0f;
            matrix[2, 0] = 0.0f; matrix[2, 1] = 0.0f; matrix[2, 2] = 1.0f; matrix[2, 3] = 0.0f;
            matrix[3, 0] = x; matrix[3, 1] = y; matrix[3, 2] = z; matrix[3, 3] = 1.0f;
        }
        #endregion
    }
}
