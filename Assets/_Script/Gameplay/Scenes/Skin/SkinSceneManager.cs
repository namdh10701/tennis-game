using Monetization.Ads;
using Monetization.Ads.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SkinSceneManager : MonoBehaviour
    {
        [SerializeField] private SkinUI _skinUI;
        private void Start()
        {
            _skinUI.Init(GameDataManager.Instance.GameDatas.Skins);
        }

    }
}
