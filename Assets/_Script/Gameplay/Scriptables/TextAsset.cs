using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/TextAsset", fileName = "TextAsset")]
public class TextAsset : ScriptableObject
{
    public List<Sprite> TextSprites;
}