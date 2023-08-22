using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SkinSceneManager : MonoBehaviour
    {
        [SerializeField] private SkinUI _skinUI;
        private List<Skin> skins;
        private void Awake()
        {
            skins = new List<Skin>();
            skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin()); skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin());
            skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin()); skins.Add(new Skin());
            _skinUI.Init(skins);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
