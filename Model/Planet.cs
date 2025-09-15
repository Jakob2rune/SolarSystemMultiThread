using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SolarSystemMultiThread.Model
{
    /// <summary>
    /// A class representing a planet in the solar system simulation.
    /// </summary>
    public class Planet : OrbitalBody
    {
        public override Ellipse? Ellipse { get; set; }
        public double Speed { get; set;  }

        public double Angle { get; set; }

        override public int Diameter 
        { 
            get { return (int)Ellipse.Width; } 
            set 
            { 
                Ellipse.Width = value; 
                Ellipse.Height = value; 
            }
        }

        public override SolidColorBrush Color
        {
            get { return (SolidColorBrush)Ellipse.Fill; }
            set { Ellipse.Fill = value; }
        }

        private double _xPos;
        override public double XPos
        {
            get { return _xPos; }
            set
            {
                _xPos = value;
                propertyIsChanged();
            }
        }

        private double _yPos;
        override public double YPos
        {
            get { return _yPos; }
            set
            {
                _yPos = value;
                propertyIsChanged();
            }
        }



        private int _newXPos;
        private int _newYPos;
        private double _orbitRadius;

        /// <summary>
        /// Constructor to initialize a planet with specified properties.
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="speed"></param>
        /// <param name="color"></param>
        /// <param name="orbitRadius"></param>
        /// <param name="initialAngle"></param>

        public Planet(int diameter, double speed, SolidColorBrush color, int orbitRadius, double initialAngle)
        {
            Ellipse = new Ellipse()
            {
                Width = diameter,
                Height = diameter,
                Fill = color
            };
            Speed = speed;
            _orbitRadius = orbitRadius;
            Angle = initialAngle; // Set the initial angle

            // Set initial position
            SetStartPosition();
        }

        /// <summary>
        /// Calculate the new position of the planet based on its angle and orbit radius.
        /// Point is a struct with X and Y properties.
        /// </summary>
        /// <returns></returns>
        private Point CalcNewPos()
        {
            
            // Trying to center the orbit in the sun
            double centerX = 400 - (Diameter / 2);
            double centerY = 400 - (Diameter / 2);

            return new Point(
                centerX + _orbitRadius * Math.Cos(Angle),
                centerY + _orbitRadius * Math.Sin(Angle)
            );
        }

        /// <summary>
        /// Updates the position of the object based on its current speed and angle.
        /// To be used as an event handler for a timer tick event, the delegate signature must match.
        /// </summary>
        /// <param name="sender">The source of the event that triggered the move operation.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        public override void Move(Object sender, EventArgs e) {
            Angle += Speed;
            Point newPos = CalcNewPos();

            XPos = newPos.X;
            YPos = newPos.Y;
        }


        /// <summary>
        /// Method to set the planet's position based on Spped and orbit radius.
        /// </summary>
        public void SetStartPosition()
        {
            if (Speed > 0) // Only update if the planet moves (not the sun)
            {
                Angle += Speed;
                if (Angle >= 360) Angle -= 360;

                // Center of canvas 
                XPos = 400 + _orbitRadius * Math.Cos(Angle * Math.PI / 180) - Diameter / 2;
                YPos = 400 + _orbitRadius * Math.Sin(Angle * Math.PI / 180) - Diameter / 2;
            }
            else
            {
                // Center the sun
                XPos = 400 - Diameter / 2;
                YPos = 400 - Diameter / 2;
            }
        }
      
    }
}
