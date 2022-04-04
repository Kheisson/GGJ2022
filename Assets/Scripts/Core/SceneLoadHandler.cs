using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class SceneLoadHandler : MonoBehaviour
    {
        [SerializeField] private Image progressBar;

        #region Consts
        private const string Map = "Map";
        private const string Level = "Level";
        #endregion

        #region Methods

        IEnumerator BeginLoading(string sceneName)
        {
            DOTween.KillAll();
            progressBar.fillAmount = 0f;
            yield return new WaitForSecondsRealtime(1f);
            var sceneToLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!sceneToLoad.isDone)
            {
                progressBar.fillAmount = sceneToLoad.progress;
                if (sceneToLoad.progress >= 0.9f)
                {
                    sceneToLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
        public void LoadLevel(string levelNum)
        {
            var level = Level + levelNum;
            StartCoroutine(BeginLoading(level));
        }

        public void LoadToMap()
        {
            StartCoroutine(BeginLoading(Map));
        }

        #endregion
    }
}