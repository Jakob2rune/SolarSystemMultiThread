
using SolarSystemMultiThread.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SolarSystemMultiThread.ViewModel
{
    /// <summary>
    /// A class to manage the collection of planets and their positions on the canvas.
    /// </summary>
    public class CanvasViewModel : Bindable
    {
        public ObservableCollection<OrbitalBody> _orbitalBodies = new ObservableCollection<OrbitalBody>();
        public ObservableCollection<Planet> Planets { get; set; } = new ObservableCollection<Planet>();
        public ObservableCollection<Moon> Moons { get; set; } = new ObservableCollection<Moon>();

        public ObservableCollection<OrbitalBody> OrbitalBodies
        {
            get { return _orbitalBodies; }
            set
            {
                _orbitalBodies = value;
                propertyIsChanged();
            }
        }

        // Constructor
        public CanvasViewModel()
        {

            InitializePlanets();
            InitializeMoons();
            InitializeSaturnRings();
            //SetupTimer();
            SetupMultiThreadTimer();


        }





        /// <summary>
        /// Method to initialize the collection of planets with their properties.
        /// </summary>
        private void InitializePlanets()
        {
            var newPlanets = new ObservableCollection<Planet>()
            {
              // Sun, Index 0
              new Planet(80, 0.000, System.Windows.Media.Brushes.Yellow, 0, 0),

              // Mercury, index 1
              new Planet(12, 0.05, System.Windows.Media.Brushes.Gray, 50, 0),
       
              // Venus, index 2
              new Planet(18, 0.04, System.Windows.Media.Brushes.Orange, 90, 40),
        
              // Earth, index 3
              new Planet(20, 0.03, System.Windows.Media.Brushes.Blue, 130, 80),

              // Mars, index 4
              new Planet(16, 0.025, System.Windows.Media.Brushes.Red, 170, 120),

              // Jupiter, index 5   
              new Planet(45, 0.015, System.Windows.Media.Brushes.Brown, 210, 160),
        
              // Rejsen Til Saturn
              new Planet(40, 0.012, System.Windows.Media.Brushes.Goldenrod, 250, 200),
        
              // Uranus, index 7
              new Planet(30, 0.009, System.Windows.Media.Brushes.LightBlue, 300, 240),
        
              // Neptune, index 8
              new Planet(30, 0.007, System.Windows.Media.Brushes.DarkBlue, 360, 280)


            };

            //add to planets collection
            foreach (var planet in newPlanets)
            {
                Planets.Add(planet);
            }

            // Add to OrbitalBodies collection 
            foreach (var planet in Planets)
            {
                OrbitalBodies.Add(planet);
            }
        }
        /// <summary>
        /// Initialize the moons and link them to their parent planets.
        /// </summary>
        private void InitializeMoons()
        {
            var newMoons = new ObservableCollection<Moon>()
            {
            //Earth Moon
            new Moon(6, 0.1, System.Windows.Media.Brushes.LightGray, 25, 0, (Planet)OrbitalBodies[3]),
            // Jupiter Moons
            new Moon(4, 0.08, System.Windows.Media.Brushes.LightGray, 30, 0, (Planet)OrbitalBodies[5]),
            new Moon(5, 0.06, System.Windows.Media.Brushes.LightGray, 35, 180, (Planet)OrbitalBodies[5])


            };

            // Add them to the Moons collection
            foreach (var moon in newMoons)
            {
                Moons.Add(moon);
            }

            // Add to OrbitalBodies collection
            foreach (var moon in Moons)
            {
                OrbitalBodies.Add(moon);
            }
        }

        /// <summary>
        /// Intializes a collection of small rocks to simulate Saturn's rings.
        /// </summary>
        private void InitializeSaturnRings()
        {
            Random random = new Random();
            double min = 0.012;
            double max = 0.05;

            for (int i = 0; i < 500; i++) // Create 500 rocks around saturn
            {
                double randomNumber = min + (random.NextDouble() * (max - min));
                Moon rock = new Moon(2, randomNumber, System.Windows.Media.Brushes.LightGray, random.Next(30, 50), random.Next(0, 360), (Planet)OrbitalBodies[6]);

                OrbitalBodies.Add(rock);
            }

        }

        /// <summary>
        /// Initializes and starts a timer to periodically update the positions of orbital bodies.
        /// </summary>
        /// <remarks>The timer is configured to trigger every 20 milliseconds. For each orbital body,
        /// except the first one  (assumed to represent the sun), the timer's tick event is associated with the body's
        /// <see cref="Move"/> method. This ensures that the positions of the orbital bodies are updated at regular
        /// intervals.</remarks>
        private void SetupTimer()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);

            // Add Move event handler for each body except the sun
            foreach (var body in OrbitalBodies)
            {
                // Skip the sun, index 0e
                if (body != OrbitalBodies[0])
                {
                    timer.Tick += body.Move;
                }
            }
            timer.Start();
        }
        private void SetupMultiThreadTimer()
        {

        }
        
    }

}
