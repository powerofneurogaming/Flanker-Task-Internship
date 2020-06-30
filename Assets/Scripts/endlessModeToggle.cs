using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endlessModeToggle : MonoBehaviour
{
    public Toggle endlessToggle;
    public InputField levelSelect;

    public static bool endlessMode;

    // Start is called before the first frame update
    void Start()
    {
        endlessMode = false;
    }

    public void onToggle()
    {
        if (endlessToggle.isOn)
        {
            endlessMode = true;
            levelSelect.interactable = false;
        }
        else
        {
            endlessMode = false;
            levelSelect.interactable = true;
        }
    }
}
