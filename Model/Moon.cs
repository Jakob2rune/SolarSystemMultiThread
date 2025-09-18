using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SolarSystemMultiThread.Model
{
    /// <summary>
    /// Represents a moon that orbits a parent planet, with properties for its orbital characteristics,  position, and
    /// appearance.
    /// </summary>
    /// <remarks>The <see cref="Moon"/> class models a celestial body that orbits a <see cref="Planet"/>.  It
    /// provides properties to define its orbital path, speed, and appearance, as well as methods  to update its
    /// position based on its orbital motion. The moon's position is calculated relative  to its parent planet, and its
    /// movement is determined by its orbital speed and angle.</remarks>
    public class Moon : OrbitalBody
    {
        public override Ellipse? Ellipse { get; set; }
        public double Speed { get; set; }
        public double Angle { get; set; }
        private double _orbitRadius;

        
        public Planet ParentPlanet { get; set; }
        override public int Diameter
        {
            get
            {
                if (Ellipse.Dispatcher.CheckAccess())
                    return (int)Ellipse.Width;
                else
                    return (int)Ellipse.Dispatcher.Invoke(() => Ellipse.Width);
            }
            set
            {
                if (Ellipse.Dispatcher.CheckAccess())
                {
                    Ellipse.Width = value;
                    Ellipse.Height = value;
                }
                else
                {
                    Ellipse.Dispatcher.Invoke(() => {
                        Ellipse.Width = value;
                        Ellipse.Height = value;
                    });
                }
            }
        }
        override public SolidColorBrush Color
        {
            get { return (SolidColorBrush)Ellipse.Fill; }
            set { Ellipse.Fill = value; }
        }
        private double _xPos;
        override public double XPos
        {
            get { return _xPos; }
            set {
                _xPos = value;

                //Application.Current.Dispatcher.Invoke(() => propertyIsChanged());
            }
        }

        private double _yPos;
        override public double YPos
        {
            get { return _yPos; }
            set {
                _yPos = value;
                //Application.Current.Dispatcher.Invoke(() => propertyIsChanged());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Moon"/> class, representing a moon orbiting a planet.
        /// </summary>
        /// <remarks>The moon's position is calculated relative to its parent planet based on the
        /// specified orbit radius and initial angle. The <paramref name="speed"/> parameter determines how quickly the
        /// moon moves along its orbit.</remarks>
        /// <param name="diameter">The diameter of the moon, in pixels, used to define its visual size.</param>
        /// <param name="speed">The speed at which the moon orbits its parent planet. Higher values indicate faster movement.</param>
        /// <param name="color">The color of the moon, represented as a <see cref="SolidColorBrush"/>.</param>
        /// <param name="orbitRadius">The radius of the moon's orbit around its parent planet, in pixels.</param>
        /// <param name="initialAngle">The initial angle of the moon's position in its orbit, in radians.</param>
        /// <param name="parentPlanet">The planet that the moon orbits. This must not be <see langword="null"/>.</param>
        public Moon(int diameter, double speed, SolidColorBrush color, int orbitRadius, double initialAngle, Planet parentPlanet)
        {
            Ellipse = new Ellipse()
            {
                Width = diameter,
                Height = diameter,
                Fill = color
            };
            Speed = speed;
            _orbitRadius = orbitRadius;
            Angle = initialAngle;
            ParentPlanet = parentPlanet;

            XPos = parentPlanet.XPos + _orbitRadius * Math.Cos(Angle);
            YPos = parentPlanet.YPos + _orbitRadius * Math.Sin(Angle);
        }

        /// <summary>
        /// Updates the position of the object based on its orbital motion around the parent planet.
        /// delegate signature must match.
        /// </summary>
        /// <remarks>The position is calculated relative to the center of the parent planet, taking into
        /// account the object's orbital radius and current angle. If the <see cref="ParentPlanet"/> is null, the method
        /// does nothing.</remarks>
        /// <param name="sender">The source of the event that triggered the movement.</param>
        /// <param name="e">The event data associated with the movement.</param>
        public override void Move(Object sender, EventArgs e)
        {
            if (ParentPlanet == null)
                return;

            // New angle based on speed
            Angle += Speed;

            // Calc new position relative to the parent planet
            double centerX = ParentPlanet.XPos + ParentPlanet.Diameter / 2;
            double centerY = ParentPlanet.YPos + ParentPlanet.Diameter / 2;

            double x = centerX + _orbitRadius * Math.Cos(Angle);
            double y = centerY + _orbitRadius * Math.Sin(Angle);

            XPos = x;
            YPos = y;
        }
    }
}
