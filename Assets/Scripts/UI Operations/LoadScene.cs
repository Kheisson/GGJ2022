using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image loadingBarImage;

    #endregion

    #region Methods

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(LoadSceneAsync());
    }
    
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingBarImage.fillAmount = asyncOperation.progress;

            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}
