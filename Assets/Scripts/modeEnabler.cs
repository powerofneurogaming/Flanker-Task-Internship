// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Handles locking of gamemodes if not yet purchased
public class modeEnabler : MonoBehaviour
{
    [SerializeField]
    GameObject ttMode;

    [SerializeField]
    GameObject endlessMode;

    // Lock modes if not purchased
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
