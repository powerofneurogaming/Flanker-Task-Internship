using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starCounter : MonoBehaviour
{
    [SerializeField]
    Text starText;

    // Start is called before the first frame update
    void Start()
    {
        starText.text = stateManager.Instance.getStars().ToString();
    }

    private void Update()
    {
        starText.text = stateManager.Instance.getStars().ToString();
    }
}
