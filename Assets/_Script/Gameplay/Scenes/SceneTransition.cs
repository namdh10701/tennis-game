using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Phoenix
{
    public class SceneTransition : MonoBehaviour
    {
        Animator _animator;
        bool _changeSceneAllowed;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            EnterScene();
        }
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        public IEnumerator ChangeSceneCoroutine(string sceneName)
        {
            _changeSceneAllowed = false;
            AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(sceneName);
            loadSceneOp.allowSceneActivation = false;
            _animator.Play("Exit");
            while (!_changeSceneAllowed)
            {
                yield return null;
            }
            loadSceneOp.allowSceneActivation = true;
        }

        public void PreloadChangeScene(AsyncOperation loadSceneOp)
        {
            StartCoroutine(PreloadChangeSceneCoroutine(loadSceneOp));
        }

        public IEnumerator PreloadChangeSceneCoroutine(AsyncOperation loadSceneOp)
        {
            _changeSceneAllowed = false;
            _animator.Play("Exit");
            while (!_changeSceneAllowed)
            {
                yield return null;
            }
            Debug.Log("pkl");
            loadSceneOp.allowSceneActivation = true;
        }

        public void OnAllowToChangeScene()
        {
            _changeSceneAllowed = true;
        }

        public void EnterScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
                _animator.Play("Enter");
        }
    }
}
