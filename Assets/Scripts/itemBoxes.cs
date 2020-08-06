// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;

// Item boxes for consumables so player knows what items they have
public class itemBoxes : MonoBehaviour
{
    // Long Fuse item box
    [SerializeField]
    GameObject fuseCount;
    [SerializeField]
    GameObject fusePanel;
    [SerializeField]
    GameObject fuseItem;

    // Stopwatch item box
    [SerializeField]
    GameObject watchCount;
    [SerializeField]
    GameObject watchPanel;
    [SerializeField]
    GameObject watchItem;

    // Helping Hand item box
    [SerializeField]
    GameObject handCount;
    [SerializeField]
    GameObject handPanel;
    [SerializeField]
    GameObject handItem;

    // Good Luck Kiss item box
    [SerializeField]
    GameObject lipsCount;
    [SerializeField]
    GameObject lipsPanel;
    [SerializeField]
    GameObject lipsItem;

    // Number sprites
    [SerializeField]
    Sprite[] num;

    // Set up item boxes depending on scene
    private void Start()
    {
        fuseCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.longFuse];
        watchCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.stopwatch];
        handCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodGloves];
        lipsCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodLuckKiss];

        // Highlight active item boxes used in Classic Mode
        if (SceneManager.GetActiveScene().name == "Classic Select")
        {
            if (stateManager.Instance.goodLuckKiss > 0)
            {
                lipsPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                lipsCount.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }

        // Highlight active item boxes used in Time Trial Mode
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

        // Highlight active item boxes used in Endless Mode
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

        // Highlight active item boxes for current game
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

        // Highlight active item boxes for any game/mode
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

    // Update boxes as items are gained/lost
    void Update()
    {
        // Fuses are consumed in the course of an Endless game; the count should update to reflect this
        if (stateManager.Instance.gameMode == 2 && SceneManager.GetActiveScene().name == "Flanker Main")
        {
            fuseCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.longFuse];
        }

        // Gloves only apply to one difficulty mode in Endless; it should dynamically highlight based on user activity
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

        // Items should dynamically highlight and update numbers when new ones are purchased
        if(SceneManager.GetActiveScene().name == "Shop")
        {
            fuseCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.longFuse];
            watchCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.stopwatch];
            handCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodGloves];
            lipsCount.GetComponent<SpriteRenderer>().sprite = num[stateManager.Instance.goodLuckKiss];

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
