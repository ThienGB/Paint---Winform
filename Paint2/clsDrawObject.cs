using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public abstract class clsDrawObject
    {
        public Point p1;
        public Point p2;
        public bool isBr = false;
        public bool isSelect = false;
        public bool isGroup = false;
        public int width = 2;
        public Color color = Color.Black;
        public DashStyle dash = DashStyle.Solid;
        public Point pointTail;
        public int Swidth;
        public int Sheight;

        public int NumGroup = -1;

        public List<Point> PointList = new List<Point>();
        public abstract void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush);
    };
}
