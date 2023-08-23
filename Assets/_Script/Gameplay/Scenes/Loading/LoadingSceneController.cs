using System.Collections;
using Enviroments;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Monetization.Ads;
using Phoenix;

namespace Gameplay
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField] private SceneTransition _sceneTransition;
        public Image progressBar;
        private void Start()
        {
            if (Enviroment.ENV == Enviroment.Env.DEV)
                _sceneTransition.ChangeScene("MenuScene");
            else
                StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            float startTime = Time.time;
            float timeout = 5;
            AsyncOperation asyncOperation;
            asyncOperation = SceneManager.LoadSceneAsync("MenuScene");
            asyncOperation.allowSceneActivation = false;
            bool openAdShowed = false;
            // Loading phase
            while (!asyncOperation.isDone)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime > 4)
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
                    _sceneTransition.PreloadChangeScene(asyncOperation);
                }

                yield return null;
            }
        }
    }
}
