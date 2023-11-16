using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public class clsCurve : clsDrawObject
    {

        public override void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush)
        {
            myPen.Color = this.color;
            myPen.Width = this.width;
            myPen.DashStyle = dash;
            myBrush = new SolidBrush(this.color);
            if (this.isBr)
                myGp.FillClosedCurve(myBrush, PointList.ToArray());
            else
                myGp.DrawCurve(myPen, PointList.ToArray());

        }

    };
}
