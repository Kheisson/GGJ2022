using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawn
{
    public static class SpawnGrid
    {

        #region Fields

        private static int _maxScreenSize;
        private static readonly List<GridTile> Grid = new List<GridTile>();

        #endregion

        #region Methods

        public static void Init()
        {
            InitializeGrid();
        }
        
        /// <summary>
        ///  Returns the first position that is not occupied 
        /// </summary>
        /// <returns>An unoccupied position on the grid or a Vector3.back in case there is no place left</returns>
        public static Vector3 GetOpenSpot()
        {
            var selected = Grid.FirstOrDefault(space => space.IsOccupied == false);

            if (selected != null)
            {
                selected.IsOccupied = true;
                Grid[Random.Range(0, Grid.Count)].IsOccupied = true;
                return selected.Location;
            }
            
            return Vector3.back;
        }

        /// <summary>
        /// Sets all tiles to occupied
        /// </summary>
        public static void ClearGrid()
        {
            foreach (var grid in Grid)
            {
                grid.IsOccupied = false;
            }
        }

        /// <summary>
        /// Initializes grid based on the screen boundaries set in GameSettings
        /// </summary>
        private static void InitializeGrid()
        {
            _maxScreenSize = -GameSettings.ScreenBoundaries.x > 30 ? 7 : 5;
            for (var i = 0; i < _maxScreenSize; i++)
            {
                Grid.Add(new GridTile(i));
            }
        }

        #endregion

        #region Inner Class

        private class GridTile
        {
            public bool IsOccupied = false;
            public Vector3 Location { get; private set; }

            public GridTile(int spot)
            {
                Location = new Vector3(SetLocation(spot), 0f, 0f);
            }
            
            //Returns location based on the place in loop
            private float SetLocation(int space)
            {
                var place = 0f;
                switch (space) //0 -> 17, 1 - -8.5, 2 - 8.5, 3 - -17 , 4 - 0
                {
                    case 0:
                        place = 17f;
                        break;
                    case 1:
                        place = -8.5f;
                        break;
                    case 2:
                        place = 8.5f;
                        break;
                    case 3:
                        place = -17f;
                        break;
                    case 4:
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

        #endregion
    }
}