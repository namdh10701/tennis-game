using Monetization.Ads;
using Monetization.Ads.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phoenix
{
    public class AdsExample : MonoBehaviour
    {
        public InterPopup popup;
        public NativeAdPanel nativeAdPanel;
        // Start is called before the first frame update

        private void Start()
        {
            AdsController.Instance.ShowNativeAd(nativeAdPanel);
        }

        public void OnShowInter()
        {
            AdsController.Instance.ShowInter(
                () =>
                {
                    // Sau khi Inter ad đóng thì chạy cái gì
                    // Vd: chuyển scene, đóng popup,...
                }
                );
        }

        public void OnShowReward()
        {
            AdsController.Instance.ShowReward(
                (watched) =>
                {
                    if (watched)
                    {
                        // phần thưởng cho người chơi
                    }
                }
                );
        }

        public void OnOpenInterPopup()
        {
            popup.Open();
            //popup.Open(false); nếu muốn mở lên mà k xem Inter
            //tương tự với popup.Close(); và popup.Close(false);
        }
    }
}
