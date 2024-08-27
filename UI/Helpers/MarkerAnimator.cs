using DAL.Models;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace UI.Helpers
{
    public class MarkerAnimator
    {
        private readonly DispatcherTimer _timer;
        private readonly List<Movement> _movements;
        private readonly List<Unit> _units;
        private readonly GMapControl _map;
        private readonly Dictionary<int, GMapMarker> _unitMarkers;
        private readonly Dictionary<int, Movement> _currentMovements;
        private readonly Dictionary<int, DateTime> _lastUpdateTimes;
        private readonly Dictionary<int, PointLatLng> _targetPositions;

        public MarkerAnimator(GMapControl map, List<Movement> movements, List<Unit> units)
        {
            _map = map;
            _movements = movements;
            _units = units;
            _unitMarkers = new Dictionary<int, GMapMarker>();
            _currentMovements = new Dictionary<int, Movement>();
            _lastUpdateTimes = new Dictionary<int, DateTime>();
            _targetPositions = new Dictionary<int, PointLatLng>();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Update every second
            };
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            InitializeMarkers();
            _timer.Start();
        }

        private void InitializeMarkers()
        {
            foreach (var movement in _movements)
            {
                if (!_unitMarkers.ContainsKey(movement.UnitId))
                {
                    var marker = CreateMarker(movement.UnitId, movement.Status);
                    _unitMarkers[movement.UnitId] = marker;
                    _map.Markers.Add(marker);

                    // Initialize positions
                    _currentMovements[movement.UnitId] = movement;
                    _targetPositions[movement.UnitId] = new PointLatLng(movement.Latitude, movement.Longitude);
                    _lastUpdateTimes[movement.UnitId] = DateTime.Now;
                }
            }
        }

        private GMapMarker CreateMarker(int unitId, string status)
        {
            var canvas = new Canvas();
            var image = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Images/transperenteTriangle.png")),
                Width = 30,
                Height = 30
            };

            canvas.Children.Add(image);
            Canvas.SetLeft(image, -15);
            Canvas.SetTop(image, -15);
            string initialStatus = "Starting Drill!";
            var textBlock = new TextBlock
            {
                Text = initialStatus,
                Foreground = System.Windows.Media.Brushes.Black,
                Background = System.Windows.Media.Brushes.White,
                Margin = new System.Windows.Thickness(-15, 30, 0, 0),
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                Visibility = Visibility.Collapsed // Initially hidden
            };
            Canvas.SetLeft(textBlock, -15);
            Canvas.SetTop(textBlock, 15); // Adjust as needed
            canvas.Children.Add(textBlock);

            image.MouseLeftButtonDown += (s, e) =>
            {
                textBlock.Visibility = textBlock.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            };

            return new GMapMarker(new PointLatLng(0, 0)) { Shape = canvas };
        }

        private void ShowMarkerData(string markerData)
        {
            MessageBox.Show(markerData);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            foreach (var unitId in _unitMarkers.Keys.ToList())
            {
                if (_currentMovements.TryGetValue(unitId, out var currentMovement))
                {
                    // Filtrar movimentos futuros para o unitId atual
                    var nextMovement = _movements
                        .Where(m => m.UnitId == unitId && m.Timestamp > currentMovement.Timestamp)
                        .OrderBy(m => m.Timestamp)
                        .FirstOrDefault();

                    if (nextMovement != null)
                    {
                        // Calcular o tempo decorrido desde a última atualização
                        var elapsed = (now - _lastUpdateTimes[unitId]).TotalSeconds;
                        var totalMoveTime = (nextMovement.Timestamp - currentMovement.Timestamp).TotalSeconds;

                        if (elapsed >= totalMoveTime)
                        {
                            // Atualizar a posição do marcador para o próximo movimento
                            UpdateMarkerPosition(unitId, new PointLatLng(nextMovement.Latitude, nextMovement.Longitude));
                            _currentMovements[unitId] = nextMovement;
                            _targetPositions[unitId] = new PointLatLng(nextMovement.Latitude, nextMovement.Longitude);
                            _lastUpdateTimes[unitId] = now;
                        }
                        else
                        {
                            // Interpolação da posição
                            var startPosition = _unitMarkers[unitId].Position;
                            var endPosition = _targetPositions[unitId];
                            var progress = elapsed / totalMoveTime;

                            var newLat = startPosition.Lat + (endPosition.Lat - startPosition.Lat) * progress;
                            var newLng = startPosition.Lng + (endPosition.Lng - startPosition.Lng) * progress;
                            UpdateMarkerPosition(unitId, new PointLatLng(newLat, newLng));
                        }

                        var marker = _unitMarkers[unitId];
                        var canvas = marker.Shape as Canvas;
                        var textBlock = canvas?.Children.OfType<TextBlock>().FirstOrDefault();

                        if (textBlock != null)
                        {
                            string latestStatus = currentMovement != null ? currentMovement.Status : "Unknown";
                            textBlock.Text = $"Name: {_units.First(x => x.Id == unitId).UnitName}" +
                                $"\nLat: {currentMovement.Latitude} Long: {currentMovement.Latitude}" +
                                $"\nStatus: {latestStatus} " +
                                $"\nTime: {currentMovement.Timestamp}" +
                                $"\nId: {currentMovement.Id} ";
                        }
                    }
                }
            }
        }




        private void UpdateMarkerPosition(int unitId, PointLatLng newPosition)
        {
            var marker = _unitMarkers[unitId];
            marker.Position = newPosition;
        }
    }
}
