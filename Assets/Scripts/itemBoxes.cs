using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class itemBoxes : MonoBehaviour
{
    [SerializeField]
    GameObject fuseCount;
    [SerializeField]
    GameObject fusePanel;
    [SerializeField]
    GameObject fuseItem;

    [SerializeField]
    GameObject watchCount;
    [SerializeField]
    GameObject watchPanel;
    [SerializeField]
    GameObject watchItem;

    [SerializeField]
    GameObject handCount;
    [SerializeField]
    GameObject handPanel;
    [SerializeField]
    GameObject handItem;

    [SerializeField]
    GameObject lipsCount;
    [SerializeField]
    GameObject lipsPanel;
    [SerializeField]
    GameObject lipsItem;

    [SerializeField]
    Sprite[] num;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Classic Select")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Time Select")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.stopwatch > 0)
            {
                watchPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Endless Select")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.longFuse > 0)
            {
                fusePanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.difficulty == 0 && stateManager.Instance.goodGloves > 0)
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Flanker Main")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.gameMode == 1 && stateManager.Instance.stopwatch > 0)
            {
                watchPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.gameMode == 2 && stateManager.Instance.longFuse > 0)
            {
                fusePanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.gameMode == 2 && stateManager.Instance.difficulty == 0 && stateManager.Instance.goodGloves > 0)
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.stopwatch > 0)
            {
                watchPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.longFuse > 0)
            {
                fusePanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.goodGloves > 0)
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        fuseCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.longFuse];
        watchCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.stopwatch];
        handCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodGloves];
        lipsCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodLuckKiss];

        if (SceneManager.GetActiveScene().name == "Endless Select")
        {
            if (stateManager.Instance.difficulty == 0 && stateManager.Instance.goodGloves > 0)
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
            }
        }

        if(SceneManager.GetActiveScene().name == "Shop")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.stopwatch > 0)
            {
                watchPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                watchCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.longFuse > 0)
            {
                fusePanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                fuseCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            if (stateManager.Instance.goodGloves > 0)
            {
                handPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                handCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }
}
