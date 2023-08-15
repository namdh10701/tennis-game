using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Monetization.Ads;

namespace Gameplay
{
    public class LoadingSceneController : MonoBehaviour
    {
        public Image progressBar;
        private void Start()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            float startTime = Time.time;
            float timeout = 3;
            AsyncOperation asyncOperation;
            asyncOperation = SceneManager.LoadSceneAsync("MenuScene");
            asyncOperation.allowSceneActivation = false;
            bool openAdShowed = false;
            // Loading phase
            while (!asyncOperation.isDone)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime > 2)
                {
                    if (!openAdShowed)
                    {
                        openAdShowed = true;
                        AdsController.Instance.ShowAppOpenAd();
                    }
                }
                if (elapsedTime < timeout)
                {
                    progressBar.fillAmount = Mathf.Clamp01(elapsedTime / timeout);
                }
                else
                {
                    progressBar.fillAmount = Mathf.Clamp01(asyncOperation.progress / 0.9f + (elapsedTime - timeout) / timeout);
                }

                if (asyncOperation.progress >= 0.9f && elapsedTime >= timeout)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
