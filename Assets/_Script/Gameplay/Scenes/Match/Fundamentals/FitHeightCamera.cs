using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class FitHeightCamera : MonoBehaviour
    {
        public Canvas canvas;
        public SpriteRenderer bg;
        public RectTransform banner;
        private void Awake()
        {
            bool isReversed = SettingManager.Instance.GameSettings.IsReversed;
            float bgWidth = bg.bounds.size.x;
            float bgHeight = bg.bounds.size.y;
            float targetOrthoSize1 = Mathf.Max(bgWidth * 0.5f / Camera.main.aspect, bgHeight * 0.5f);


            float cameraOriginalSize = 5;
            float ratio = banner.rect.height / Screen.height;
            Debug.Log(ratio);
            float targetOrthoSize2 = cameraOriginalSize * ratio * 2 + cameraOriginalSize;

            Camera.main.orthographicSize = targetOrthoSize2 > targetOrthoSize1 ? targetOrthoSize2 : targetOrthoSize1;
            Camera.main.transform.position = new Vector3(0, +(Camera.main.orthographicSize - cameraOriginalSize) * .5f, -10);
            //Camera.main.transform.position = new Vector3(0, +(Camera.main.orthographicSize - cameraOriginalSize), -10);

            if (isReversed)
            {
                Camera.main.transform.position = new Vector3(0, +(Camera.main.orthographicSize - cameraOriginalSize) * .5f, -10);

            }
            else
            {
                Camera.main.transform.position = new Vector3(0, -(Camera.main.orthographicSize - cameraOriginalSize) * .5f, -10);

            }
        }
    }
}
