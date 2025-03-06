using System.Collections.Generic;
using UnityEngine;

public class PaintSplatterHandler : MonoBehaviour
{
    public static PaintSplatterHandler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else 
        {
            Instance = this;
        }
    }

    public List<Color> Colors;
    public List<Sprite> Splatters;
}
