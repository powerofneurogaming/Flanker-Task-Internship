using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugMode : MonoBehaviour
{
    [SerializeField]
    GameObject debugUI;

    // Start is called before the first frame update
    void Start()
    {
        if(stateManager.Instance.debug == true)
        {
            debugUI.SetActive(true);
        }
        else
        {
            debugUI.SetActive(false);
        }
    }
}
