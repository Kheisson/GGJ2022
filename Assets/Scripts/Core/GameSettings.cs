using System;
using UnityEngine;

namespace Core
{
    public static class GameSettings
    {
        #region Fields
        
        private static Vector3 _screenBoundaries;
        private static float? _screenBoundariesTop = null;
        private const float SCREEN_BUFFERZONE = 10f;
        private const float SCREEN_BOUNDRY_DIVIDER = 1.3f;

        #endregion

        #region Properties
        /// <summary>
        /// Sets the boundaries based on the screen size when the app is launched
        /// </summary>
        public static Vector3 ScreenBoundaries
        {
            get
            {
                if (_screenBoundaries != Vector3.zero)
                {
                    return _screenBoundaries;
                }
                
                _screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    Camera.main.transform.position.z));
                return _screenBoundaries;
            }
        }
        /// <summary>
        /// Returns a coordinate on the Y axis that represents a deadzone on top (where the buttons are)
        /// </summary>
        public static float? ScreenBoundariesTop
        {
            get
            {
                if (_screenBoundariesTop != null)
                {
                    return _screenBoundariesTop;
                }

                _screenBoundariesTop = Math.Abs(_screenBoundaries.y + SCREEN_BUFFERZONE);
                return _screenBoundariesTop;
            }
        }

        public static float ScreenBoundaryDivider => SCREEN_BOUNDRY_DIVIDER;

        #endregion
    }
}