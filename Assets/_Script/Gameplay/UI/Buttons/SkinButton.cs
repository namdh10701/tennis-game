using UnityEngine;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine.UI;
using Monetization.Ads;
using System;

namespace UI
{
    public class SkinButton : MonoBehaviour
    {
        private AnimatedButton _button;
        private Image _image;
        private void Awake()
        {
            _button = GetComponent<AnimatedButton>();
            _image = GetComponent<Image>();
        }
        public void Init(Skin skin)
        {
            _image.sprite = skin.Sprite;
            _button.SetOnClickEnvent(() =>
            {
                AdsController.Instance.ShowReward(
                    (watched) =>
                    {
                        if (watched)
                        {
                            Debug.Log("ok");
                        }
                        else
                        {

                            Debug.Log("not ok");
                        }
                    }
                    );
            });

        }

        public void Disalbe()
        {
            _image.color = new Color(0, 0, 0, 0);
            _button.SetEnable(false);
        }
    }
}
