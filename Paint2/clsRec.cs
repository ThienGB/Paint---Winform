using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public class clsRec : clsDrawObject
    {
        public override void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush)
        {
            myPen.Color = this.color;
            myPen.Width = this.width;
            myPen.DashStyle = dash;
            myBrush = new SolidBrush(this.color);
            Swidth = this.p2.X - this.p1.X;
            Sheight = this.p2.Y - this.p1.Y;
            if (this.isBr)
            {
                myGp.FillRectangle(myBrush, this.p1.X, this.p1.Y, Swidth, Sheight);
            }
            else
                myGp.DrawRectangle(myPen, this.p1.X, this.p1.Y, Swidth, Sheight);
        }

    };
}
