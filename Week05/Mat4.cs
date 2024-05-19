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
    }
}
