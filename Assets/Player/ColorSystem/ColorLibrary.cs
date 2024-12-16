using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLibrary : MonoBehaviour
{
    [SerializeField] GameColor[] colors;
    [SerializeField] GameColor[] primaryColors;
    [SerializeField] GameColor[] secondaryColors;
    [SerializeField] GameColor rainbow;

    public GameColor GetRandomColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }

    public GameColor GetRandomPrimaryColor()
    {
        return primaryColors[Random.Range(0, primaryColors.Length)];
    }

    public GameColor GetRandomSecondaryColor()
    {
        return secondaryColors[Random.Range(0, secondaryColors.Length)];
    }
}
