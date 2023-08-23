using UnityEngine;
using System.Collections;

namespace Gameplay
{
    public static class Vibration
    {
        private static bool _isRunVibration = true;

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject Vibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass UnityPlayer;
        public static AndroidJavaObject CurrentActivity;
        public static AndroidJavaObject Vibrator;
#endif

        public static void Vibrate()
        {
            if (!_isRunVibration || !IsAndroid())
                return;

            if (IsAndroid())
                Vibrator.Call("vibrate");
            else
                Handheld.Vibrate();
        }


        public static void Vibrate(long milliseconds)
        {
            if (!_isRunVibration || !IsAndroid())
                return;

            if (IsAndroid())
                Vibrator.Call("vibrate", milliseconds);
            else
                Handheld.Vibrate();
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
           if (!_isRunVibration || !IsAndroid())
                return;

            if (IsAndroid())
                Vibrator.Call("vibrate", pattern, repeat);
            else
                Handheld.Vibrate();
        }

        public static bool HasVibrator()
        {
            
            return IsAndroid();
        }

        public static void Cancel()
        {
            if (IsAndroid())
                Vibrator.Call("cancel");
        }

        private static bool IsAndroid()
        {
            return UnityPlayer != null && CurrentActivity != null && Vibrator != null;
        }
        public static void SetState(bool isEnable)
        {
            _isRunVibration = isEnable;
        }
    }
}