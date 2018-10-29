using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.AffineTransform
{
    public class CoordinateShape
    {
        public static void Draw(Graphics g)
        {
            g.DrawLine(Pens.White, -6, 0, 60, 0);//x line
            g.DrawLine(Pens.White, 0, -6, 0, 60);//y line
            PointF[] xArrows = new PointF[] { new PointF(60, 6), new PointF(60, -6), new PointF(75, 0) };
            PointF[] yArrows = new PointF[] { new PointF(-6, 60), new PointF(6, 60), new PointF(0, 75) };
            g.DrawPolygon(Pens.White, xArrows);
            g.DrawPolygon(Pens.White, yArrows);
            g.DrawRectangle(Pens.White, -6, -6, 12, 12);

            g.DrawLine(Pens.White, 90, 8, 102, -8);//x text
            g.DrawLine(Pens.White, 90, -8, 102, 8);

            g.DrawLine(Pens.White, 6, 101, 0, 93);//y text
            g.DrawLine(Pens.White, -6, 101, 0, 93);
            g.DrawLine(Pens.White, 0, 93, 0, 85);
        }
    }
}
