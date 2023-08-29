using UnityEngine;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine.UI;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using DG.Tweening;
using UnityEngine.Purchasing;

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
        [SerializeField] private Image _icon;
        [SerializeField] private Button _AdButton;
        public Skin Skin { get; set; }
        public bool Disabled { get; private set; }
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        public void Init(SkinUI skinUI, Skin skin, Sprite sprite, Sprite iconSprite)
        {
            Skin = skin;
            _icon.sprite = iconSprite;
            _toolImage.sprite = sprite;
            _toolImage.preserveAspect = true;
            _lock.SetActive(!skin.Unlocked);

            if (skin.Type == Skin.SkinType.GLOVES)
            {
                _toolImage.transform.localScale *= 0.97f;
                _toolImage.transform.DOLocalRotate(new Vector3(0, 0, 45), 0);
            }
            else
            {
                _toolImage.transform.localScale *= 1.1f;
                _toolImage.transform.DOLocalRotate(new Vector3(0, 0, -45), 0);
                if (skin.Type == Skin.SkinType.RACKET || skin.Type == Skin.SkinType.BASEBAT)
                {
                    _toolImage.transform.localScale *= 1.1f;
                }
            }

            if (!Skin.Unlocked)
            {
                _button.IsClickable = false;
                _AdButton.onClick.AddListener(() =>
                {
                    FirebaseAnalytics.Instance.PushEvent("REWARD_AD_CLICKED_UNLOCK_SKIN");
                    AdsController.Instance.ShowReward(
                        (watched) =>
                        {
                            if (watched)
                            {
                                _button.IsClickable = true;
                                _lock.SetActive(false);
                                _AdButton.gameObject.SetActive(false);
                                GameDataManager.Instance.UnlockSkin(Skin);
                                FirebaseAnalytics.Instance.PushEvent("REWARD_AD_COMPLETED_SKIN_ID_" + Skin.ID);
                                _button.RemoveOnClickEvent();
                                _button.SetOnClickEnvent(() =>
                                {
                                    skinUI.OnSkinSelect(Skin);
                                });
                            }
                        }
                        );
                });

            }
            else
            {
                _AdButton.gameObject.SetActive(false);
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
