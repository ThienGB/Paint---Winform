using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Paint2.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Paint2
{
    public partial class Form1 : Form
    {
        Pen myPen;
        SolidBrush myBrush;
        Graphics gp;

        bool isPress = false;

        bool bLine = false;
        bool bRect = false;
        bool bEcllipse = false;
        bool bCircle = false;
        bool bPolygon = false;
        bool bCurve = false;
        bool bDrawingPolygon = false;

        bool bSelect = false;

        bool bBr = false;
        int width = 2;
        Color color = Color.Black;
        DashStyle dash = DashStyle.Solid;
        Size originalSize;
        List<clsDrawObject> lstObject = new List<clsDrawObject>();
        List<Group> groups = new List<Group>();
        public Form1()
        {
            InitializeComponent();
            gp = ptbDrawing.CreateGraphics();
            myPen = new Pen(color, width);
            myPen.DashStyle = dash;
            myBrush = new SolidBrush(color);
            originalSize = ptbDrawing.Size;

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnPen_Click(object sender, EventArgs e)
        {

        }
        public void refreshDrawing()
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, ptbDrawing, new object[] { true });
            ptbDrawing.Refresh();
        }
        private void btnLine_Click(object sender, EventArgs e)
        {
            this.bLine = true;
            this.bRect = false;
            this.bEcllipse = false;
            this.bSelect = false;
            this.bPolygon = false;
            this.bCurve = false;
            this.bCircle = false;
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            this.bRect = true;
            this.bLine = false;
            this.bEcllipse = false;
            this.bSelect = false;
            this.bPolygon = false;
            this.bCurve = false;
            this.bCircle = false;
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            this.bEcllipse = true;
            this.bRect = false;
            this.bLine = false;
            this.bSelect = false;
            this.bPolygon = false;
            this.bCurve = false;
            this.bCircle = false;
        }
        private void btnPolygon_Click(object sender, EventArgs e)
        {
            this.bPolygon = true;
            this.bLine = false;
            this.bEcllipse = false;
            this.bRect = false;
            this.bCurve = false;
            this.bCircle = false;
            this.bSelect = false;

        }

        private void btnBezier_Click(object sender, EventArgs e)
        {
            this.bCurve = true;
            this.bLine = false;
            this.bEcllipse = false;
            this.bRect = false;
            this.bPolygon = false;
            this.bSelect = false;
            this.bCircle = false;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.bCurve = false;
            this.bLine = false;
            this.bEcllipse = false;
            this.bRect = false;
            this.bPolygon = false;
            this.bSelect = true;
            this.bCircle = false;
        }
        private void ptbDrawing_Paint_1(object sender, PaintEventArgs e)
        {
            if (bDrawingPolygon)
            {
                int n = this.lstObject[this.lstObject.Count - 1].PointList.Count - 1;
                if (this.lstObject[this.lstObject.Count - 1].PointList.Count == 2)
                {
                    e.Graphics.DrawLine(myPen, this.lstObject[this.lstObject.Count - 1].PointList[0], this.lstObject[this.lstObject.Count - 1].PointList[1]);
                    this.refreshDrawing();
                    for (int i = 0; i < this.lstObject.Count - 1; i++)
                    {
                        this.lstObject[i].Draw(e.Graphics, this.myPen, this.myBrush);
                    }
                }
                else
                {
                    for (int i = 0; i < this.lstObject.Count; i++)
                    {
                        this.lstObject[i].Draw(e.Graphics, this.myPen, myBrush);
                    }
                    this.refreshDrawing();
                }
            }
            else
            {
                for (int i = 0; i < this.lstObject.Count; i++)
                {
                    this.lstObject[i].Draw(e.Graphics, this.myPen, myBrush);
                }
            }


        }

        private void ptbDrawing_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (bDrawingPolygon)
            {
                int n = this.lstObject[this.lstObject.Count - 1].PointList.Count - 1;
                this.lstObject[this.lstObject.Count - 1].PointList[n] = e.Location;
                this.refreshDrawing();
            }
            else if (this.isPress == true && this.lstObject.Count > 0)
            {
                this.lstObject[this.lstObject.Count - 1].p2 = e.Location;
                this.refreshDrawing();
            }
        }

        private void ptbDrawing_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (lstObject.Count > 0)
            {
                if (bPolygon)
                {

                }
                else
                {
                    this.isPress = false;
                    this.lstObject[lstObject.Count - 1].p2 = e.Location;
                    // this.refreshDrawing();
                }
                if (bSelect)
                {
                    clsRec a = lstObject[lstObject.Count - 1] as clsRec;
                    for (int i = 0; i < lstObject.Count - 1; i++)
                    {
                        if (isBound(a, lstObject[i]))
                        {
                            //lstGrObject.Add(lstObject[i]);
                            lstObject[i].isSelect = true;
                            myPen.Color = Color.Red;
                            myPen.Width = 4;
                            if (lstObject[i].PointList.Count == 0)
                                gp.DrawRectangle(myPen, lstObject[i].p1.X, lstObject[i].p1.Y,
                                    lstObject[i].p2.X - lstObject[i].p1.X, lstObject[i].p2.Y - lstObject[i].p1.Y);
                            else
                            {
                                gp.DrawRectangle(myPen, lstObject[i].p1.X, lstObject[i].p1.Y,
                                    lstObject[i].pointTail.X - lstObject[i].p1.X, lstObject[i].pointTail.Y - lstObject[i].p1.Y);
                            }    
                            // MessageBox.Show("CHAM");
                        }
                    }
                    lstObject.RemoveAt(lstObject.Count - 1);
                    //this.refreshDrawing();
                }
                if (bLine)
                {
                    this.bLine = true;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = false;
                    this.bCircle = false;
                }

                if (bEcllipse)
                {
                    this.bLine = false;
                    this.bEcllipse = true;
                    this.bRect = false;
                    this.bPolygon = false;
                    this.bCircle = false;
                }

                if (bRect)
                {
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = true;
                    this.bPolygon = false;
                    this.bCircle = false;
                }
                if (bCircle)
                {
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = false;
                    this.bCircle = true;
                }
                if (bSelect)
                {
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = false;
                    this.bCircle = false;
                    this.bSelect = true;
                }
            }
        }
        public bool isBound(clsRec a, clsDrawObject b)
        {
            if (b.PointList.Count == 0)
            {
                if (a.p1.X < b.p1.X && a.p1.Y < b.p1.Y && a.p2.X > b.p2.X && a.p2.Y > b.p2.Y)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                foreach (Point p in b.PointList)
                {
                    if (a.p1.X > p.X || a.p1.Y > p.Y || a.p2.X < p.X || a.p2.Y < p.Y)
                        return false;
                    else
                    {
                        return true;
                    }
                }
                return true;
            }

        }
        private void ptbDrawing_MouseDown_1(object sender, MouseEventArgs e)
        {
            this.isPress = true;

            if (this.bLine)
            {
                clsDrawObject myObj;
                myObj = new clsLine();
                myObj.p1 = e.Location;
                myObj.isBr = bBr;
                myObj.width = width;
                myObj.color = color;
                myObj.dash = dash;
                lstObject.Add(myObj);
            }
            else if (this.bEcllipse)
            {
                clsDrawObject myObj;
                myObj = new clsEllipse();
                myObj.p1 = e.Location;
                myObj.isBr = bBr;
                myObj.width = width;
                myObj.color = color;
                myObj.dash = dash;
                lstObject.Add(myObj);
            }
            else if (this.bCircle)
            {
                clsDrawObject myObj;
                myObj = new clsCircle();
                myObj.p1 = e.Location;
                myObj.isBr = bBr;
                myObj.width = width;
                myObj.color = color;
                myObj.dash = dash;
                lstObject.Add(myObj);
            }
            else if (this.bRect)
            {
                clsDrawObject myObj;
                myObj = new clsRec();
                myObj.p1 = e.Location;
                myObj.isBr = bBr;
                myObj.width = width;
                myObj.color = color;
                myObj.dash = dash;
                lstObject.Add(myObj);
            }
            else if (this.bPolygon)
            {
                if (!bDrawingPolygon)
                {
                    clsDrawObject myObj;
                    myObj = new clsPolygon();
                    myObj.p1 = e.Location;
                    myObj.PointList.Add(e.Location);
                    myObj.PointList.Add(e.Location);
                    myObj.isBr = bBr;
                    myObj.width = width;
                    myObj.color = color;
                    myObj.dash = dash;
                    lstObject.Add(myObj);
                    bDrawingPolygon = true;
                }
                else
                {
                    clsPolygon myObj = lstObject[lstObject.Count - 1] as clsPolygon;
                    myObj.PointList.Add(e.Location);
                    lstObject[lstObject.Count - 1] = myObj;
                }
            }
            else if (this.bCurve)
            {
                if (!bDrawingPolygon)
                {
                    clsDrawObject myObj;
                    myObj = new clsCurve();
                    myObj.p1 = e.Location;
                    myObj.PointList.Add(e.Location);
                    myObj.PointList.Add(e.Location);
                    myObj.isBr = bBr;
                    myObj.width = width;
                    myObj.color = color;
                    myObj.dash = dash;
                    lstObject.Add(myObj);
                    bDrawingPolygon = true;
                }
                else
                {
                    clsCurve myObj = lstObject[lstObject.Count - 1] as clsCurve;
                    myObj.PointList.Add(e.Location);
                    lstObject[lstObject.Count - 1] = myObj;
                }
            }
            else if (this.bSelect)
            {
                Control control = sender as Control;
                if (lstObject.Count > 0 && lstObject[lstObject.Count - 1].isSelect)
                {
                    for (int i = 0; i < lstObject.Count; i++)
                    {
                        lstObject[i].isSelect = false;
                    }
                   // lstObject.RemoveAt(lstObject.Count - 1);
                    this.refreshDrawing();
                }
                Point a = e.Location;
                for (int i = 0; i < lstObject.Count; i++)
                {
                    Point p1 = lstObject[i].p1;
                    Point p2 = lstObject[i].p2;
                    
                    if (a.X > p1.X && a.Y > p1.Y && a.X < p2.X && a.Y < p2.Y)
                    {
                        if (lstObject[i].isSelect)
                        {
                            lstObject[i].isSelect = false;
                            refreshDrawing();
                            break;

                        }
                        if ((Control.ModifierKeys) == Keys.Control)
                        {
                            
                            lstObject[i].isSelect = true;
                            myPen.Color = Color.Red;
                            myPen.Width = 4;
                            myPen.DashStyle = DashStyle.Dash;
                            if (lstObject[i].PointList.Count == 0)
                                gp.DrawRectangle(myPen, p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                            else
                                gp.DrawRectangle(myPen, lstObject[i].p1.X, lstObject[i].p1.Y,
                                   lstObject[i].pointTail.X - lstObject[i].p1.X, lstObject[i].pointTail.Y - lstObject[i].p1.Y);

                        }
                        else
                        {
                            for (int j = 0; j < lstObject.Count; j++)
                                lstObject[j].isSelect = false;
                            refreshDrawing();
                            lstObject[i].isSelect = true;
                            myPen.Color = Color.Red;
                            myPen.Width = 4;
                            myPen.DashStyle = DashStyle.Dash;
                            if (lstObject[i].PointList.Count == 0)
                                gp.DrawRectangle(myPen, p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                            else
                                gp.DrawRectangle(myPen, lstObject[i].p1.X, lstObject[i].p1.Y,
                                   lstObject[i].pointTail.X - lstObject[i].p1.X, lstObject[i].pointTail.Y - lstObject[i].p1.Y);
                        }

                        /*else
                        {
                            lstObject[i].isSelect = true;
                            myPen.Color = Color.Red; 
                            myPen.Width = 4;
                            myPen.DashStyle = DashStyle.Dash;
                            if (lstObject[i].PointList.Count == 0)
                                gp.DrawRectangle(myPen, p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                            else
                                gp.DrawRectangle(myPen, lstObject[i].p1.X, lstObject[i].p1.Y,
                                   lstObject[i].pointTail.X - lstObject[i].p1.X, lstObject[i].pointTail.Y - lstObject[i].p1.Y);
                        }*/
                    }
                }
                clsDrawObject myObj;
                myObj = new clsRec();
                myObj.p1 = e.Location;
                myObj.isBr = false;
                myObj.width = 1;
                myObj.color = Color.Black;
                myObj.dash = DashStyle.Dash;
                myObj.isSelect = true;
                lstObject.Add(myObj);
            }
            if (lstObject.Count > 0 && !bSelect)
            {
                for (int i = 0; i < lstObject.Count; i++)
                {
                    lstObject[i].isSelect = false;
                }
            }    
        }
        private void ptbDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (bPolygon)
                {
                    bDrawingPolygon = false;
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = true;
                    bCurve = false;
                }
                else
                {
                    bDrawingPolygon = false;
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = false;
                    bCurve = true;
                }

            }
        }

        int i = 0;
        private void btnFill_Click(object sender, EventArgs e)
        {
            if (i == 0)
            {
                bBr = true;
                i = 1;
            }
            else if (i == 1)
            {
                bBr = false;
                i = 0;
            }

        }


        private void btnLineSize_Scroll(object sender, EventArgs e)
        {
            width = btnLineSize.Value;
        }

        public void refreshNumGroup(List<Group> groups, List<clsDrawObject> lstObject)
        {
            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i].isSelect && lstObject[i] is clsRec)
                {
                    for (int j = 0; j < groups.Count; j++)
                    {
                        if (lstObject[i] == groups[j].Rec)
                        {
                            for (int k = 0; k < lstObject.Count; k++)
                            {
                                for (int l = 0; l < groups[j].lstGrObject.Count; l++)
                                {
                                    if (groups[j].lstGrObject[l] == lstObject[k])
                                    {
                                        lstObject[k].NumGroup = j;
                                    }    
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i].isSelect && lstObject[i] is clsRec)
                {
                    for (int j = 0; j < groups.Count; j++)
                    {
                        if (lstObject[i] == groups[j].Rec)
                        {                            
                            lstObject.RemoveAt(i);
                            for (int k = 0; k < lstObject.Count; k++)
                            {
                                if (lstObject[k].NumGroup == j)
                                {
                                    lstObject.RemoveAt(k);
                                    k--;
                                }
                            }
                            groups.RemoveAt(j);
                            refreshNumGroup(groups, lstObject);
                            refreshDrawing();
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i].isSelect)
                {
                    lstObject.RemoveAt(i);
                    i--;
                }
            }           
            this.refreshDrawing();
        }
        private void Color_Click(object sender, EventArgs e)
        {
            PictureBox bt = (PictureBox)sender;
            color = bt.BackColor;
            ptbColor.BackColor = color;

            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i].isSelect)
                    lstObject[i].color = color;
            }
            refreshDrawing();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string a = cbbDash.SelectedItem.ToString();
            if (a == "Solid")
                dash = DashStyle.Solid;
            else if (a == "Dash")
                dash = DashStyle.Dash;
            else if (a == "Dot")
                dash = DashStyle.Dot;
            else if (a == "DashDot")
                dash = DashStyle.DashDot;
            else if (a == "DastDotDot")
                dash = DashStyle.DashDotDot;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bCircle = true;
            this.bPolygon = false;
            this.bLine = false;
            this.bEcllipse = false;
            this.bRect = false;
            this.bCurve = false;
            this.bSelect = false;
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            lstObject.Clear();
            this.refreshDrawing();
        }


        private void ptbDrawing_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (bPolygon)
                {
                    bDrawingPolygon = false;
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = true;
                    bCurve = false;
                    clsPolygon poly = lstObject[lstObject.Count - 1] as clsPolygon;
                    setPointHeadTailPoly(poly);
                    lstObject[lstObject.Count - 1] = poly;
                }
                else
                {
                    bDrawingPolygon = false;
                    this.bLine = false;
                    this.bEcllipse = false;
                    this.bRect = false;
                    this.bPolygon = false;
                    bCurve = true;
                    clsCurve cur = lstObject[lstObject.Count - 1] as clsCurve;
                    setPointHeadTailCurve(cur);
                    lstObject[lstObject.Count - 1] = cur;
                }

            }
        }

        private void ptbDrawing_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //e.Control.M
        }
        public void setPointHeadTailCurve(clsCurve curve)
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            curve.PointList.ForEach(p =>
            {
                if (minX > p.X)
                {
                    minX = p.X;
                }
                if (minY > p.Y)
                {
                    minY = p.Y;
                }
                if (maxX < p.X)
                {
                    maxX = p.X;
                }
                if (maxY < p.Y)
                {
                    maxY = p.Y;
                }
            });
            curve.p1 = new Point(minX, minY);
            curve.pointTail = new Point(maxX, maxY);
        }
        public void setPointHeadTailPoly(clsPolygon curve)
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            curve.PointList.ForEach(p =>
            {
                if (minX > p.X)
                {
                    minX = p.X;
                }
                if (minY > p.Y)
                {
                    minY = p.Y;
                }
                if (maxX < p.X)
                {
                    maxX = p.X;
                }
                if (maxY < p.Y)
                {
                    maxY = p.Y;
                }
            });
            curve.p1 = new Point(minX, minY);
            curve.pointTail = new Point(maxX, maxY);
        }
        public void setPointHeadTail(Group group)
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            for (int i = 0; i < group.lstGrObject.Count; i++)
            {
                clsDrawObject shape = group.lstGrObject[i];
                if (shape is clsCurve || shape is clsPolygon)
                    shape.p2 = shape.pointTail;
                if (shape.p1.X < minX)
                {
                    minX = shape.p1.X;
                }
                if (shape.p2.X < minX)
                {
                    minX = shape.p2.X;
                }

                if (shape.p1.Y < minY)
                {
                    minY = shape.p1.Y;
                }
                if (shape.p2.Y < minY)
                {
                    minY = shape.p2.Y;
                }

                if (shape.p1.X > maxX)
                {
                    maxX = shape.p1.X;
                }
                if (shape.p2.X > maxX)
                {
                    maxX = shape.p2.X;
                }

                if (shape.p1.Y > maxY)
                {
                    maxY = shape.p1.Y;
                }
                if (shape.p2.Y > maxY)
                {
                    maxY = shape.p2.Y;
                }
            }
            group.Rec.p1 = new Point(minX - 1 , minY - 1);
            group.Rec.p2 = new Point(maxX + 1, maxY + 1);
        }
        private void btnGroup_Click(object sender, EventArgs e)
        {
            Group gr = new Group();
            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i].isSelect)
                {
                    lstObject[i].NumGroup = groups.Count;
                    gr.lstGrObject.Add(lstObject[i]);
                    lstObject[i].isSelect = false;
                }                      
            }
            setPointHeadTail(gr);
            gr.Rec.color = Color.White;
            gr.Rec.width = 1;
            lstObject.Add(gr.Rec);
            groups.Add(gr);
            this.refreshDrawing();

        }

        private void btnUnGroup_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstObject.Count; i++)
            {
               if (lstObject[i].isSelect && lstObject[i] is clsRec)
               { 
                    for (int j = 0; j < groups.Count; j++)
                    {
                        if (lstObject[i] == groups[j].Rec)
                        {
                            groups.RemoveAt(j);
                            lstObject.RemoveAt(i);
                            refreshDrawing();
                            return;
                        }    
                    }    
               }    
            }    
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        float prezoomFator = 100;
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Zoom_ValueChanged(object sender, EventArgs e)
        {
            float zoomFactor = btnZoom.Value / 100.0f;
            
            ptbDrawing.Width = (int)(originalSize.Width * zoomFactor);
            ptbDrawing.Height = (int)(originalSize.Height * zoomFactor);
            //ptbDrawing.Size = new Size(ptbDrawing.Width / 2, ptbDrawing.Height / 2);
            for (int i = 0; i < lstObject.Count; i++)
            {
                if (lstObject[i] is clsCurve)
                {
                    clsCurve shapeCurve = lstObject[i] as clsCurve;
                    for (int j = 0; j < shapeCurve.PointList.Count; j++)
                    {
                        Point cur = shapeCurve.PointList[j];
                        cur.X = cur.X * 2;
                        cur.Y = cur.Y / 2;
                        shapeCurve.PointList[j] = cur;
                    }
                }
                else if (lstObject[i] is clsPolygon)
                {
                    clsPolygon ShapePolygon = lstObject[i] as clsPolygon;
                    for (int j = 0; j < ShapePolygon.PointList.Count; j++)
                    {
                        Point cur = ShapePolygon.PointList[j];
                        cur.X = cur.X / 2;
                        cur.Y = cur.Y / 2;
                        ShapePolygon.PointList[j] = cur;
                    }
                }
                else
                {
                    Point ph = lstObject[i].p1;
                    Point pt = lstObject[i].p2;
                    if (zoomFactor < prezoomFator)
                    {              
                        //ph.X = (int)(ph.X / 1.1);
                        ph.Y = (int)(ph.Y / 1.1);                       
                        pt.X = (int)(pt.X / 1.1);
                        pt.Y = (int)(pt.Y / 1.1);
                    }    
                    else
                    {
                        //ph.X = (int)(ph.X * 1.1);
                        ph.Y = (int)(ph.Y * 1.1);
                        pt.X = (int)(pt.X * 1.1);
                        pt.Y = (int)(pt.Y * 1.1);
                    }    
                    lstObject[i].p1 = ph;
                    lstObject[i].p2 = pt;
                    lstObject[i].pointTail = pt;
                }
                prezoomFator = zoomFactor;
                refreshDrawing();
            }
        }

        private void Zoom_Scroll(object sender, EventArgs e)
        {
            
/*            float zoomFactor = Zoom.Value / 100.0f; // Chuyển giá trị TrackBar thành tỷ lệ zoom

            // Thay đổi kích thước của panel
            ptbDrawing.Width = (int)(originalSize.Width * zoomFactor);
            ptbDrawing.Height = (int)(originalSize.Height * zoomFactor);
            refreshDrawing();*/
        }
    }
    
}
