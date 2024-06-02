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
                    result += matrix[j, i].ToString("F2") + "\t";
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

        #region Rotate
        public void MakeRotate(float angleX, float angleY, float angleZ)
        {
            float radX = angleX * (float)Math.PI / 180.0f;
            float radY = angleY * (float)Math.PI / 180.0f;
            float radZ = angleZ * (float)Math.PI / 180.0f;

            Mat4 rotationX = new Mat4();
            Mat4 rotationY = new Mat4();
            Mat4 rotationZ = new Mat4();

            rotationX.matrix[1, 1] = (float)Math.Cos(radX);
            rotationX.matrix[1, 2] = (float)Math.Sin(radX);
            rotationX.matrix[2, 1] = -(float)Math.Sin(radX);
            rotationX.matrix[2, 2] = (float)Math.Cos(radX);

            rotationY.matrix[0, 0] = (float)Math.Cos(radY);
            rotationY.matrix[0, 2] = -(float)Math.Sin(radY);
            rotationY.matrix[2, 0] = (float)Math.Sin(radY);
            rotationY.matrix[2, 2] = (float)Math.Cos(radY);

            rotationZ.matrix[0, 0] = (float)Math.Cos(radZ);
            rotationZ.matrix[0, 1] = (float)Math.Sin(radZ);
            rotationZ.matrix[1, 0] = -(float)Math.Sin(radZ);
            rotationZ.matrix[1, 1] = (float)Math.Cos(radZ);

            Mat4 result = new Mat4();
            result.Mult(rotationX, rotationY);
            result.Mult(result, rotationZ);

            this.matrix = result.matrix;
        }
        #endregion

        #region
        private float INNER_PRODUCT(Mat4 a, Mat4 b, int r,int c)
        {
            return
                (a.matrix[r,0] * b.matrix[0,c]) +
                (a.matrix[r,1] * b.matrix[1,c]) +
                (a.matrix[r,2] * b.matrix[2,c]) +
                (a.matrix[r,3] * b.matrix[3,c]);
        }

        public void PreMult(Mat4 other)
        {
            float[] t = { 0.0f, 0.0f, 0.0f, 0.0f };
            for (int col = 0; col < 4; ++col)
            {
                t[0] = INNER_PRODUCT(other, this, 0, col);
                t[1] = INNER_PRODUCT(other, this, 1, col);
                t[2] = INNER_PRODUCT(other, this, 2, col);
                t[3] = INNER_PRODUCT(other, this, 3, col);
                matrix[0,col] = t[0];
                matrix[1,col] = t[1];
                matrix[2,col] = t[2];
                matrix[3,col] = t[3];
            }
        }

        public void SetRow(int row, float v1, float v2, float v3, float v4 )
        {
            matrix[row,0] = v1;
            matrix[row,1] = v2;
            matrix[row,2] = v3;
            matrix[row,3] = v4;
        }
        public void PostMult(Mat4 other)
        {
            float[] t = { 0.0f, 0.0f, 0.0f, 0.0f };
            for(int row=0; row<4; ++row)
            {
                t[0] = INNER_PRODUCT( this, other, row, 0 );
                t[1] = INNER_PRODUCT( this, other, row, 1 );
                t[2] = INNER_PRODUCT( this, other, row, 2 );
                t[3] = INNER_PRODUCT( this, other, row, 3 );
                SetRow(row, t[0], t[1], t[2], t[3]);
            }
        }

        public void Mult(Mat4 lhs, Mat4 rhs )
        {
            if (lhs==this)
            {
                PostMult(rhs);
                return;
            }
            if (rhs==this)
            {
                PreMult(lhs);
                return;
            }

            // PRECONDITION: We assume neither lhs nor rhs == this
            // if it did, use preMult or postMult instead
            matrix[0,0] = INNER_PRODUCT(lhs, rhs, 0, 0);
            matrix[0,1] = INNER_PRODUCT(lhs, rhs, 0, 1);
            matrix[0,2] = INNER_PRODUCT(lhs, rhs, 0, 2);
            matrix[0,3] = INNER_PRODUCT(lhs, rhs, 0, 3);
            matrix[1,0] = INNER_PRODUCT(lhs, rhs, 1, 0);
            matrix[1,1] = INNER_PRODUCT(lhs, rhs, 1, 1);
            matrix[1,2] = INNER_PRODUCT(lhs, rhs, 1, 2);
            matrix[1,3] = INNER_PRODUCT(lhs, rhs, 1, 3);
            matrix[2,0] = INNER_PRODUCT(lhs, rhs, 2, 0);
            matrix[2,1] = INNER_PRODUCT(lhs, rhs, 2, 1);
            matrix[2,2] = INNER_PRODUCT(lhs, rhs, 2, 2);
            matrix[2,3] = INNER_PRODUCT(lhs, rhs, 2, 3);
            matrix[3,0] = INNER_PRODUCT(lhs, rhs, 3, 0);
            matrix[3,1] = INNER_PRODUCT(lhs, rhs, 3, 1);
            matrix[3,2] = INNER_PRODUCT(lhs, rhs, 3, 2);
            matrix[3,3] = INNER_PRODUCT(lhs, rhs, 3, 3);
        }
        #endregion

        #region LookAt matrix for camera.
        public void MakeLookAt(Vec3 eye, Vec3 center, Vec3 up)
        {
            Vec3 f = center - eye;
            f.Normalize();
            Vec3 s = Vec3.Cross(f, up);
            s.Normalize();
            Vec3 u = Vec3.Cross(s, f);

            matrix[0, 0] = s.x;
            matrix[0, 1] = u.x;
            matrix[0, 2] = -f.x;
            matrix[0, 3] = 0.0f;

            matrix[1, 0] = s.y;
            matrix[1, 1] = u.y;
            matrix[1, 2] = -f.y;
            matrix[1, 3] = 0.0f;

            matrix[2, 0] = s.z;
            matrix[2, 1] = u.z;
            matrix[2, 2] = -f.z;
            matrix[2, 3] = 0.0f;

            matrix[3, 0] = -Vec3.Dot(s, eye);
            matrix[3, 1] = -Vec3.Dot(u, eye);
            matrix[3, 2] = Vec3.Dot(f, eye);
            matrix[3, 3] = 1.0f;
        }
        #endregion

        #region Perspective
        public void MakePerspective(float fovY, float aspect, float near, float far)
        {
            float f = 1.0f / (float)Math.Tan(fovY / 2.0f);
            matrix[0, 0] = f / aspect;
            matrix[0, 1] = 0.0f;
            matrix[0, 2] = 0.0f;
            matrix[0, 3] = 0.0f;

            matrix[1, 0] = 0.0f;
            matrix[1, 1] = f;
            matrix[1, 2] = 0.0f;
            matrix[1, 3] = 0.0f;

            matrix[2, 0] = 0.0f;
            matrix[2, 1] = 0.0f;
            matrix[2, 2] = (far + near) / (near - far);
            matrix[2, 3] = (2 * far * near) / (near - far);

            matrix[3, 0] = 0.0f;
            matrix[3, 1] = 0.0f;
            matrix[3, 2] = -1.0f;
            matrix[3, 3] = 0.0f;
        }
        #endregion

        #region Orthographic
        public void MakeOrthographic(float left, float right, float bottom, float top, float near, float far)
        {
            matrix[0, 0] = 2.0f / (right - left);
            matrix[0, 1] = 0.0f;
            matrix[0, 2] = 0.0f;
            matrix[0, 3] = -(right + left) / (right - left);

            matrix[1, 0] = 0.0f;
            matrix[1, 1] = 2.0f / (top - bottom);
            matrix[1, 2] = 0.0f;
            matrix[1, 3] = -(top + bottom) / (top - bottom);

            matrix[2, 0] = 0.0f;
            matrix[2, 1] = 0.0f;
            matrix[2, 2] = -2.0f / (far - near);
            matrix[2, 3] = -(far + near) / (far - near);

            matrix[3, 0] = 0.0f;
            matrix[3, 1] = 0.0f;
            matrix[3, 2] = 0.0f;
            matrix[3, 3] = 1.0f;
        }
        #endregion
    }
}
