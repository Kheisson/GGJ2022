using System.Collections.Generic;
using Core;
using UnityEngine;

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
        //Gets a position based on a grid spot
        public static Vector3 GetSpot(int position)
        {
            return Grid[position].Location;
        }
        //Returns a relative spot based on the X values of the item
        public static int GetSpotBasedOnPosition(float position)
        {
            int place = 0;
            switch (position)
            {
               case -17f:
                   place = 0;
                   break;
               case -8.5f:
                   place = 1;
                   break;
               case 0f:
                   place = 2;
                   break;
               case 8.5f:
                   place = 3;
                   break;
               case 17f:
                   place = 4;
                   break;
               case -25.5f:
                   place = 0;
                   break;
               case 25.5f:
                   place = 4;
                   break;
            }
            return place;
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
            public Vector3 Location { get; }

            public GridTile(int spot)
            {
                Location = new Vector3(SetLocation(spot), 0f, 0f);
            }
            
            //Returns location based on the place in loop
            private float SetLocation(int space)
            {
                var place = 0f;
                
                if ((space == 0 || space == 4) && _maxScreenSize == 7)
                    space = space == 0 ? 5 : 6; //Spot adjustment for iPad Resolutions
                
                switch (space)
                {
                    case 0:
                        place = -17f;
                        break;
                    case 1:
                        place = -8.5f;
                        break;
                    case 2:
                        break;
                    case 3:
                        place = 8.5f;
                        break;
                    case 4:
                        place = 17f;
                        break;
                    case 5:
                        place = -25.5f;
                        break;
                    case 6:
                        place = 25.5f;
                        break;
                }
                return place;
            }
        }

        #endregion
    }
}