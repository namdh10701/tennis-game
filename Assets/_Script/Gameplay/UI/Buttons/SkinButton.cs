using UnityEngine;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine.UI;
using Monetization.Ads;
using Services.FirebaseService.Analytics;

namespace UI
{
    public class SkinButton : MonoBehaviour
    {
        [SerializeField] private AnimatedButton _button;
        private Image _image;
        [SerializeField] private Image _toolImage;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _lock;
        [SerializeField] private GameObject _beingUsed;
        public Skin Skin { get; set; }
        public bool Disabled { get; private set; }
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        public void Init(SkinUI skinUI, Skin skin, Sprite sprite)
        {
            Skin = skin;
            _toolImage.sprite = sprite;
            _toolImage.preserveAspect = true;
            _lock.SetActive(!skin.Unlocked);
            if (!Skin.Unlocked)
            {
                _button.SetOnClickEnvent(() =>
                {
                    FirebaseAnalytics.Instance.PushEvent("REWARD_AD_CLICKED_UNLOCK_SKIN");
                    AdsController.Instance.ShowReward(
                        (watched) =>
                        {
                            if (watched)
                            {
                                _lock.SetActive(false);
                                GameDataManager.Instance.UnlockSkin(Skin);
                                FirebaseAnalytics.Instance.PushEvent("REWARD_AD_COMPLETED_SKIN_ID_" + Skin.ID);
                            }
                            _button.RemoveOnClickEvent();
                            _button.SetOnClickEnvent(() =>
                            {
                                skinUI.OnSkinSelect(Skin);
                            });
                        }
                        );
                });
            }
            else
            {
                UpdateSkinButtonVisualAndFunction(skinUI);
            }
        }

        public void UpdateSkinButtonVisualAndFunction(SkinUI skinUI)
        {
            if (Skin.BeingUsed)
                _beingUsed.SetActive(true);
            else
            {
                _beingUsed.SetActive(false);
                _button.RemoveOnClickEvent();
                _button.SetOnClickEnvent(() =>
                {
                    skinUI.OnSkinSelect(Skin);
                });
            }
        }

        public void Disalbe()
        {
            Disabled = true;
            Debug.Log
                ((_image == null) + "image is null");
            _image.color = new Color(0, 0, 0, 0);
            _content.SetActive(false);


        }
    }
}
