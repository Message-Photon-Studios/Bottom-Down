using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ColorShrooms : MonoBehaviour
{
    [SerializeField] int colorAmount;
    [SerializeField] Material emptyMaterial;
    [SerializeField] SerializedDictionary<GameColor, Material> colorMaterials;


    
}
