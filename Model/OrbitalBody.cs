using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SolarSystemMultiThread.Model
{
    abstract public class OrbitalBody : Bindable
    {
        public abstract Ellipse Ellipse { get; set; }
        public abstract double XPos { get; set; }
        public abstract double YPos { get; set; }
        public abstract int Diameter { get; set; }
        public abstract SolidColorBrush Color {get; set;}
        public abstract void Move(object sender, EventArgs e);
    }
}
