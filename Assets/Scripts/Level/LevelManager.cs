using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {

        #region Fields
        [SerializeField] private float backgroundSpeed = 12f;
        private GameObject _backgroundGameObject;
        private bool _levelOnGoing = false;
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

        private void Start()
        {
            _backgroundGameObject = GameObject.FindGameObjectWithTag("Background");
        }

        private void Update()
        {
            if (!_levelOnGoing) return;
            _backgroundGameObject.transform.Translate(Vector3.forward * backgroundSpeed * Time.deltaTime);
        }
       
        public void LevelOnGoingChange() => _levelOnGoing = !_levelOnGoing;

        #endregion


    }
}
