using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonTransparency : MonoBehaviour
{
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.5f;
    }
}
