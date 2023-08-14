using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phoenix
{
    public class FitHeightCamera : MonoBehaviour
    {
        public SpriteRenderer bg;

        private void Awake()
        {
            // Get the dimensions of the background sprite
            float bgWidth = bg.bounds.size.x;
            float bgHeight = bg.bounds.size.y;

            // Calculate the required orthographic size to fit both width and height
            float targetOrthoSize = Mathf.Max(bgWidth * 0.5f / Camera.main.aspect, bgHeight * 0.5f);

            // Set the calculated orthographic size to the camera
            Camera.main.orthographicSize = targetOrthoSize;
        }
    }
}
