using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace naiwny
{
    public partial class Form1 : Form
    {
        public int X;
        public int Y;
        bool nope = true;
        private static int logicNeighbourhood = 0;
        private static bool logicPeriodic = false;
        private static int numberOfFirstSeeds = 0;
        private static int seedRule = 0;
        private static bool progState;
        public int aa = 0;
        public int click = 0;
        int all;
        Bitmap n;
        Random rnd = new Random();
        /* POLA*/
        private Point[,] map, newmap;
        private Point[,] tp, dys;
        private int width, height;
        private int neigh = 0;
        bool choice = false;
        bool periodic = true;
        private double step = 0.001;
        private double wartKryt;
        private double[] ro;
        private int z = 2000; //ilość iteracji
        private int counter;
        /**/
        public Form1()
        {
            InitializeComponent();
            button2.BackColor = Color.FromArgb(46, 204, 112);
            textBox1.Text = "300";
            textBox2.Text = "300";
            textBox3.Text = "50";
            comboBox1.Items.AddRange(new object[] {"von Neumann'a",
                        "Moore'a",
                        "Heksagonal left",
                        "Hexagonal rigth",
                        "Hexagonal random",
                        "Pentagonal random"});
            comboBox2.Items.AddRange(new object[] {"Losowe",
                        "Równomierne",
                        "Losowe z promieniem R",
                        "Przez kliknięcie"});
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            //pcb.Image = new Bitmap(400, 400);
            progState = false;
            textBox4.Text = "0,001";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            X = Convert.ToInt32(textBox2.Text);
            Y = Convert.ToInt32(textBox1.Text);
            if (X > 670 || Y > 410 || X < 0 || Y < 0)
            {
                MessageBox.Show("Wymiary są za duze", "Uwaga!", MessageBoxButtons.OK);
            }
            else {
                pcb.BorderStyle = BorderStyle.FixedSingle;
                pcb.Height = Y;
                pcb.Width = X;
            }
            numberOfFirstSeeds = Convert.ToInt32(textBox3.Text);
            all = X * Y;
            n = new Bitmap(X, Y);

            width = (X + 2);
            height = (Y + 2);
            map = new Point[width, height];
            newmap = new Point[width, height];
            dys = new Point[width, height];
            tp = new Point[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height ; j++)
                {
                    map[i, j] = new Point();
                    newmap[i, j] = new Point();
                    dys[i, j] = new Point();
                    tp[i, j] = new Point();
                }
            }

        }
        private void pcb_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            newSeed(y, x);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "von Neumann'a")
                logicNeighbourhood = 1;
            else if (comboBox1.SelectedItem.ToString() == "Moore'a")
                logicNeighbourhood = 0;
            else if (comboBox1.SelectedItem.ToString() == "Heksagonal left")
                logicNeighbourhood = 2;
            else if (comboBox1.SelectedItem.ToString() == "Hexagonal rigth")
                logicNeighbourhood = 3;
            else if (comboBox1.SelectedItem.ToString() == "Hexagonal random")
                logicNeighbourhood = 4;
            else if (comboBox1.SelectedItem.ToString() == "Pentagonal random")
                logicNeighbourhood = 5;

            if (checkBox1.Checked)
                logicPeriodic = true;
            else
                logicPeriodic = false;

            if (comboBox2.SelectedItem.ToString() == "Losowe")
                seedRule = 0;
            else if (comboBox2.SelectedItem.ToString() == "Równomierne")
                seedRule = 1;
            else if (comboBox2.SelectedItem.ToString() == "Losowe z promieniem R")
                seedRule = 2;
            else if (comboBox2.SelectedItem.ToString() == "Przez kliknięcie")
                seedRule = 3;

            numberOfFirstSeeds = Convert.ToInt32(textBox3.Text);

            Task.Run(()=> {
                start();
                while (nope)
                {
                    if (progState)
                    {
                        calculateDensity();
                        if (aa == 0)
                            setPos();
                        rekrystalizacja(aa);
                        nextStep(dys, tp);
                        aa++;
                        draw(tp);
                    }
                    else
                    {
                        draw(newmap);

                        if (seedRule == 3)
                        {
                            nextStep(map, newmap);
                        }
                        else
                        {

                            nextStep(map, newmap);
                        }
                        up();
                    }
                }
            });
        }

        void draw(Point[,] arr)
        {
            for (int i = 1; i < X; i++)
            {
                for (int j = 1; j < Y; j++)
                {
                    n.SetPixel(i, j, arr[i, j].Color);
                }
            }
            pcb.Image = n;
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            if (nope == false)
            {
                nope = true;
                button3.Text = "Stop";
            }
            else {
                nope = false;
                button3.Text = "Resume";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            nope = false;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = new Point();
                    newmap[i, j] = new Point();
                }
            }
            nope = true;
            if (comboBox1.SelectedItem.ToString() == "von Neumann'a")
                logicNeighbourhood = 1;
            else if (comboBox1.SelectedItem.ToString() == "Moore'a")
                logicNeighbourhood = 0;
            else if (comboBox1.SelectedItem.ToString() == "Heksagonal left")
                logicNeighbourhood = 2;
            else if (comboBox1.SelectedItem.ToString() == "Hexagonal rigth")
                logicNeighbourhood = 3;
            else if (comboBox1.SelectedItem.ToString() == "Hexagonal random")
                logicNeighbourhood = 4;
            else if (comboBox1.SelectedItem.ToString() == "Pentagonal random")
                logicNeighbourhood = 5;

            if (checkBox1.Checked)
                logicPeriodic = true;
            else
                logicPeriodic = false;

            if (comboBox2.SelectedItem.ToString() == "Losowe")
                seedRule = 0;
            else if (comboBox2.SelectedItem.ToString() == "Równomierne")
                seedRule = 1;
            else if (comboBox2.SelectedItem.ToString() == "Losowe z promieniem R")
                seedRule = 2;
            else if (comboBox2.SelectedItem.ToString() == "Przez kliknięcie")
                seedRule = 3;

            numberOfFirstSeeds = Convert.ToInt32(textBox3.Text);

            start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            step = Convert.ToDouble(textBox4.Text);
            if (click == 0)
            {
                progState = true;
                click++;
            }
            else
            {
                for (int i = 1; i < X; i++)
                    for (int j = 1; j < Y; j++)
                        map[i, j] = dys[i, j]; aa = 0;
                progState = false;
            }
        }

        public void rekrystalizacja(int a)
        {
            for (int i = 1; i < X; i++)
            {
                for (int j = 1; j < Y; j++)
                {
                    if (a == 0)
                    {
                        dys[i, j].Ro += (ro[a] / all) * dys[i, j].Pos;
                        dys[i, j].RoLeft = dys[i, j].RoLeft + (ro[a] / all) * (1 - dys[i, j].Pos);
                    }

                    else
                    {
                        dys[i, j].Ro += ((ro[a] - ro[a - 1]) / all) * dys[i, j].Pos;
                        dys[i, j].RoLeft = dys[i, j].RoLeft + ((ro[a] - ro[a - 1]) / all) * (1 - dys[i, j].Pos);
                    }

                    if (dys[i, j].Ro > wartKryt)
                    {
                        dys[i, j].State = true;
                        dys[i, j].Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    }
                }
            }
            reszta();
        }

        private void updateMap(Point[,] arr, Point[,] tmp)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    arr[i, j].State = tmp[i, j].State;
                    arr[i, j].Color = tmp[i, j].Color;
                }
            }
        }

        public void up()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    tp[i, j].Color = newmap[i, j].Color;
        }

        public void newSeed(int x, int y)
        {
            map[y + 1, x + 1].State = true;
            Random rng = new Random();
            map[y + 1, x + 1].Color = Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
        }
        public void start()
        {
            if (seedRule == 0)
            {
                Random rng = new Random();
                for (int i = 0; i < numberOfFirstSeeds; i++)
                {
                    int a = rng.Next(1,X);
                    int b = rng.Next(1,Y);
                    map[a + 1, b + 1].State = true;
                    newmap[a + 1, b + 1].State = true;
                    map[a + 1, b + 1].Color = Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
                    newmap[a + 1, b + 1].Color = map[a + 1, b + 1].Color;
                }
            }
            else if (seedRule == 1)
            {
                Random rng = new Random();
                double formula = Math.Sqrt(width * height / numberOfFirstSeeds);
                int iIterations = (int)(height / formula);
                int jIterations = (int)(width / formula);
                int spaceX = width / (jIterations + 1);
                int spaceY = height / (iIterations + 1);
                for (int i = 0; i < iIterations; i++)
                {
                    for (int j = 0; j < jIterations; j++)
                    {
                        int x = (int)(spaceX + j * spaceX);
                        int y = (int)(spaceY + i * spaceY);
                        map[x, y].State = true;
                        newmap[x, y].State = true;
                        map[x, y].Color = Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
                        newmap[x, y].Color = map[x, y].Color;
                    }
                }
            }
        }

        private void moore(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j - 1].State)
                {
                    tmp[i - 1, j - 1].Color = arr[i, j].Color;
                    tmp[i - 1, j - 1].State = true;
                }
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i - 1, j + 1].State)
                {
                    tmp[i - 1, j + 1].Color = arr[i, j].Color;
                    tmp[i - 1, j + 1].State = true;
                }
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j - 1].State)
                {
                    tmp[i + 1, j - 1].Color = arr[i, j].Color;
                    tmp[i + 1, j - 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
                if (!arr[i + 1, j + 1].State)
                {
                    tmp[i + 1, j + 1].Color = arr[i, j].Color;
                    tmp[i + 1, j + 1].State = true;
                }
            }
        }

        private void neumann(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
            }
        }

        private void hexaLeft(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j - 1].State)
                {
                    tmp[i - 1, j - 1].Color = arr[i, j].Color;
                    tmp[i - 1, j - 1].State = true;
                }
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
                if (!arr[i + 1, j + 1].State)
                {
                    tmp[i + 1, j + 1].Color = arr[i, j].Color;
                    tmp[i + 1, j + 1].State = true;
                }
            }
        }

        private void hexaRight(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i - 1, j + 1].State)
                {
                    tmp[i - 1, j + 1].Color = arr[i, j].Color;
                    tmp[i - 1, j + 1].State = true;
                }
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j - 1].State)
                {
                    tmp[i + 1, j - 1].Color = arr[i, j].Color;
                    tmp[i + 1, j - 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
            }
        }

        private void pentaLeft(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j - 1].State)
                {
                    tmp[i - 1, j - 1].Color = arr[i, j].Color;
                    tmp[i - 1, j - 1].State = true;
                }
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }

                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }

                if (!arr[i + 1, j - 1].State)
                {
                    tmp[i + 1, j - 1].Color = arr[i, j].Color;
                    tmp[i + 1, j - 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
            }
        }

        private void pentaRight(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {

                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i - 1, j + 1].State)
                {
                    tmp[i - 1, j + 1].Color = arr[i, j].Color;
                    tmp[i - 1, j + 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
                if (!arr[i + 1, j + 1].State)
                {
                    tmp[i + 1, j + 1].Color = arr[i, j].Color;
                    tmp[i + 1, j + 1].State = true;
                }
            }
        }
        private void pentaTop(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
                if (!arr[i + 1, j - 1].State)
                {
                    tmp[i + 1, j - 1].Color = arr[i, j].Color;
                    tmp[i + 1, j - 1].State = true;
                }
                if (!arr[i + 1, j].State)
                {
                    tmp[i + 1, j].Color = arr[i, j].Color;
                    tmp[i + 1, j].State = true;
                }
                if (!arr[i + 1, j + 1].State)
                {
                    tmp[i + 1, j + 1].Color = arr[i, j].Color;
                    tmp[i + 1, j + 1].State = true;
                }
            }
        }

        private void pentaBottom(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i, j].State)
            {
                if (!arr[i - 1, j - 1].State)
                {
                    tmp[i - 1, j - 1].Color = arr[i, j].Color;
                    tmp[i - 1, j - 1].State = true;
                }
                if (!arr[i - 1, j].State)
                {
                    tmp[i - 1, j].Color = arr[i, j].Color;
                    tmp[i - 1, j].State = true;
                }
                if (!arr[i - 1, j + 1].State)
                {
                    tmp[i - 1, j + 1].Color = arr[i, j].Color;
                    tmp[i - 1, j + 1].State = true;
                }
                if (!arr[i, j - 1].State)
                {
                    tmp[i, j - 1].Color = arr[i, j].Color;
                    tmp[i, j - 1].State = true;
                }
                if (!arr[i, j + 1].State)
                {
                    tmp[i, j + 1].Color = arr[i, j].Color;
                    tmp[i, j + 1].State = true;
                }
            }
        }

        private void makePeriodic(Point[,] arr)
        {
            for (int i = 1; i < (height - 1); i++)
            {
                arr[0, i] = arr[width - 2, i];
                arr[width - 1, i] = arr[1, i];

            }
            for (int i = 1; i < (width - 1); i++)
            {
                arr[i, 0] = arr[i, height - 2];
                arr[i, height - 1] = arr[i, 1];
            }
        }
        private void nextStep(Point[,] arr, Point[,] tmp)
        {

            if (periodic)
                makePeriodic(arr);
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (neigh == 0) moore(i, j, arr, tmp);
                    else if (neigh == 1) neumann(i, j, arr, tmp);
                    else if (neigh == 2) hexaLeft(i, j, arr, tmp);
                    else if (neigh == 3) hexaRight(i, j, arr, tmp);
                    else if (neigh == 4)
                    {
                        Random rng = new Random();
                        int temp = rng.Next(0, 2);
                        if (temp == 1) hexaLeft(i, j, arr, tmp);
                        else hexaRight(i, j, arr, tmp);
                    }
                    else if (neigh == 5)
                    {
                        int temp = randPenta();
                        if (temp == 0) pentaBottom(i, j, arr, tmp);
                        else if (temp == 1) pentaTop(i, j, arr, tmp);
                        else if (temp == 2) pentaLeft(i, j, arr, tmp);
                        else if (temp == 3) pentaRight(i, j, arr, tmp);
                    }
                }
            }
            updateMap(arr, tmp);
        }

        private int randPenta()
        {
            Random rng = new Random();
            int a = rng.Next(0, 3);
            return a;
        }

        public void calculateDensity()
        {
            double A = 86710969050178.5;
            double B = 9.41268203527779;
            double t = step;

            ro = new double[z];
            ro[0] = 1;
            for (int i = 1; i < z; i++)
            {
                ro[i] = A / B + (1 - (A / B)) * Math.Exp(-1 * B * t);
                t += step;
            }
            wartKryt = 76842668.25 * 10;
        }

        public void setPos()
        {
            counter = 0;

            for (int i = 1; i < X; i++)
            {
                for (int j = 1; j < Y; j++)
                {
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            if (i - m == i && j - n == j)
                                continue;
                            else if (map[i + m, j + n].Color == map[i, j].Color)
                                counter++;
                        }
                    }
                    if (counter == 8)
                        dys[i, j].Pos = 0.2;
                    else
                    {
                        dys[i, j].Pos = 0.8;
                    }
                    counter = 0;
                }
            }
        }

        public void reszta()
        {
            double h = 1;
            bool nice = true;
            int tr = 20;
            int q, w;
            for (int i = 1; i < width; i++)
                for (int j = 1; j < height; j++)
                    h = h + dys[i, j].RoLeft;
            while (nice)
            {
                q = rnd.Next(1, width);
                w = rnd.Next(1, height);
                if (dys[q, w].Pos == 0.8)
                {
                    dys[q, w].Ro = h / 20;
                    tr--;
                }
                if (tr == 0)
                    nice = false;
            }
        }

    }
}
