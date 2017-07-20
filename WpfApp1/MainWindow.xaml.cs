using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private map m;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Clear();
            canvas.Init();
            int x =Convert.ToInt32(textBox.Text), y=Convert.ToInt32(textBox_Copy.Text);
            m=new map(x, y);
            m.Height = Convert.ToInt32(textBox_Height.Text);
            m.Width = Convert.ToInt32(textBox_Width.Text);
            m.X = Convert.ToInt32(textBox_X.Text);
            m.Y = Convert.ToInt32(textBox_Y.Text);
            canvas.Draw(ref m);
        }

        private void textBox_Height_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(canvas);
            VisualWitness visual = canvas.GetVisual(pointClicked);

            if (visual != null)
            {
                int oriX = m.X, oriY = m.Y;
                int height = m.Height, width = m.Width;
                int round = 5;
                if (visual.type == "cLine")
                {
                    canvas.cLine[visual.x, visual.y].status = (canvas.cLine[visual.x, visual.y].status + 1) % 3;

                }
                if (visual.type == "rLine")
                {
                    canvas.rLine[visual.x, visual.y].status = (canvas.rLine[visual.x, visual.y].status + 1) % 3;
                }
                if (visual.type == "Point")
                {
                    label2_Copy1.Content = visual.x;
                    label2_Copy2.Content = visual.y;
                }
                if (visual.type == "Square")
                {
                    label2.Content = Convert.ToInt32(visual.x);
                    label2_Copy.Content = Convert.ToInt32(visual.y);
                    canvas.onClick_x = visual.x;
                    canvas.onClick_y = visual.y;
                    if (canvas.s[visual.x, visual.y].squ == -1)
                    {
                        checkBox.IsChecked = false;
                        comboBox.SelectedIndex = -1;
                    }
                    else
                    {
                        checkBox.IsChecked = true;
                        comboBox.SelectedIndex = canvas.s[visual.x, visual.y].squ;
                    }
                    if (canvas.s[visual.x, visual.y].oct == -1)
                    {
                        checkBox2.IsChecked = false;
                        comboBox1.SelectedIndex=-1;
                    }
                    else
                    {
                        checkBox2.IsChecked = true;
                        comboBox1.SelectedIndex = canvas.s[visual.x, visual.y].oct;
                    }
                }
            }
            else
            {
                canvas.onClick_x = m.row;
                canvas.onClick_y = m.column;
            }
                canvas.Clear();
                canvas.Draw(ref m);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            m.Start_Point.Clear();
            m.End_Point.Clear();
            for(int i = 0; i < canvas.xxx; i++)
            {
                VisualWitness v = canvas.GetVisualWitnessChild(i);
                if (v.type == "rLine")
                {
                    m.rLine[v.x, v.y] = canvas.rLine[v.x, v.y];
                }
                if (v.type == "cLine")
                {
                    m.cLine[v.x, v.y] = canvas.cLine[v.x, v.y];
                }
                if (v.type == "Point")
                {
                    m.p[v.x, v.y] = canvas.p[v.x, v.y];
                    if (canvas.p[v.x, v.y].status == 2) m.Start_Point.Add(canvas.p[v.x, v.y]);
                    if (canvas.p[v.x, v.y].status == 3) m.End_Point.Add(canvas.p[v.x, v.y]);
                }
                if (v.type == "Square")
                {
                    m.s[v.x, v.y] = canvas.s[v.x, v.y];
                }
            }
            if (m.Start_Point.Count == 0)
            {
                label3.Content = "NO START POINT!!";
                return;
            }
            else
            {
                label3.Content = "";
            }
            if (m.End_Point.Count == 0)
            {
                label3.Content = "NO START POINT!!";
                return;
            }
            else
            {
                label3.Content = "";
            }
            m.solve();
            for(int i = 0; i <= m.row; i++)
            {
                for(int j = 0; j < m.column; j++)
                {
                    if (m.visitedrLine[i, j] == 1)
                    {
                        canvas.DrawrLine(ref m, i, j, Brushes.Yellow);
                    }
                }
            }
            for (int i = 0; i < m.row; i++)
            {
                for (int j = 0; j <= m.column; j++)
                {
                    if (m.visitedcLine[i, j] == 1)
                    {
                        canvas.DrawcLine(ref m, i, j, Brushes.Yellow);
                    }
                }
            }
            
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(label2.Content);
            int y = Convert.ToInt32(label2_Copy.Content);
            if (checkBox.IsChecked != true)
            {
                canvas.s[x, y].squ = -1;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = Convert.ToInt32(label2.Content);
            int y = Convert.ToInt32(label2_Copy.Content);
            canvas.s[x, y].squ = comboBox.SelectedIndex;
            canvas.Clear();
            canvas.Draw(ref m);
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(label2.Content);
            int y = Convert.ToInt32(label2_Copy.Content);
            if (checkBox2.IsChecked != true)
            {
                canvas.s[x, y].oct = -1;
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = Convert.ToInt32(label2.Content);
            int y = Convert.ToInt32(label2_Copy.Content);
            canvas.s[x, y].oct = comboBox1.SelectedIndex;
            canvas.Clear();
            canvas.Draw(ref m);
        }

        private void comboBox11_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canvas.background = comboBox1_Copy.SelectedIndex;
            canvas.Clear();
            canvas.Draw(ref m);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(label2_Copy1.Content);
            int y = Convert.ToInt32(label2_Copy2.Content);
            if (canvas.p[x, y].status == 2) canvas.p[x, y].status = 0;
            else canvas.p[x, y].status = 2;
            canvas.Clear();
            canvas.Draw(ref m);
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(label2_Copy1.Content);
            int y = Convert.ToInt32(label2_Copy2.Content);
            if (canvas.p[x, y].status == 3) canvas.p[x, y].status = 0;
            else canvas.p[x, y].status = 3;
            canvas.Clear();
            canvas.Draw(ref m);
        }

        private void button2_Copy1_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(label2_Copy1.Content);
            int y = Convert.ToInt32(label2_Copy2.Content);
            if (canvas.p[x, y].status == 1) canvas.p[x, y].status = 0;
            else canvas.p[x, y].status = 1;
            canvas.Clear();
            canvas.Draw(ref m);
        }
    }

    public class VisualWitness : DrawingVisual
    {
        private string _type;
        public int x;
        public int y;
        public int onClick;

        public string type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }


    }

    public class DrawingCanvas : Canvas
    {
        private List<VisualWitness> visuals = new List<VisualWitness>();
        public line[,] rLine;
        public line[,] cLine;
        public point[,] p;
        public square[,] s;
        public int onClick_x;
        public int onClick_y;
        public int background=1;

        public DrawingCanvas()
        {
            this.Init();
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        public int xxx
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        public VisualWitness GetVisualWitnessChild(int index)
        {
            return visuals[index];
        }

        public void AddVisual(VisualWitness visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void RemoveVisual(VisualWitness visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public VisualWitness GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as VisualWitness;
        }

        public void Clear()
        {
            int x = this.VisualChildrenCount;
            for (int i = 0; i < x; i++)
            {
                this.RemoveVisual(this.GetVisualWitnessChild(0));
            }
        }

        public void Init()
        {
            rLine = new line[11, 10];
            cLine = new line[10, 11];
            p = new point[11, 11];
            s = new square[10, 10];
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    rLine[i, j] = new line(i,j);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    cLine[i, j] = new line(i,j);
                }
            }
            for(int i = 0; i < 11; i++)
            {
                for(int j = 0; j < 11; j++)
                {
                    p[i, j] = new point(i, j);
                }
            }
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    s[i, j] = new square(i, j);
                }
            }
        }

        public void DrawLine(ref map m)
        {
            Brush b;
            for (int i = 0; i <= m.row; i++)
            {
                for (int j = 0; j < m.column; j++)
                {
                    if (rLine[i, j].status == 1) b = Brushes.White;
                    
                    else b = Brushes.Black;
                    this.DrawrLine(ref m, i, j, b);
                }
            }
            for (int i = 0; i < m.row; i++)
            {
                for (int j = 0; j <= m.column; j++)
                {
                    if (cLine[i, j].status == 1) b = Brushes.White;
                    
                    else b = Brushes.Black;
                    this.DrawcLine(ref m, i, j, b);
                }
            }

            //return visual;
        }

        public void DrawAllSquare(ref map m)
        {
            for (int i = 0; i < m.row; i++)
            {
                for (int j = 0; j < m.column; j++)
                {
                    this.DrawSquare(ref m, i, j, GetBrushes(background));
                }
            }

        }

        public void DrawAllPoint(ref map m)
        {
            Brush b;
            for (int i = 0; i <= m.row; i++)
            {
                for (int j = 0; j <= m.column; j++)
                {
                    if (p[i, j].status == 1) b = Brushes.Gray;
                    else b = Brushes.Red;
                    this.DrawPoint(ref m, i, j, b);
                }
            }
        }

        public void Draw(ref map m)
        {
            this.DrawLine(ref m);
            this.DrawAllSquare(ref m);
            this.DrawAllPoint(ref m);
        }

        public void DrawrLine(ref map m, int i, int j, Brush b)
        {
            VisualWitness visual = new VisualWitness();
            DrawingContext dc = visual.RenderOpen();
            int oriX = m.X, oriY = m.Y;
            int height = m.Height, width = m.Width;
            int round = 5;
            Rect r = new Rect(oriX + j * (height + width), oriY + i * (height + width), width + 2 * height, height);
            dc.DrawRoundedRectangle(b, null, r, round, round);
            if (rLine[i, j].status == 2)
            {
                int standX = oriX + height + width / 2 + j * (height + width);
                int standY = oriY + height / 2 + i * (height + width);
                int radius = height / 2;

                StreamGeometry g = GetHex(standX, standY, radius);

                dc.DrawGeometry(Brushes.Gray, null, g);
            }
            dc.Close();
            visual.type = "rLine";
            visual.x = i;
            visual.y = j;
            this.AddVisual(visual);
        }

        public void DrawcLine(ref map m, int i, int j, Brush b)
        {
            VisualWitness visual = new VisualWitness();
            DrawingContext dc = visual.RenderOpen();
            int oriX = m.X, oriY = m.Y;
            int height = m.Height, width = m.Width;
            int round = 5;
            Rect r = new Rect(oriX + j * (height + width), oriY + i * (height + width), height, width + 2 * height);
            dc.DrawRoundedRectangle(b, null, r, round, round);
            if (cLine[i, j].status == 2)
            {
                int standX = oriX + height / 2 + j * (height + width);
                int standY = oriY + height +width/ 2 + i * (height + width);
                int radius = height / 2;

                StreamGeometry g = GetHex(standX, standY, radius);

                dc.DrawGeometry(Brushes.Gray, null, g);
            }
            dc.Close();
            visual.type = "cLine";
            visual.x = i;
            visual.y = j;
            this.AddVisual(visual);
        }

        public void DrawPoint(ref map m, int i, int j, Brush b)
        {
            VisualWitness visual = new VisualWitness();
            DrawingContext dc = visual.RenderOpen();
            int height = m.Height, width = m.Width;
            int oriX = m.X, oriY = m.Y;
            int x = oriX + j * (height + width), y = oriY + i * (height + width);
            if (p[i, j].status <= 1)
            {
                Rect r = new Rect(x, y, height, height);
                dc.DrawRoundedRectangle(b, null, r, 5, 5);
            }
            if (p[i, j].status == 1) dc.DrawGeometry(Brushes.Gray, null, GetHex(x + height / 2, y + height / 2, height / 2));
            if (p[i, j].status == 2) dc.DrawEllipse(b, null, new Point(x + height / 2, y + height / 2), height, height);
            if(p[i,j].status==3)
            {
                Rect rec = new Rect(x, y, height, height);
                if(i==0) rec = new Rect(x, y-height, height, height*2);
                if (i == m.row) rec = new Rect(x, y, height, height * 2);
                if (j == 0) rec = new Rect(x-height, y, height*2, height );
                if (j == m.column) rec = new Rect(x, y, height * 2, height);
                dc.DrawRoundedRectangle(b, null, rec, 5, 5);
            }
            dc.Close();
            visual.type = "Point";
            visual.x = i;
            visual.y = j;
            this.AddVisual(visual);
        }

        public StreamGeometry GetHex(int i,int j,int radius)
        {
            int standX = i;
            int standY = j;

            StreamGeometry g = new StreamGeometry();
            g.FillRule = FillRule.EvenOdd;

            StreamGeometryContext ctx = g.Open();

            ctx.BeginFigure(new Point(standX - radius, standY), true, true);
            ctx.LineTo(new Point(standX - radius / 2, standY - radius * 1.723 / 2), true, false);
            ctx.LineTo(new Point(standX + radius / 2, standY - radius * 1.723 / 2), true, false);
            ctx.LineTo(new Point(standX + radius, standY), true, false);
            ctx.LineTo(new Point(standX + radius / 2, standY + radius * 1.723 / 2), true, false);
            ctx.LineTo(new Point(standX - radius / 2, standY + radius * 1.723 / 2), true, false);

            ctx.Close();

            return g;
        }

        public CombinedGeometry GetOct(int i, int j, int radius)
        {
            int standX = i;
            int standY = j;

            StreamGeometry g = new StreamGeometry();
            g.FillRule = FillRule.EvenOdd;

            StreamGeometryContext ctx = g.Open();
            ctx.BeginFigure(new Point(standX - radius, standY), true, true);
            ctx.LineTo(new Point(standX, standY - radius), true, false);
            ctx.LineTo(new Point(standX + radius, standY), true, false);
            ctx.LineTo(new Point(standX, standY + radius), true, false);
            ctx.Close();

            StreamGeometry h = new StreamGeometry();
            h.FillRule = FillRule.EvenOdd;

            ctx = h.Open();
            ctx.BeginFigure(new Point(standX - radius / 1.414, standY - radius / 1.414), true, true);
            ctx.LineTo(new Point(standX + radius / 1.414, standY - radius / 1.414), true, false);
            ctx.LineTo(new Point(standX + radius / 1.414, standY + radius / 1.414), true, false);
            ctx.LineTo(new Point(standX - radius / 1.414, standY + radius / 1.414), true, false);
            ctx.Close();

            CombinedGeometry cg = new CombinedGeometry(g, h);

            return cg;
        }

        public StreamGeometry GetSqu(int i, int j, int radius)
        {
            int standX = i;
            int standY = j;

            StreamGeometry h = new StreamGeometry();
            h.FillRule = FillRule.EvenOdd;
            StreamGeometryContext ctx;
            ctx = h.Open();
            ctx.BeginFigure(new Point(standX - radius / 1.414, standY - radius / 1.414), true, true);
            ctx.LineTo(new Point(standX + radius / 1.414, standY - radius / 1.414), true, false);
            ctx.LineTo(new Point(standX + radius / 1.414, standY + radius / 1.414), true, false);
            ctx.LineTo(new Point(standX - radius / 1.414, standY + radius / 1.414), true, false);
            ctx.Close();

            return h;
        }


        public Brush GetBrushes(int index)
        {
            switch (index)
            {
                case 0: return Brushes.Black; 
                case 1: return Brushes.White;
                case 2: return Brushes.Red;
                case 3: return Brushes.Green;
                case 4: return Brushes.Purple;
                case 5: return Brushes.Blue;
                default: return Brushes.RosyBrown;
            }
        }

        public void DrawSquare(ref map m, int i, int j, Brush b)
        {
            VisualWitness visual = new VisualWitness();
            DrawingContext dc = visual.RenderOpen();
            int height = m.Height, width = m.Width;
            int oriX = m.X + height, oriY = m.Y + height;
            Rect r = new Rect(oriX + j * (height + width), oriY + i * (height + width), width, width);
            Pen p=null;
            if (i == onClick_x && j==onClick_y) p = new Pen(Brushes.Red, 2);
            dc.DrawRectangle(b, p, r);
            if (s[i, j].oct != -1) dc.DrawGeometry(GetBrushes(s[i,j].oct), null, GetOct(oriX + j * (height + width)+width/2, oriY + i * (height + width) + width / 2, width / 4));
            if (s[i, j].squ != -1) dc.DrawGeometry(GetBrushes(s[i, j].squ), null, GetSqu(oriX + j * (height + width) + width / 2, oriY + i * (height + width) + width / 2, width / 4));
            dc.Close();
            visual.type = "Square";
            visual.x = i;
            visual.y = j;
            this.AddVisual(visual);
        }
    }
    public class point
    {
        public int status;
        public int x;
        public int y;

        public point(int x,int y)
        {
            status = 0;
            this.x = x;
            this.y = y;
        }

    }

    public class square
    {
        public int oct;
        public int squ ;
        public int x;
        public int y;

        public square() {
            oct = -1;
            squ = -1;
        }

        public square(int i,int j)
        {
            x = i;
            y = j;
            oct = -1;
            squ = -1;
        }

    }

    public class line
    {
        private int _x;
        private int _y;
        public int status = 0;
        //0:普通，1:不可通过，2:必须通过
        public line()
        {
            _x = 0;
            _y = 0;
            status = 0;
        }

        public line(int x, int y)
        {
            _x = x;
            _y = y;
            status = 0;
        }
    }

    public class map
    {
        private int _row;
        private int _column;
        private int _X;
        private int _Y;
        private int _Height;
        private int _Width;
        public List<point> Start_Point;
        public List<point> End_Point;
        private int mirror;
        public line[,] rLine;
        public line[,] cLine;
        public point[,] p;
        public square[,] s;
        public int[,] visitedcLine;
        public int[,] visitedrLine;
        public int[,] visitedPoint;
        public int[,] visitedSquareOct;
        public int[,] visitedSquareSqu;
        public int[,] visitedSquare;

        public map(int row,int column)
        {
            _column = column;
            rLine = new line[11, 10];
            cLine = new line[10, 11];
            p = new point[11, 11];
            s = new square[10, 10];
            _row = row;
            Start_Point = new List<point>();
            End_Point = new List<point>();
        }

        public int row
        {
            get
            {
                return _row;
            }
        }
        public int column
        {
            get
            {
                return _column;
            }
        }

        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        protected int dfs(int x,int y,point ep)
        {
            int flag = 0;
            if (x == ep.x&&y==ep.y&&test()==true) return 1;
            else
            {
                if (x > 0 && this.cLine[x - 1, y].status != 1 && this.visitedcLine[x - 1, y] == 0 && this.visitedPoint[x - 1, y] == 0)
                {
                    visitedcLine[x - 1, y] = 1;
                    visitedPoint[x - 1, y] = 1;
                    flag = dfs(x - 1, y,ep);
                    if (flag == 1) return 1;
                    visitedcLine[x - 1, y] = 0;
                    visitedPoint[x - 1, y] = 0;

                }
                if (x < row && this.cLine[x, y].status != 1 && this.visitedcLine[x, y] == 0 && this.visitedPoint[x + 1, y] == 0)
                {
                    visitedcLine[x, y] = 1;
                    visitedPoint[x + 1, y] = 1;
                    flag = dfs(x + 1, y,ep);
                    if (flag == 1) return 1;
                    visitedcLine[x, y] = 0;
                    visitedPoint[x + 1, y] = 0;

                }
                if (y > 0 && this.rLine[x, y - 1].status != 1 && this.visitedrLine[x, y - 1] == 0 && this.visitedPoint[x, y - 1] == 0)
                {
                    visitedrLine[x, y - 1] = 1;
                    visitedPoint[x, y - 1] = 1;
                    flag = dfs(x, y - 1,ep);
                    if (flag == 1) return 1;
                    visitedrLine[x, y - 1] = 0;
                    visitedPoint[x, y - 1] = 0;

                }
                if (y < column && this.rLine[x, y].status != 1 && this.visitedrLine[x, y] == 0 && this.visitedPoint[x, y + 1] == 0)
                {
                    visitedrLine[x, y] = 1;
                    visitedPoint[x, y + 1] = 1;
                    flag = dfs(x, y + 1,ep);
                    if (flag == 1) return 1;
                    visitedrLine[x, y] = 0;
                    visitedPoint[x, y + 1] = 0;
                }
            }
            return 0;
        }

        public void solve()
        {
            for (int i = 0; i < Start_Point.Count; i++)
            {
                for (int j = 0; j < End_Point.Count; j++)
                {
                    visitedcLine = new int[row, column + 1];
                    visitedrLine = new int[row + 1, column];
                    visitedPoint = new int[row + 1, column + 1];
                    point sp = Start_Point[i],ep=End_Point[j];
                    visitedPoint[sp.x, sp.y] = 1;
                    dfs(sp.x, sp.y,ep);
                }
            }
        }
        
        public bool testrLine(int x,int y)
        {
            if (rLine[x, y].status == 2)
            {
                if (visitedrLine[x, y] == 1) return true;
                else return false;
            }
            return true;
        }
        public bool testcLine(int x, int y)
        {
            if (cLine[x, y].status == 2)
            {
                if (visitedcLine[x, y] == 1) return true;
                else return false;
            }
            return true;
        }
        public bool testPoint(int x,int y)
        {
            if (p[x, y].status == 1)
            {
                if (visitedPoint[x,y] != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool testALLrLine()
        {
            for(int i = 0; i <= row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if (testrLine(i, j) == false) return false;
                }
            }
            return true;
        }
        public bool testALLcLine()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j <= column; j++)
                {
                    if (testcLine(i, j) == false) return false;
                }
            }
            return true;
        }
        public bool testAllPoint()
        {
            for(int i = 0; i <= row; i++)
            {
                for(int j = 0; j <= column; j++)
                {
                    if (testPoint(i, j) == false) return false;
                }
            }
            return true;
        }

        public void Seperate()
        {
            visitedSquare = new int[10, 10];
            int u = 1;
            for (int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if (visitedSquare[i, j] == 0)
                    {
                        sep(i,j,u++);
                    }
                }
            }
        }

        public void sep(int x,int y,int flag)
        {
            visitedSquare[x, y] = flag;
            if (visitedcLine[x, y + 1] == 0 && y < column - 1) sep(x, y + 1, flag);
            if (visitedrLine[x + 1, y] == 0 && x < row - 1) sep(x+1 , y, flag);
            if (visitedcLine[x, y] == 0 && y >=  1&&visitedSquare[x,y-1]==0) sep(x , y-1, flag);
            if (visitedrLine[x, y] == 0 && x>= 1 && visitedSquare[x-1, y ] == 0) sep(x-1, y, flag);
        }

        public bool testSquareOct()
        {
            int[,] oct= new int[10, 10];
            int[,] squ = new int[10, 10];
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    if (s[i, j].squ != -1) squ[visitedSquare[i, j], s[i, j].squ]++;
                    if (s[i, j].oct != -1) oct[visitedSquare[i, j], s[i, j].oct]++;
                }
            }
            for(int i = 0; i < 10; i++)
            {
                int flag = 0;
                for(int j = 0; j < 10; j++)
                {
                    if (oct[i, j] != 0&&oct[i,j]!=2) return false;
                    if (squ[i, j] != 0) flag++;
                    if (flag > 1) return false;
                }
            }
            //for(int j = 0; j < 4; j++)
            //{
            //    int flag = 0;
            //    for(int i = 0; i < 10; i++)
            //    {
            //        if (squ[i, j] != 0) flag++;
            //        if (flag > 1) return false;
            //    }
            //}
            return true;
        }

        public bool test()
        {
            Seperate();
            if (testALLcLine() == false) return false;
            if (testALLrLine() == false) return false;
            if (testAllPoint() == false) return false;
            if (testSquareOct() == false) return false;
            return true;
        }
    }  

}
