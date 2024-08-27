using BLL.Services;
using DAL;
using DAL.Models;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UI.Helpers;
using UI.Services;

namespace UI
{
    public partial class MainWindow : Window
    {
        private List<MarkerData> markers = new List<MarkerData>();
        private UnitServices unit;
        private readonly GMap.NET.PointLatLng InitialPosition = new GMap.NET.PointLatLng(37.234332396, -115.80666344);
        private DispatcherTimer _timer;
        private readonly MovementServiceClient _serviceClient;
        private MarkerAnimator _markerAnimator;
        private DateTime _lastProcessedTimestamp = DateTime.MinValue;
        public MainWindow(MovementService movementService, UnitServices unitServices)
        {
            unit = unitServices;
            _serviceClient = new MovementServiceClient();
            InitializeComponent();
            InitializeTimer();
            LoadData();
            
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
            _timer.Tick += async (sender, e) => await FetchMarkerDataAsync();
            _timer.Start();
        }

        private void LoadData()
        {
            // Configure Map
            MyMap.MapProvider = GMapProviders.OpenStreetMap;
            MyMap.Position = InitialPosition;
            MyMap.Zoom = 18;
            MyMap.ShowCenter = false;
            MyMap.CanDragMap = true;
        }
        private async Task FetchMarkerDataAsync()
        {
            var allMovements = await _serviceClient.GetAllMovementsAsync();
            var AllUnits = unit.GetAllUnits();
            if (_lastProcessedTimestamp == DateTime.MinValue && allMovements.Count > 0)
            {
                // Initial load: process all movements
                _lastProcessedTimestamp = allMovements.Max(m => m.Timestamp);

                // Initialize and start marker animation with all movements
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        _markerAnimator = new MarkerAnimator(MyMap, allMovements
                                                                        .OrderBy(m => m.Timestamp)
                                                                        .ToList()
                                                                        , AllUnits);
                        _markerAnimator.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error starting marker animation: {ex.Message}");
                    }
                });
            }
            else if(allMovements.Count > 0)
            {
                // Subsequent load: filter new movements
                var newMovements = allMovements
                    .Where(m => m.Timestamp > _lastProcessedTimestamp)
                    .OrderBy(m => m.Timestamp)
                    .ToList();

                // Update the last processed timestamp with the latest from the new movements
                if (newMovements.Any())
                {
                    _lastProcessedTimestamp = newMovements.Max(m => m.Timestamp);

                    // Run marker animation on the UI thread
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            _markerAnimator = new MarkerAnimator(MyMap, newMovements, AllUnits);
                            _markerAnimator.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error starting marker animation: {ex.Message}");
                        }
                    });
                }
                else
                {
                    Console.WriteLine("No new movements available.");
                }
            }
        }
        private void LoadMarkers()
        {
            // Clear existing markers
            MyMap.Markers.Clear();

            foreach (var markerData in markers)
            {
                var marker = CreateImageMarker(markerData.Position, markerData.ExtraData);
                MyMap.Markers.Add(marker);
            }
        }
        private GMapMarker CreateImageMarker(PointLatLng position, string ExtraData)
        {
            var canvas = new Canvas();
            var image = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\charl\source\repos\MilitaryTraining\UI\images\triangle.png"))
                , Width = 30    
                , Height = 40
            };

            canvas.Children.Add(image);
            Canvas.SetLeft(image, -15); // Center the image horizontally
            Canvas.SetTop(image, -15);  // Center the image vertically

            var marker = new GMapMarker(position)
            {
                Shape = canvas
            };

            // Handle mouse click event
            image.MouseLeftButtonDown += (s, e) =>
            {
                ShowMarkerData(ExtraData);
            };

            return marker;
        }

        private void ShowMarkerData(string markerData)
        {
            // Display extra data in a suitable UI element
            MessageBox.Show(markerData);
        }

        //private async void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    // Fetch and display marker data based on slider value
        //    await FetchMarkerDataAsync();
        //}
    }
}
    

