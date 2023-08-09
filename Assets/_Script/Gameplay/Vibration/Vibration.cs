using UnityEngine;
using System.Collections;

namespace Phoenix.Gameplay.Vibration
{
    public static class Vibration
    {
        private static bool isRunVibration = true;

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        public static void Vibrate()
        {
            if (!isRunVibration)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif
            if (isAndroid())
                vibrator.Call("vibrate");
            else
                Handheld.Vibrate();
        }


        public static void Vibrate(long milliseconds)
        {
            if (!isRunVibration)
            {
                return;
            }
#if UNITY_IOS
        return;
#endif
#if UNITY_EDITOR
            return;
#endif
            if (isAndroid())
                vibrator.Call("vibrate", milliseconds);
            else
                Handheld.Vibrate();
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
            if (!isRunVibration)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif
            if (isAndroid())
                vibrator.Call("vibrate", pattern, repeat);
            else
                Handheld.Vibrate();
        }

        public static bool HasVibrator()
        {
            return isAndroid();
        }

        public static void Cancel()
        {
            if (isAndroid())
                vibrator.Call("cancel");
        }

        private static bool isAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
            return false;
#endif
        }
        public static void SetState(bool isEnable)
        {
            isRunVibration = isEnable;
        }
    }
}