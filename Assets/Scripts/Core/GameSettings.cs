using UnityEngine;

namespace Core
{
    public static class GameSettings
    {
        #region Fields
        
        private static Vector3 _screenBoundaries;

        #endregion

        #region Methods
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

        #endregion
    }
}