using OverlayGrid.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OverlayGrid.Controllers
{
    public class HexGridController : IHexGridController
    {
        private double _radius;
        private double _height;
        private int _columns;
        private int _rows;
        private int _canvasWidth;
        private int _canvasHeight;
        private IDictionary<int, IDictionary<int, IDictionary<int, Point>>> _coordinates;

        public int CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; ResetCoordinates(); } }
        public int CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; ResetCoordinates(); } }
        public double Diameter { get => _radius * 2; set { _radius = value / 2; ResetCoordinates(); CalculateValues(); } }

        private void ResetCoordinates()
        {
            _coordinates = null;
        }

        private void CalculateValues()
        {
            CalculateDimensions();
            CalculateColumns();
            CalculateRows();
        }

        private void CalculateDimensions()
        {
            // A hexagon consists of 6 equal triangles which corners are all 60 degrees, each sides length is the radius
            //
            //    1 --- 2             1
            //   / \   / \           /|\
            //  /   \ /   \         / | \
            // 6 --- C --- 3       6 -M- C
            //  \   / \   /
            //   \ /   \ /
            //    5 --- 4
            //
            // The only unknown value here is the height of each of those triangles. Once we know that we can calucalte any coordianates.
            // According to Pythagoras:                 [1-6] = sqrt of ([1-M]^2 + [6-M]^2).
            // Since [6-M] = 0.5 [1-M]:                 [1-6] = sqrt of ([1-M]^2 + (.5*[1-M])^2).
            // Substitute [1-6] for R and 1-M for H:    R = sqrt of (H^2 + (.5H)^2).
            // So:                                      R^2 = H^2 + (.5H)^2
            // So:                                      R^2 = H^2 + .25 * H^2
            // So:                                      R^2 = 1.25 H^2
            // So:                                      H^2 = R^2 / 1.25
            // So:                                      H = sqrt of (R^2 / 1.25)
            _height = Math.Sqrt(Math.Pow(_radius, 2) / 1.25);
        }

        private void CalculateColumns()
        {
            // A column of hexagons on an even row (starting with 0) will de placed in the Canvas every 3 _radius
            // A column of hexagons on an uneven row will de placed in the Canvas every 3 _radius with an offset of 1.5 _height
            _columns = Math.Max(
                (int)Math.Ceiling(CanvasWidth / (3 * _radius)),
                (int)Math.Ceiling((CanvasWidth - (1.5 * _height)) / (3 * _radius))
            );
        }

        private void CalculateRows()
        {
            // The first hexagon will be placed top left, leaving point 1 to start at X = 0.5 _height and Y = 0. 
            // Every other _height a new hexagon row will start
            _rows = (int)Math.Ceiling(CanvasHeight / _height);
        }

        public IDictionary<int, Point> GetCoordinates(int row, int column)
        {
            if (_coordinates == null)
            {
                CalculateColumnCoordinates();
            }
            return _coordinates[row][column];
        }

        private void CalculateColumnCoordinates()
        {
            // Fastest calculation; calculate every coordinate of every 3 rows starting by 0;
            // --> [0,0]   [1,0]   [2,0]   [3,0]   [4,0]   [5,0]   [6,0]
            //          [0,1]   [1,1]   [2,1]   [3,1]   [4,1]   [5,1]
            //     [0,2]   [1,2]   [2,2]   [3,2]   [4,2]   [5,2]   [6,2]
            // -->     [0,3]   [1,3]   [2,3]   [3,3]   [4,3]   [5,3]
            //     [0,4]   [1,4]   [2,4]   [3,4]   [4,4]   [5,4]   [6,4]
            //         [0,5]   [1,5]   [2,5]   [3,5]   [4,5]   [5,5]
            // --> [0,6]   [1,6]   [2,6]   [3,6]   [4,6]   [5,6]   [6,6]
            // To get thes ones for all first rows for uneven rows also calculate the coordinates for row -1

            InitializeCoordinates();
            for (var row = 0; row < _rows + 2; row += 3)
            {
                var xOffset = CalculateXOffset(row);
                var y_1 = row * _height;
                var startColumn = (_rows % 2 == 0) ? 0 : -1;
                for (var col = startColumn; col <= _columns; col++)
                {
                    var x_1 = xOffset + 3 * _radius * col;
                    CalculateCoordinate1(row, col, x_1, y_1);
                    CalculateCoordinate2(row, col, x_1, y_1);
                    CalculateCoordinate3(row, col, x_1, y_1);
                    CalculateCoordinate4(row, col, x_1, y_1);
                    CalculateCoordinate5(row, col, x_1, y_1);
                    CalculateCoordinate6(row, col, x_1, y_1);
                }
            }
        }

        private double CalculateXOffset(int row)
        {
            var xOffset = .5 * _height;
            if (row % 2 == 0)
            {
                xOffset = 2 * _radius;
            }

            return xOffset;
        }

        private void CalculateCoordinate1(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1), (int)Math.Round(y_1));
            SetCoordinate(row, col, 1, point);
            SetCoordinate(row - 1, col, 3, point);
            SetCoordinate(row - 2, col, 5, point);
        }

        private void CalculateCoordinate2(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1 + _radius), (int)Math.Round(y_1));
            SetCoordinate(row, col, 2, point);
            SetCoordinate(row - 1, col + 1, 6, point);
            SetCoordinate(row - 2, col, 4, point);
        }

        private void CalculateCoordinate3(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1 + _radius + .5 * _height), (int)Math.Round(y_1 + _height));
            SetCoordinate(row, col, 3, point);
            SetCoordinate(row - 1, col + 1, 5, point);
            SetCoordinate(row + 1, col + 1, 1, point);
        }

        private void CalculateCoordinate4(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1 + _radius), (int)Math.Round(y_1 + 2 * _height));
            SetCoordinate(row, col, 4, point);
            SetCoordinate(row + 2, col, 2, point);
            SetCoordinate(row + 1, col + 1, 6, point);
        }

        private void CalculateCoordinate5(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1), (int)Math.Round(y_1 + 2 * _height));
            SetCoordinate(row, col, 5, point);
            SetCoordinate(row + 2, col, 1, point);
            SetCoordinate(row + 1, col, 3, point);
        }

        private void CalculateCoordinate6(int row, int col, double x_1, double y_1)
        {
            var point = new Point((int)Math.Round(x_1 - .5 * _height), (int)Math.Round(y_1 + _height));
            SetCoordinate(row, col, 6, point);
            SetCoordinate(row + 1, col, 2, point);
            SetCoordinate(row - 1, col, 4, point);
        }

        private void SetCoordinate(int row, int col, int position, Point point)
        {
            if (row >= 0 && row < _rows &&
                col >= 0 && col < _columns)
            {
                _coordinates[row][col][position] = point;
            }
        }

        private void InitializeCoordinates()
        {
            for (var row = 0; row < _rows; row++)
            {
                _coordinates[row] = new Dictionary<int, IDictionary<int, Point>>();
                for (var col = 0; col < _columns; col++)
                {
                    _coordinates[row][col] = new Dictionary<int, Point>();
                }
            }
        }
    }
}
