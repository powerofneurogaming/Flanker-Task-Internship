// Unity libraries
using UnityEngine;

// Single-fire script for activating nighttime background
public class backgroundSwitcher : MonoBehaviour
{
    // In-game background
    [SerializeField]
    GameObject background;

    // Nighttime backgrund sprite
    [SerializeField]
    Sprite nightBG;

    // If night mode is enabled, set nighttime background
    void Start()
    {
        if(stateManager.Instance.nightMode == true)
        {
            background.GetComponent<SpriteRenderer>().sprite = nightBG;
        }
    }
}
