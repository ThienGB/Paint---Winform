using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public class clsPolygon : clsDrawObject
    {

        public override void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush)
        {
            myPen.Color = this.color;
            myPen.Width = this.width;
            myPen.DashStyle = dash;
            myBrush = new SolidBrush(this.color);
            if (this.isBr)
                myGp.FillPolygon(myBrush, PointList.ToArray());
            else
                myGp.DrawPolygon(myPen, PointList.ToArray());

        }

    };
}
