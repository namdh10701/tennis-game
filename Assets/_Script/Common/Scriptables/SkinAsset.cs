using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/SkinAsset", fileName = "SkinAsset")]
public class SkinAsset : ScriptableObject
{
    public List<Sprite> skinSprites;
}