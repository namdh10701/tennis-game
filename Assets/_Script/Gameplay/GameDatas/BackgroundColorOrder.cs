using System;
using System.Collections.Generic;

[Serializable]
public class BackgroundColorOrder
{
    public List<string> Strings;

    public BackgroundColorOrder()
    {
        Strings = new List<string>
        {
            "red",
            "yellow",
            "green",
            "blue",
            "purple",
            "orange",
            "pink"
        };
    }
}