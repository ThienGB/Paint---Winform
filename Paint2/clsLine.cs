using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public class clsLine : clsDrawObject
    {
        public override void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush)
        {
            myPen.Color = this.color;
            myPen.Width = this.width;
            myPen.DashStyle = dash;
            myGp.DrawLine(myPen, this.p1, this.p2);
        }

    };
}
