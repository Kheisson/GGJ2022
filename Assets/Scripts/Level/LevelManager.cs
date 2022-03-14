using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {

        #region Fields

        private const string UISceneName = "main";

        public Action UISceneLoadedEvent;

        #endregion

        #region Methods

        private void Awake()
        {
            var uiScene = SceneManager.LoadSceneAsync(UISceneName, LoadSceneMode.Additive);
            while (uiScene.isDone)
            {
                return;
            }
            uiScene.allowSceneActivation = true;
            uiScene.completed += operation => UISceneLoadedEvent?.Invoke();
        }

        #endregion
    
    
    }
}
