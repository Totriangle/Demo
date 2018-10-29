using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.AffineTransform
{
    public class UserMatrix
    {
        public static Matrix initMatrix = new Matrix(1, 0, 0, -1, 30, 500);

        public static Matrix ScaleByPoint(float scaleFactor,PointF scalePoint)
        {
            Matrix matrix = new Matrix(scaleFactor, 0, 0, scaleFactor, (1 - scaleFactor) * scalePoint.X, (1 - scaleFactor) * scalePoint.Y);
            initMatrix.Multiply(matrix, MatrixOrder.Prepend);
            return matrix;
        }

        public static PointF GetMousePoint(PointF point)
        {
            Matrix matrix = new Matrix(1 / initMatrix.Elements[0], 0, 0, 1 / initMatrix.Elements[3], -initMatrix.Elements[4] / initMatrix.Elements[0], -initMatrix.Elements[5] / initMatrix.Elements[3]);
            PointF[] pot = new PointF[] { point };
            matrix.TransformPoints(pot);
            return pot[0];
        }

        public static PointF GetMousePoint(float x,float y)
        {
            PointF point = new PointF(x, y);
            return GetMousePoint(point);
        }
    }
}
