using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Monetization.Ads.UI
{
    public class NativeAdPanel : MonoBehaviour
    {
        private CachedNativeAd _cachedNativeAd;
        private NativeAd _nativeAd;
        [SerializeField] private GameObject loadingObject;
        [SerializeField] private GameObject loadedObject;
        [SerializeField] private RawImage icon;
        [SerializeField] private RawImage image;
        [SerializeField] private RawImage adChoices;
        [SerializeField] private Image rateStar;
        [SerializeField] private Text headline;
        [SerializeField] private Text body;
        [SerializeField] private Text callToAction;

        public bool IsNativeAdShowed { get; private set; }

        public CachedNativeAd CachedNativeAd
        {
            get
            {
                return _cachedNativeAd;
            }
            set
            {
                _cachedNativeAd = value;
                SetData(_cachedNativeAd);
                IsNativeAdShowed = false;
            }
        }
        public void Show()
        {
            StartCoroutine(HandleShow());
        }

        private IEnumerator HandleShow()
        {
            yield return new WaitUntil(() => _cachedNativeAd != null);
            gameObject.SetActive(true);
            IsNativeAdShowed = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            AdsController.Instance.RegisterNativeAdPanel(this);
        }

        private void OnDestroy()
        {
            AdsController.Instance.UnRegisterNativeAdPanel(this);
        }

        #region HandleNativeAdData
        public void SetData(CachedNativeAd nativeAd)
        {
            _nativeAd = nativeAd.NativeAd;
            List<Texture2D> imageTexture2DList = _nativeAd.GetImageTextures();
            if (imageTexture2DList != null)
            {
                HandleImageTexture2D(imageTexture2DList);
            }

            Texture2D iconTexture = _nativeAd.GetIconTexture();
            if (iconTexture != null)
            {
                HandleIconTexture(iconTexture);
            }
            Texture2D adChoicesTexture = _nativeAd.GetAdChoicesLogoTexture();
            if (adChoicesTexture != null)
            {
                HandleAdChoiceTexture(adChoicesTexture);
            }
            string headline = _nativeAd.GetHeadlineText();
            if (headline != null && headline != "")
            {
                HandleHeadline(headline);
            }
            string body = _nativeAd.GetBodyText();
            if (body != null && body != "")
            {
                HandleBody(body);
            }
            string callToAction = _nativeAd.GetCallToActionText();
            if (callToAction != null && callToAction != "")
            {
                HandleCallToAction(callToAction);
            }
            double starRating = _nativeAd.GetStarRating();
            if (starRating != 0)
            {
                HandleStarRating(starRating);
            }

            HandleNativeAdError();
        }

        private void HandleNativeAdError()
        {
            if (!_nativeAd.RegisterIconImageGameObject(icon.gameObject))
            {
                Debug.Log("error registering Icon");
            }
            if (!_nativeAd.RegisterStoreGameObject(image.gameObject))
            {
                Debug.Log("error registering Image");
            }
            if (!_nativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject))
            {
                Debug.Log("error registering adChoices");
            }
            if (!_nativeAd.RegisterHeadlineTextGameObject(headline.gameObject))
            {
                Debug.Log("error registering headline");
            }
            if (!_nativeAd.RegisterCallToActionGameObject(callToAction.gameObject))
            {
                Debug.Log("error registering callToAction");
            }
            if (!_nativeAd.RegisterBodyTextGameObject(body.gameObject))
            {
                Debug.Log("error registering body");
            }
        }

        private void HandleStarRating(double starRating)
        {
            Debug.Log(starRating);
        }

        private void HandleCallToAction(string callToActionText)
        {
            callToAction.text = callToActionText;
        }

        private void HandleBody(string bodyText)
        {
            body.text = bodyText;
        }

        private void HandleHeadline(string headlineText)
        {
            headline.text = headlineText;
        }

        private void HandleAdChoiceTexture(Texture2D adChoicesTexture)
        {
            adChoices.texture = adChoicesTexture;
        }

        private void HandleIconTexture(Texture2D iconTexture)
        {
            icon.texture = iconTexture;
        }

        private void HandleImageTexture2D(List<Texture2D> imageTexture2DList)
        {
            Texture2D imageTexture = GetRandom(imageTexture2DList);
            image.texture = imageTexture;
        }

        private Texture2D GetRandom(List<Texture2D> imageTexture2dList)
        {
            return imageTexture2dList[Random.Range(0, imageTexture2dList.Count)];
        }
        #endregion
    }
}