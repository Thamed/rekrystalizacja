using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace naiwny
{
    class Point
    {
        private Color color;
        private bool state;
        private double pos;
        private bool rek;
        private double ro;
        private double roLeft;

        public Point()
        {
            this.Color = Color.Black;
            this.State = false;
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public bool State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public bool Rek
        {
            get
            {
                return rek;
            }

            set
            {
                rek = value;
            }
        }

        public double Ro
        {
            get
            {
                return ro;
            }

            set
            {
                ro = value;
            }
        }

        public double RoLeft
        {
            get
            {
                return roLeft;
            }

            set
            {
                roLeft = value;
            }
        }

        public double Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
            }
        }
    }
}
