﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint2
{
    public class clsEllipse : clsDrawObject
    {
        public override void Draw(Graphics myGp, Pen myPen, SolidBrush myBrush)
        {
            myPen.Color = this.color;
            myPen.Width = this.width;
            myPen.DashStyle = dash;
            myBrush = new SolidBrush(this.color);
            if (this.isBr)
                myGp.FillEllipse(myBrush, this.p1.X, this.p1.Y, this.p2.X - this.p1.X, this.p2.Y - this.p1.Y);
            else
                myGp.DrawEllipse(myPen, this.p1.X, this.p1.Y, this.p2.X - this.p1.X, this.p2.Y - this.p1.Y);

        }
    };
}
