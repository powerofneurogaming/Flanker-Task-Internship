using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : MonoBehaviour
{
    [SerializeField]
    GameObject timeButton;
    
    [SerializeField]
    GameObject timeInfo;

    [SerializeField]
    GameObject endlessButton;

    [SerializeField]
    GameObject endlessInfo;

    [SerializeField]
    GameObject nightButton;

    [SerializeField]
    GameObject nightInfo;

    [SerializeField]
    GameObject handButton;

    [SerializeField]
    GameObject handInfo;

    [SerializeField]
    GameObject fuseButton;

    [SerializeField]
    GameObject fuseInfo;

    [SerializeField]
    GameObject watchButton;

    [SerializeField]
    GameObject watchInfo;

    [SerializeField]
    GameObject gloveButton;

    [SerializeField]
    GameObject gloveInfo;

    [SerializeField]
    GameObject kissButton;

    [SerializeField]
    GameObject kissInfo;

    [SerializeField]
    GameObject infoPanel;

    [SerializeField]
    GameObject infoName;

    [SerializeField]
    GameObject infoText;

    [SerializeField]
    string[] itemNames;

    [SerializeField]
    string[] itemBlurbs;

    [SerializeField]
    Sprite[] itemSprites;

    // Start is called before the first frame update
    void Start()
    {
        infoPanel.SetActive(false);

        if (stateManager.Instance.timeTrial == true)
        {
            buttonDisabler(timeButton, false);
        }
        if (stateManager.Instance.endlessMode == true)
        {
            buttonDisabler(endlessButton, false);
        }
        if (stateManager.Instance.nightPurchased == true)
        {
            buttonDisabler(nightButton, false);
        }
        if (stateManager.Instance.handPurchased == true)
        {
            buttonDisabler(handButton, false);
        }
        if(stateManager.Instance.longFuse >= 3)
        {
            buttonDisabler(fuseButton, true);
        }
        if(stateManager.Instance.stopwatch >= 3)
        {
            buttonDisabler(watchButton, true);
        }
        if(stateManager.Instance.goodGloves >= 3)
        {
            buttonDisabler(gloveButton, true);
        }
        if(stateManager.Instance.goodLuckKiss >= 3)
        {
            buttonDisabler(kissButton, true);
        }
    }

    public void buttonDisabler(GameObject button, bool consumable)
    {
        button.GetComponent<Button>().interactable = false;
        if (consumable == false)
        {
            button.GetComponentInChildren<Text>().fontSize = 9;
            button.GetComponentInChildren<Text>().text = "PURCHASED";
        }
        else
        {
            button.GetComponentInChildren<Text>().fontSize = 7;
            button.GetComponentInChildren<Text>().text = "OUT OF STOCK";
        }
    }

    public void showInfo(int which)
    {
        infoPanel.SetActive(true);
        infoName.GetComponent<Text>().text = itemNames[which];
        infoText.GetComponent<Text>().text = itemBlurbs[which];
    }

    public void hideInfo()
    {
        infoPanel.SetActive(false);
    }

    public void buyTT(int price)
    {
        if (stateManager.Instance.timeTrial == true)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.timeTrial = true;
        buttonDisabler(timeButton, false);
        PlayerPrefs.SetInt("timeTrial_" + stateManager.Instance.playerName, true ? 1 : 0);
    }

    public void buyEM(int price)
    {
        if (stateManager.Instance.endlessMode == true)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.endlessMode = true;
        buttonDisabler(endlessButton, false);
        PlayerPrefs.SetInt("endlessMode_" + stateManager.Instance.playerName, true ? 1 : 0);
    }

    public void buyNight(int price)
    {
        if (stateManager.Instance.nightPurchased == true)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.nightPurchased = true;
        stateManager.Instance.nightMode = true;
        buttonDisabler(nightButton, false);
        PlayerPrefs.SetInt("nightPurchased_" + stateManager.Instance.playerName, true ? 1 : 0);
        PlayerPrefs.SetInt("nightMode_" + stateManager.Instance.playerName, true ? 1 : 0);
    }

    public void buyHand(int price)
    {
        if (stateManager.Instance.handPurchased == true)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.handPurchased = true;
        stateManager.Instance.oldHand = true;
        buttonDisabler(handButton, false);
        PlayerPrefs.SetInt("handPurchased_" + stateManager.Instance.playerName, true ? 1 : 0);
        PlayerPrefs.SetInt("oldHand_" + stateManager.Instance.playerName, true ? 1 : 0);
    }

    public void buyFuse(int price)
    {
        if (stateManager.Instance.longFuse >= 3)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.longFuse++;
        buttonDisabler(fuseButton, true);
        PlayerPrefs.SetInt("longFuse_" + stateManager.Instance.playerName, stateManager.Instance.longFuse);
    }

    public void buyStopwatch(int price)
    {
        if (stateManager.Instance.stopwatch >= 3)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.stopwatch++;
        buttonDisabler(watchButton, true);
        PlayerPrefs.SetInt("stopwatch_" + stateManager.Instance.playerName, stateManager.Instance.stopwatch);
    }

    public void buyGloves(int price)
    {
        if (stateManager.Instance.goodGloves >= 3)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.goodGloves++;
        buttonDisabler(gloveButton, true);
        PlayerPrefs.SetInt("goodGloves_" + stateManager.Instance.playerName, stateManager.Instance.goodGloves);
    }

    public void buyKiss(int price)
    {
        if (stateManager.Instance.goodLuckKiss >= 3)
        {
            return;
        }
        stateManager.Instance.removeStars(price);
        stateManager.Instance.goodLuckKiss++;
        buttonDisabler(kissButton, true);
        PlayerPrefs.SetInt("goodLuckKiss_" + stateManager.Instance.playerName, stateManager.Instance.goodLuckKiss);
    }
}