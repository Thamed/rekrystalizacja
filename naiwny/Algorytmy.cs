using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace naiwny
{
    class Algorytmy
    {
        private Point[,] map, newmap;
        private Point[,] tp, dys;
        private int width, height;
        private int firstGeneration;
        private int neigh = 0;
        bool choice = false;
        bool periodic = true;
        int seedRule = 0;
        public double step = 0.001;
        public double wartKryt;
        public double[] ro;
        public int z = 2000; //ilość iteracji
        public int counter;
        Random rnd = new Random();

        private void updateMap(Point[,]arr, Point[,] tmp)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    arr[i,j].State = tmp[i,j].State;
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
            map[y + 1,x + 1].State=true;
            Random rng = new Random();
            map[y + 1,x + 1].Color=Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
        }
        public void start()
        {
            if (seedRule == 0)
            {
                Random rng = new Random();
                for (int i = 0; i < firstGeneration; i++)
                {
                    int x = rng.Next(width - 2);
                    int y = rng.Next(height - 2);
                    map[x + 1,y + 1].State = true;
                    newmap[x + 1,y + 1].State = true;
                    map[x + 1,y + 1].Color = Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
                    newmap[x + 1,y + 1].Color = map[x + 1,y + 1].Color;
                }
            }
            else if (seedRule == 1)
            {
                Random rng = new Random();
                double formula = Math.Sqrt(width * height / firstGeneration);
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
                        map[x, y].Color  = Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255));
                        newmap[x, y].Color = map[x, y].Color;
                    }
                }
            }
        }

        private void moore(int i, int j, Point[,] arr, Point[,]tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j - 1].State)
                {
                    tmp[i - 1,j - 1].Color=arr[i,j].Color;
                    tmp[i - 1,j - 1].State=true;
                }
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i - 1,j + 1].State)
                {
                    tmp[i - 1,j + 1].Color=arr[i,j].Color;
                    tmp[i - 1,j + 1].State=true;
                }
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j - 1].State)
                {
                    tmp[i + 1,j - 1].Color=arr[i,j].Color;
                    tmp[i + 1,j - 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
                if (!arr[i + 1,j + 1].State)
                {
                    tmp[i + 1,j + 1].Color=arr[i,j].Color;
                    tmp[i + 1,j + 1].State=true;
                }
            }
        }

        private void neumann(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
            }
        }

        private void hexaLeft(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j - 1].State)
                {
                    tmp[i - 1,j - 1].Color=arr[i,j].Color;
                    tmp[i - 1,j - 1].State=true;
                }
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
                if (!arr[i + 1,j + 1].State)
                {
                    tmp[i + 1,j + 1].Color=arr[i,j].Color;
                    tmp[i + 1,j + 1].State=true;
                }
            }
        }

        private void hexaRight(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i - 1,j + 1].State)
                {
                    tmp[i - 1,j + 1].Color=arr[i,j].Color;
                    tmp[i - 1,j + 1].State=true;
                }
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j - 1].State)
                {
                    tmp[i + 1,j - 1].Color=arr[i,j].Color;
                    tmp[i + 1,j - 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
            }
        }

        private void pentaLeft(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j - 1].State)
                {
                    tmp[i - 1,j - 1].Color=arr[i,j].Color;
                    tmp[i - 1,j - 1].State=true;
                }
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1, j].State = true;
                }

                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }

                if (!arr[i + 1,j - 1].State)
                {
                    tmp[i + 1,j - 1].Color=arr[i,j].Color;
                    tmp[i + 1,j - 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
            }
        }

        private void pentaRight(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {

                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i - 1,j + 1].State)
                {
                    tmp[i - 1,j + 1].Color=arr[i,j].Color;
                    tmp[i - 1, j + 1].State = true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
                if (!arr[i + 1,j + 1].State)
                {
                    tmp[i + 1,j + 1].Color=arr[i,j].Color;
                    tmp[i + 1,j + 1].State=true;
                }
            }
        }
        private void pentaTop(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            { 
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
                if (!arr[i + 1,j - 1].State)
                {
                    tmp[i + 1,j - 1].Color=arr[i,j].Color;
                    tmp[i + 1,j - 1].State=true;
                }
                if (!arr[i + 1,j].State)
                {
                    tmp[i + 1,j].Color=arr[i,j].Color;
                    tmp[i + 1,j].State=true;
                }
                if (!arr[i + 1,j + 1].State)
                {
                    tmp[i + 1,j + 1].Color=arr[i,j].Color;
                    tmp[i + 1,j + 1].State=true;
                }
            }
        }

        private void pentaBottom(int i, int j, Point[,] arr, Point[,] tmp)
        {
            if (arr[i,j].State)
            {
                if (!arr[i - 1,j - 1].State)
                {
                    tmp[i - 1,j - 1].Color=arr[i,j].Color;
                    tmp[i - 1,j - 1].State=true;
                }
                if (!arr[i - 1,j].State)
                {
                    tmp[i - 1,j].Color=arr[i,j].Color;
                    tmp[i - 1,j].State=true;
                }
                if (!arr[i - 1,j + 1].State)
                {
                    tmp[i - 1,j + 1].Color=arr[i,j].Color;
                    tmp[i - 1,j + 1].State=true;
                }
                if (!arr[i,j - 1].State)
                {
                    tmp[i,j - 1].Color=arr[i,j].Color;
                    tmp[i,j - 1].State=true;
                }
                if (!arr[i,j + 1].State)
                {
                    tmp[i,j + 1].Color=arr[i,j].Color;
                    tmp[i,j + 1].State=true;
                }
            }
        }

        private void makePeriodic(Point[,] arr)
        {
            for (int i = 1; i < (height - 1); i++)
            {
                arr[0,i] = arr[width - 2,i];
                arr[width - 1,i] = arr[1,i];

            }
            for (int i = 1; i < (width - 1); i++)
            {
                arr[i,0] = arr[i, height - 2];
                arr[i, height - 1] = arr[i,1];
            }
        }
        public void nextStep(Point[,]arr, Point[,] tmp)
        {

            if (periodic)
                makePeriodic(arr);
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (neigh == 0) moore(i, j,arr, tmp);
                    else if (neigh == 1) neumann(i, j, arr, tmp);
                    else if (neigh == 2) hexaLeft(i, j, arr, tmp);
                    else if (neigh == 3) hexaRight(i, j, arr, tmp);
                    else if (neigh == 4)
                    {
                        Random rng = new Random();
                        int temp = rng.Next(0, 2);
                        if (temp==1) hexaLeft(i, j, arr, tmp);
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
            int a = rng.Next(0,3);
            return a;
        }

        public void setNeigh(int neigh)
        {
            this.neigh = neigh;

            if (neigh == 5 || neigh == 4)
            {
                Random rng = new Random();
                choice = Convert.ToBoolean(rng.Next(0, 1));
            }
        }

        public void setPeriodic(bool periodic)
        {
            this.periodic = periodic;
        }

        public bool isPeriodic()
        {
            return periodic;
        }

        public void setSeedRule(int seedRule)
        {
            this.seedRule = seedRule;
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
            wartKryt = 76842668.25*10;
        }

        public void setPos()
        {
            counter=0;
            
            for (int i =1; i<width; i++)
            {
                for (int j = 1; j < height; j++)
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
