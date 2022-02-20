using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public static class SpawnGrid
    {
        
        private static int _maxScreenSize;
        private static List<GridTile> _grid = new List<GridTile>();

        public static List<GridTile> Grid
        {
            get
            {
                if (_grid.Count != 0)
                    return _grid;
                
                _maxScreenSize = -GameSettings.ScreenBoundaries.x > 30 ? 7 : 5;
                InitializeGrid(_maxScreenSize);
                return _grid;
            }
        }

        public static Vector3 GetOpenSpot()
        {
            var selected = _grid.FirstOrDefault(space => space.IsOccupied == false);

            if (selected != null)
            {
                selected.IsOccupied = true;
                _grid[Random.Range(0, _grid.Count)].IsOccupied = true;
                return selected.Location;
            }
            
            return Vector3.back;
        }

        public static void ClearGrid()
        {
            foreach (var grid in _grid)
            {
                grid.IsOccupied = false;
            }
        }

        private static void InitializeGrid(int tableSize)
        {
            for (var i = 0; i < tableSize; i++)
            {
                _grid.Add(new GridTile(i));
            }
        }

        //Inner class that holds data for a grid tile
        public class GridTile
        {
            private Vector3 _location;
            
            public bool IsOccupied = false;
            public Vector3 Location
            {
                get => _location;
                private set => _location = value;
            }

            public GridTile(int spot)
            {
                _location = new Vector3(SetLocation(spot), 0f, 0f);
            }
            
            //Returns location based on the place in loop
            private float SetLocation(int space)
            {
                var place = 0f;
                switch (space)
                {
                    case 0:
                        break;
                    case 1:
                        place = 8.5f;
                        break;
                    case 2:
                        place = -8.5f;
                        break;
                    case 3:
                        place = 17f;
                        break;
                    case 4:
                        place = -17f;
                        break;
                    case 5:
                        place = 25.5f;
                        break;
                    case 6:
                        place = -25.5f;
                        break;
                }
                return place;
            }
        }
    }
}