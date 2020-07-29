using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    [SerializeField]
    Sprite nightBG;

    // Start is called before the first frame update
    void Start()
    {
        if(stateManager.Instance.nightMode == true)
        {
            background.GetComponent<SpriteRenderer>().sprite = nightBG;
        }
    }
}
