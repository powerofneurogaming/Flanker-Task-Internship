// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Star counter in the corner of the screen
public class starCounter : MonoBehaviour
{
    [SerializeField]
    Text starText;

    // Continually update star counter with current star count
    private void Update()
    {
        starText.text = stateManager.Instance.getStars().ToString();
    }
}
