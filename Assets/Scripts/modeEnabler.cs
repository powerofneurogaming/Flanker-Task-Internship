using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modeEnabler : MonoBehaviour
{
    [SerializeField]
    GameObject ttMode;

    [SerializeField]
    GameObject endlessMode;

    // Start is called before the first frame update
    void Start()
    {
        if (stateManager.Instance.timeTrial == false)
        {
            ttMode.GetComponent<Button>().interactable = false;
            ttMode.GetComponentInChildren<Text>().text = "LOCKED";
        }
        if (stateManager.Instance.endlessMode == false)
        {
            endlessMode.GetComponent<Button>().interactable = false;
            endlessMode.GetComponentInChildren<Text>().text = "LOCKED";
        }
    }
}
