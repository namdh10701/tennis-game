using System;
using UnityEngine;

public static class GetSetTimeSave
{
    public static void SetTime(string data)
    {
        PlayerPrefs.SetString(data, System.DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }
    public static void SetTime(string data, DateTime dateTime)
    {
        PlayerPrefs.SetString(data, dateTime.ToBinary().ToString());
        PlayerPrefs.Save();
    }
    public static DateTime GetTime(string data)
    {
        long temp = Convert.ToInt64(PlayerPrefs.GetString(data, "0"));
        DateTime date = DateTime.FromBinary(temp);
        return date;
    }
    public static TimeSpan GetTimeToNow(string data)
    {
        TimeSpan timeSpan = DateTime.Now - GetTime(data);
        return timeSpan;
    }
}