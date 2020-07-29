using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxes : MonoBehaviour
{
    [SerializeField]
    GameObject fuseCount;

    [SerializeField]
    GameObject watchCount;

    [SerializeField]
    GameObject handCount;

    [SerializeField]
    GameObject lipsCount;

    [SerializeField]
    Sprite[] num;

    // Update is called once per frame
    void Update()
    {
        fuseCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.longFuse];
        watchCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.stopwatch];
        handCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodGloves];
        lipsCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodLuckKiss];
    }
}
