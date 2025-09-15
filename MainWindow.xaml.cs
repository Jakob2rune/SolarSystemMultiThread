using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolarSystemMultiThread
{
  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel.CanvasViewModel ViewModel = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the main application window by initializing its components, 
        /// creating the starfield effect, and setting the data context 
        public MainWindow()
        {
            InitializeComponent();
            CreateStarfield();

            ViewModel = new ViewModel.CanvasViewModel();
            this.DataContext = ViewModel;
        }


        /// <summary>
        /// Generates a starfield by creating and adding a collection of randomly sized, positioned, and  styled stars
        /// to the canvas.
        /// </summary>

        private void CreateStarfield()
        {
            Random random = new Random();

            for (int i = 0; i < 150; i++) // Create 150 stars
            {
                Ellipse star = new Ellipse()
                {
                    Width = random.Next(2, 4), // Random size between 2-5
                    Height = random.Next(2, 4),
                    Fill = Brushes.White,
                    Opacity = random.Next(50, 100) / 100.0 // Random opacity between 0.5-1.0
                };

                // Random position within canvas bounds
                Canvas.SetLeft(star, random.Next(0, 800));
                Canvas.SetTop(star, random.Next(0, 800));

                canvasUniverse.Children.Add(star);
            }
        }
    }
}