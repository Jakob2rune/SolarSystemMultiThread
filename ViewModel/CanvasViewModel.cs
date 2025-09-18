
using SolarSystemMultiThread.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            SetupTimer();
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
        /// A <see cref="CancellationTokenSource"/> used to signal cancellation of the game loop.
        /// </summary>
        /// <remarks>This field is intended to manage the lifecycle of the game loop's cancellation token.
        /// Ensure proper disposal of this object to avoid resource leaks.</remarks>
        private CancellationTokenSource _gameLoopCts;

        /// <summary>
        /// Initializes and starts the game loop timer, which updates the positions of orbital bodies and refreshes the
        /// UI at approximately 60 frames per second.
        /// </summary>
        /// <remarks>This method runs an asynchronous game loop that processes the movement of orbital
        /// bodies in parallel and updates their positions on the UI thread. The loop continues until it is explicitly
        /// canceled via the associated <see cref="CancellationTokenSource"/>.</remarks>
        private async void SetupTimer()
        {
            // Initialize the cancellation token source
            _gameLoopCts = new CancellationTokenSource();

            try
            {
                // Main game loop
               
                while (!_gameLoopCts.Token.IsCancellationRequested)
                {
                    var startTime = DateTime.Now;

                    // Process all bodies in parallel
                    // Skip the first body (the sun) as it does not move
                    // Use Task.Run to offload the work to background threads
                    // Collect tasks to await them later
                    // Using Select to create a task for each body's Move method
                    var moveTasks = OrbitalBodies.Skip(1)
                        .Select(body => Task.Run(() => body.Move(this, EventArgs.Empty)))
                        .ToArray();

                    // Wait for all movements to complete
                    await Task.WhenAll(moveTasks);

                    // Update UI on main thread
                    // Use Dispatcher to ensure UI updates are thread-safe
                    // Use InvokeAsync to avoid blocking the game loop
                    // Notify property changes for XPos and YPos of all bodies
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        foreach (var body in OrbitalBodies)
                        {
                            body.propertyIsChanged(nameof(body.XPos));
                            body.propertyIsChanged(nameof(body.YPos));
                        }
                    });

                    // Maintain ~60 FPS
                    // Calculate elapsed time and delay if necessary
                    // Use Math.Max to ensure non-negative delay
                    var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                    var delay = Math.Max(0, 16 - elapsed); // 16ms for 60 FPS
                    // Use Task.Delay with cancellation token to allow graceful exit
                    // This ensures the loop can be cancelled promptly
                    // Use (int) cast as Task.Delay requires an integer
                    await Task.Delay((int)delay, _gameLoopCts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                // Game loop was cancelled, normal exit
            }
        }
    }
}
