using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintGD
{
    public partial class PAINT : Form
    {
        Point start, end, pre,startselection,endselection;
        int specialkey = 0;
        bool paintting = false, Check_Mouse = false, selected = false;
        int control = 0;
        int flag = 0;
        Point starttemp, endtemp;
        Color color = Color.Black;
        List<Image> backup = new List<Image>();
        public PAINT()
        {
            InitializeComponent();
        }

        //Vẽ đường thẳng và các hình liên quan
        public void LineDraw(Bitmap bmp, Point s, Point e, Color color)
        {
            int flag = 0;
            if (Math.Abs(e.Y - s.Y) > Math.Abs(e.X - s.X))
            {
                flag = 1;
                int temp = s.X;
                s.X = s.Y;
                s.Y = temp;
                temp = e.X;
                e.X = e.Y;
                e.Y = temp;
            }
            if (s.X > e.X)
            {
                Point temp = s;
                s = e;
                e = temp;
            }
            int dX = e.X - s.X;
            int dY = Math.Abs(e.Y - s.Y);
            int selectY = 1;
            if ((e.Y - s.Y) < 0) selectY = -1;
            int p = 2 * dY - dX;
            int x, y;
            y = s.Y;
            for (x = s.X; x <= e.X; x++)
            {
                if (p < 0) p += 2 * dY;
                else
                {
                    p += 2 * (dY - dX);
                    y = y + selectY;
                }
                if (flag == 1)
                { if (0 < y && y < VE.Width && 0 < x && x < VE.Height) bmp.SetPixel(y, x, color); }
                else if (flag == 0)
                { if (0 < x && x < VE.Width && 0 < y && y < VE.Height) bmp.SetPixel(x, y, color); }
            }

        }
        public void VeTamGiacVuong(Bitmap bmp, Point s, Point e, Color color)
        {
            Point A, B;
            A = new Point();
            B = new Point();
            A.X = s.X;
            A.Y = e.Y;
            B.Y = s.Y;
            B.X = e.X;
            LineDraw(bmp, s, A, color);
            LineDraw(bmp, s, B, color);
            LineDraw(bmp, A, B, color);
        }
        public void VeHinhChuNhat(Bitmap bmp, Point s, Point e, Color color)
        {
            Point A, B;
            A = new Point();
            B = new Point();
            A.X = s.X;
            A.Y = e.Y;
            B.Y = s.Y;
            B.X = e.X;
            LineDraw(bmp, s, A, color);
            LineDraw(bmp, s, B, color);
            LineDraw(bmp, e, B, color);
            LineDraw(bmp, e, A, color);
        }

        //Vẽ đường tròn
        public void put8pixel(Bitmap bmp, Point tam,int x, int y, Color color)
        {
            int a = tam.X;
            int b = tam.Y;
            if (0 < x + a && x + a < VE.Width && 0 < y + b && y + b < VE.Height) bmp.SetPixel(x + a, y + b, color);
            if (0 < -x + a && -x + a < VE.Width && 0 < y + b && y + b < VE.Height) bmp.SetPixel(-x + a, y + b, color);
            if (0 < x + a && x + a < VE.Width && 0 < -y + b && -y + b < VE.Height) bmp.SetPixel(x + a, -y + b, color);
            if (0 < -x + a && -x + a < VE.Width && 0 < -y + b && -y + b < VE.Height) bmp.SetPixel(-x + a, -y + b, color);
            if (0 < y + a && y + a < VE.Width && 0 < x + b && x + b < VE.Height) bmp.SetPixel(y + a, x + b, color);
            if (0 < -y + a && -y + a < VE.Width && 0 < x + b && x + b < VE.Height) bmp.SetPixel(-y + a, x + b, color);
            if (0 < y + a && y + a < VE.Width && 0 < -x + b && -x + b < VE.Height) bmp.SetPixel(y + a, -x + b, color);
            if (0 < -y + a && -y + a < VE.Width && 0 < -x + b && -x + b < VE.Height) bmp.SetPixel(-y + a, -x + b, color);
        }
        public void CircleMidpointDraw(Bitmap bmp, Point tam, Point end, Color color)
        {
            int R =(int)Math.Sqrt((tam.X - end.X) * (tam.X - end.X) + (tam.Y - end.Y) * (tam.Y - end.Y));
            int x = 0;
            int y = R;
            int p = 1 - R;
            put8pixel(bmp,tam,x,y,color);
            while (x < y)
            {
                if (p < 0) p += 2*x + 3;
                else
                {
                    y--;
                    p += 2*(x - y) + 5;
                }
                x++;
                put8pixel(bmp,tam,x,y,color);
            }
        }

        //Tô màu
        public void addQueue(Bitmap bmp, Point start, ref Queue<Point> Tomau, int color_paint)
        {
            Point temp = new Point();
            int x, y;
            temp.X = start.X;
            temp.Y = start.Y + 1;
            x = temp.X;
            y = temp.Y;
            if (bmp.GetPixel(temp.X, temp.Y).ToArgb() == color_paint && 1 <= x && x <= 869 && 1 <= y && y <= 469)
                Tomau.Enqueue(temp);

            temp.Y = start.Y;
            x = temp.X;
            y = temp.Y;
            if (bmp.GetPixel(temp.X, temp.Y).ToArgb() == color_paint && 1 <= x && x <= 869 && 1 <= y && y <= 469)
                Tomau.Enqueue(temp);

            temp.Y = start.Y - 1;
            x = temp.X;
            y = temp.Y;
            if (bmp.GetPixel(temp.X, temp.Y).ToArgb() == color_paint && 1 <= x && x <= 869 && 1 <= y && y <= 469)
                Tomau.Enqueue(temp);

            temp.X = start.X + 1;
            temp.Y = start.Y;
            x = temp.X;
            y = temp.Y;
            if (bmp.GetPixel(temp.X, temp.Y).ToArgb() == color_paint && 1 <= x && x <= 869 && 1 <= y && y <= 469)
                Tomau.Enqueue(temp);

            temp.X = start.X - 1;
            x = temp.X;
            y = temp.Y;
            if (bmp.GetPixel(temp.X, temp.Y).ToArgb() == color_paint && 1 <= x && x <= 869 && 1 <= y && y <= 469)
                Tomau.Enqueue(temp);
        }
        public void TomauLoang(Bitmap bmp, Point start, Color color)
        {
            Queue<Point> Tomau = new Queue<Point>();
            Point temp=new Point();
            int color_paint = bmp.GetPixel(start.X, start.Y).ToArgb();
            if (bmp.GetPixel(start.X, start.Y).ToArgb()==color_paint)
            {
                bmp.SetPixel(start.X, start.Y, color);
                addQueue(bmp, start, ref Tomau, color_paint);
            }
            while (Tomau.Count != 0)
            {
                temp = Tomau.Dequeue();
                if (bmp.GetPixel(temp.X, temp.Y).ToArgb() != color.ToArgb())
                {
                    bmp.SetPixel(temp.X, temp.Y, color);
                    addQueue(bmp, temp, ref Tomau, color_paint);
                }
            }

        }
        
        //Vẽ Elip
        public void put4pixel(Bitmap bmp, int centerx,int centery,int b2,int a2, Color color)
        {
            if (0 < centerx + a2 && centerx + a2 < VE.Width && 0 < centery + b2 && centery + b2 < VE.Height) bmp.SetPixel(centerx + a2, centery + b2, color);
            if (0 < centerx - a2 && centerx - a2 < VE.Width && 0 < centery + b2 && centery + b2 < VE.Height) bmp.SetPixel(centerx - a2, centery + b2, color);
            if (0 < centerx + a2 && centerx + a2 < VE.Width && 0 < centery - b2 && centery - b2 < VE.Height) bmp.SetPixel(centerx + a2, centery - b2, color);
            if (0 < centerx - a2 && centerx - a2 < VE.Width && 0 < centery - b2 && centery - b2 < VE.Height) bmp.SetPixel(centerx - a2, centery - b2, color);
        }
        public void VeElip(Bitmap bmp, Point s, Point e, Color color)
        {
            int centerx, centery, a, b;
            centerx = (s.X);
            centery = (e.Y);
            a = (int)Math.Sqrt((e.Y - s.Y) * (e.Y - s.Y));
            b = (int)Math.Sqrt((s.X - e.X) * (s.X - e.X));
            b = b / 2;
            a = a / 2;
            int a2,b2; 
            float r,p;
            a2=0; b2=b;
            r=(float)b/a;
            r=r*r; p=2*r-2*b+1;
            while (r*a2<=b2)
            {
                put4pixel(bmp, centerx, centery, a2, b2, color);
                if (p<0) p += 2*r*(2*a2+3);
                else
                {
                    p +=4*(1-b2)+2*r*(2*a2+3);
                    b2--;
                }
                a2++;
            }
            b2=0;a2=a;
            r= (float)a/b;
            r=r*r; p=2*r-2*a+1;
            while (r*b2<=a2)
            {
                put4pixel(bmp, centerx, centery, a2, b2, color);
                if (p<0) p +=2*r*(2*b2+3);
                else
                {
                    p +=4*(1-a2)+2*r*(2*b2+3);
                    a2--;
                }
                b2++;
            }
        }

        //Vẽ tự do
        private void VE_MouseMove(object sender, MouseEventArgs e)
        {
            end = e.Location;
            Bitmap bmp;
            if (VE.Image == null) bmp = new Bitmap(VE.Width, VE.Height);
            else bmp = (Bitmap)VE.Image.Clone();
            if (paintting)
            {
                if (specialkey == 0)
                {
                    switch (control)
                    {
                        case 1:
                            VE.Refresh();
                            break;
                        case 2:
                            VE.Refresh();
                            break;
                        case 3:
                            VE.Image = bmp;
                            VE.Refresh();
                            break;
                        case 4:
                            VE.Refresh();
                            break;
                        case 5:
                            VE.Refresh();
                            break;
                        case 6:
                            VE.Refresh();
                            break;
                        case 10: // Freedom draw
                            LineDraw(bmp, pre, end, color);
                            pre = end;
                            backup.Add(VE.Image);
                            VE.Image = bmp;
                            break;
                        case 11: // selection
                            VE.Refresh();
                            break;
                    }
                }
                else
                {
                    switch (control)
                    {
                        case 1:
                            LineDraw(bmp, start, end, color);
                            VE.Image = bmp;
                            break;
                        case 2:
                            CircleMidpointDraw(bmp, start, end, color);
                            VE.Image = bmp;
                            break;
                        case 3:
                            VeTamGiacVuong(bmp, start, end, color);
                            VE.Image = bmp;
                            break;
                        case 4:
                            VeElip(bmp, start, end, color);
                            VE.Image = bmp;
                            break;
                        case 5:
                            VeHinhChuNhat(bmp, start, end, color);
                            VE.Image = bmp;
                            break;
                        case 6:
                            VE.Refresh();
                            break;
                        case 10: // Freedom draw
                            LineDraw(bmp, pre, end, color);
                            pre = end;
                            backup.Add(VE.Image);
                            VE.Image = bmp;
                            break;
                        case 11: // selection
                            VE.Refresh();
                            break;
                    }
                }
            }
        }

        //Undo
        private void VE_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp;
            Point A;
            Point B;
            A = new Point(0, 0);
            B = new Point(0, 0);
            if (VE.Image == null) bmp = new Bitmap(VE.Width, VE.Height);
            else bmp = (Bitmap)VE.Image.Clone();
            if (paintting)
            {
                if (specialkey == 0)
                {
                    switch (control)
                    {
                        case 1:
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), start, end);
                            break;
                        case 2:
                            int R = (int)Math.Sqrt((start.X - end.X) * (start.X - end.X) + (start.Y - end.Y) * (start.Y - end.Y));
                            e.Graphics.DrawEllipse(new Pen(new SolidBrush(color)), start.X - R, start.Y - R, R + R, R + R);
                            break;
                        case 3:
                            A.X = start.X;
                            A.Y = end.Y;
                            B.Y = start.Y;
                            B.X = end.X;
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), start, A);
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), start, B);
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), A, B);
                            break;
                        case 4:
                            int centerx, centery, a, b;
                            centerx = start.X;
                            centery = end.Y;
                            a = (int)Math.Sqrt((end.Y - start.Y) * (end.Y - start.Y));
                            b = (int)Math.Sqrt((start.X - end.X) * (start.X - end.X));
                            e.Graphics.DrawEllipse(new Pen(new SolidBrush(color)), centerx, centery, b, a);
                            break;
                        case 5:
                            A.X = start.X;
                            A.Y = end.Y;
                            B.Y = start.Y;
                            B.X = end.X;
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), start, A);
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), start, B);
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), end, A);
                            e.Graphics.DrawLine(new Pen(new SolidBrush(color)), end, B);
                            break;
                        case 11: // selection
                            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(start, new Size(end.X - start.X, end.Y - start.Y)));
                            break;
                    }
                }
                else
                {
                    switch (control)
                    {
                        case 11: // selection
                            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(start, new Size(end.X - start.X, end.Y - start.Y)));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Clipping
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            selected = false;
            Deletebtn.Enabled = false;
            if (VE.Image == null) return;
            Bitmap bmp = (Bitmap)VE.Image.Clone();
            int i, j;
            for (i = 869; i > endselection.X; i--)
                for (j = 0; j < 469; j++)
                    bmp.SetPixel(i, j, VE.BackColor);
            for (i = startselection.X; i > 0; i--)
                for (j = 0; j < 469; j++)
                    bmp.SetPixel(i, j, VE.BackColor);
            for (i = 0; i < 869; i++)
                for (j = 0; j < startselection.Y; j++)
                    bmp.SetPixel(i, j, VE.BackColor);
            for (i = 0; i < 869; i++)
                for (j = endselection.Y; j < 469; j++)
                    bmp.SetPixel(i, j, VE.BackColor);
            backup.Add(VE.Image);
            VE.Image = bmp;
        }
        

        //Hàm xử lý chính
         private void VE_MouseUp(object sender, MouseEventArgs e)
        {
            end = e.Location;
            Bitmap bmp;
            if (VE.Image == null) bmp = new Bitmap(VE.Width, VE.Height);
            else bmp = (Bitmap)VE.Image.Clone();
            paintting = false;
            starttemp = endtemp = new Point(0,0);
            switch (control)
            {
                case 1:
                    LineDraw(bmp, start, end, color);
                    break;
                case 2:
                    CircleMidpointDraw(bmp, start, end,color);
                    flag = 0;
                    break;
                case 3:
                    VeTamGiacVuong(bmp, start, end, color);
                    break;
                case 4:
                    VeElip(bmp, start, end, color);
                    break;
                case 5:
                    VeHinhChuNhat(bmp, start, end, color);
                    break;
                case 6:
                    bmp = new Bitmap(VE.Width, VE.Height);
                    VE.Image = bmp;
                    break;
                case 9:
                    TomauLoang(bmp, e.Location, color);
                    break;
                case 11:
                    selected = true;
                    VeHinhChuNhat(bmp, start, end, Color.Black);
                    startselection = start;
                    endselection = end;
                    Deletebtn.Enabled = true;
                    break;
                default:
                    break;

            }
            if (control != 11)
            {
                backup.Add(VE.Image);
                VE.Image = bmp;
            }

        }
         private void VE_Click(object sender, EventArgs e)
         {
             VE.BackColor = Color.White;
         }
         private void VE_MouseDown(object sender, MouseEventArgs e)
         {
             VE.Refresh();
             start = e.Location;
             pre = start;
             selected = false;
             paintting = true;
             Check_Mouse = true;
         }

        //Control
        private void ctr1_Click(object sender, EventArgs e)
        {
            control = 1;
        }
        private void ctr2_Click(object sender, EventArgs e)
        {
            control = 2;
        }
        private void ctr3_Click(object sender, EventArgs e)
        {
            control = 3;
        }
        private void ctr4_Click(object sender, EventArgs e)
        {
            control = 4;
        }
        private void ctr5_Click(object sender, EventArgs e)
        {
            control = 5;
        }
        private void clrall_Click(object sender, EventArgs e)
        {
            control = 6;
        }   
        private void ctr7_Click(object sender, EventArgs e)
        {
            control = 7;
            if(backup.Count > 0)
            {
                VE.Image = backup[backup.Count - 1];
                backup.RemoveAt(backup.Count - 1);
            }
        }
        private void ctr9_Click(object sender, EventArgs e)
        {
            control = 9;
        }
        private void ctr10_Click(object sender, EventArgs e)
        {
            control = 10;
        }
        private void Selectbtn_Click(object sender, EventArgs e)
        {
            control = 11;
        }

        //Color
        private void button11_Click(object sender, EventArgs e)
        {
            color = button11.BackColor;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            color = button8.BackColor;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            color = button6.BackColor;
        }
        private void button14_Click(object sender, EventArgs e)
        {
            color = Color.Black;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            color = button10.BackColor;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            color = button9.BackColor;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            color = button13.BackColor;
        }
        private void button15_Click(object sender, EventArgs e)
        {
            color = button15.BackColor;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            color = button4.BackColor;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            color = button7.BackColor;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            color = button5.BackColor;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            color = button3.BackColor;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            color = button2.BackColor;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            color = button1.BackColor;
        }
        private void button11_Click_1(object sender, EventArgs e)
        {
            color = Color.White;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            color = button12.BackColor;
        }
        private void button16_Click(object sender, EventArgs e)
        {
            color = button16.BackColor;
        }

        private void special_Click(object sender, EventArgs e)
        {
            specialkey = 1;
        }

        private void Normal_Click(object sender, EventArgs e)
        {
            specialkey = 0;
        }

    }
}