using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : MonoBehaviour
{
    AudioSource sfxSource;
    float volume;

    // Typewriter sound
    public AudioClip typekey;
    public AudioClip register;

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
    GameObject infoSprite;

    [SerializeField]
    float[] spriteSizes;

    [SerializeField]
    string[] itemNames;

    [SerializeField]
    string[] itemBlurbs;

    [SerializeField]
    Sprite[] itemSprites;

    [SerializeField]
    int[] price;

    // Start is called before the first frame update
    void Start()
    {
        sfxSource = SoundManager.Instance.audioSource;
        volume = 0.5f;

        infoPanel.SetActive(false);

        if (stateManager.Instance.timeTrial == true)
        {
            buttonDisabler(timeButton, 0);
        }
        else if(stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.endlessMode == true)
        {
            buttonDisabler(endlessButton, 0);
        }
        else if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.nightPurchased == true)
        {
            buttonDisabler(nightButton, 0);
        }
        else if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.longFuse >= 3)
        {
            buttonDisabler(fuseButton, 1);
        }
        else if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.stopwatch >= 3)
        {
            buttonDisabler(watchButton, 1);
        }
        else if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.goodGloves >= 3)
        {
            buttonDisabler(gloveButton, 1);
        }
        else if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.goodLuckKiss >= 3)
        {
            buttonDisabler(kissButton, 1);
        }
        else if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }
    }

    public void buttonDisabler(GameObject button, int disableType)
    {
        if(button.GetComponent<Button>().interactable == false)
        {
            return;
        }

        button.GetComponent<Button>().interactable = false;
        if (disableType == 0)
        {
            button.GetComponentInChildren<Text>().fontSize = 9;
            button.GetComponentInChildren<Text>().text = "PURCHASED";
        }
        else if (disableType == 1)
        {
            button.GetComponentInChildren<Text>().fontSize = 7;
            button.GetComponentInChildren<Text>().text = "OUT OF\nSTOCK";
        }
        else
        {
            button.GetComponentInChildren<Text>().fontSize = 7;
            button.GetComponentInChildren<Text>().text = "NOT ENOUGH\nSTARS";
        }
    }

    public void showInfo(int which)
    {
        sfxSource.PlayOneShot(typekey, volume);

        infoPanel.SetActive(true);
        infoName.GetComponent<Text>().text = itemNames[which];
        infoText.GetComponent<Text>().text = itemBlurbs[which];
        infoSprite.transform.localScale = new Vector2(spriteSizes[which], spriteSizes[which]);
        infoSprite.GetComponent<SpriteRenderer>().sprite = itemSprites[which];
    }

    public void hideInfo()
    {
        sfxSource.PlayOneShot(typekey, volume);

        infoPanel.SetActive(false);
    }

    public void buyTT()
    {
        if (stateManager.Instance.timeTrial == true || stateManager.Instance.getStars() < price[0])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[0]);
        stateManager.Instance.timeTrial = true;
        buttonDisabler(timeButton, 0);
        PlayerPrefs.SetInt("timeTrial_" + stateManager.Instance.playerName, true ? 1 : 0);

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyEM()
    {
        if (stateManager.Instance.endlessMode == true || stateManager.Instance.getStars() < price[1])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[1]);
        stateManager.Instance.endlessMode = true;
        buttonDisabler(endlessButton, 0);
        PlayerPrefs.SetInt("endlessMode_" + stateManager.Instance.playerName, true ? 1 : 0);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyNight()
    {
        if (stateManager.Instance.nightPurchased == true || stateManager.Instance.getStars() < price[2])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[2]);
        stateManager.Instance.nightPurchased = true;
        stateManager.Instance.nightMode = true;
        buttonDisabler(nightButton, 0);
        PlayerPrefs.SetInt("nightPurchased_" + stateManager.Instance.playerName, true ? 1 : 0);
        PlayerPrefs.SetInt("nightMode_" + stateManager.Instance.playerName, true ? 1 : 0);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyFuse()
    {
        if (stateManager.Instance.longFuse >= 3 || stateManager.Instance.getStars() < price[3])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[3]);
        stateManager.Instance.longFuse++;
        if (stateManager.Instance.longFuse >= 3)
        {
            buttonDisabler(fuseButton, 1);
        }
        PlayerPrefs.SetInt("longFuse_" + stateManager.Instance.playerName, stateManager.Instance.longFuse);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyStopwatch()
    {
        if (stateManager.Instance.stopwatch >= 3 || stateManager.Instance.getStars() < price[4])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[4]);
        stateManager.Instance.stopwatch++;
        if (stateManager.Instance.stopwatch >= 3)
        {
            buttonDisabler(watchButton, 1);
        }
        PlayerPrefs.SetInt("stopwatch_" + stateManager.Instance.playerName, stateManager.Instance.stopwatch);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyGloves()
    {
        if (stateManager.Instance.goodGloves >= 3 || stateManager.Instance.getStars() < price[5])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[5]);
        stateManager.Instance.goodGloves++;
        if (stateManager.Instance.goodGloves >= 3)
        {
            buttonDisabler(gloveButton, 1);
        }
        PlayerPrefs.SetInt("goodGloves_" + stateManager.Instance.playerName, stateManager.Instance.goodGloves);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void buyKiss()
    {
        if (stateManager.Instance.goodLuckKiss >= 3 || stateManager.Instance.getStars() < price[6])
        {
            return;
        }
        sfxSource.PlayOneShot(register, volume);
        stateManager.Instance.removeStars(price[6]);
        stateManager.Instance.goodLuckKiss++;
        if(stateManager.Instance.goodLuckKiss >= 3)
        {
            buttonDisabler(kissButton, 1);
        }
        PlayerPrefs.SetInt("goodLuckKiss_" + stateManager.Instance.playerName, stateManager.Instance.goodLuckKiss);

        if (stateManager.Instance.getStars() < price[0])
        {
            buttonDisabler(timeButton, 2);
        }

        if (stateManager.Instance.getStars() < price[1])
        {
            buttonDisabler(endlessButton, 2);
        }

        if (stateManager.Instance.getStars() < price[2])
        {
            buttonDisabler(nightButton, 2);
        }

        if (stateManager.Instance.getStars() < price[3])
        {
            buttonDisabler(fuseButton, 2);
        }

        if (stateManager.Instance.getStars() < price[4])
        {
            buttonDisabler(watchButton, 2);
        }

        if (stateManager.Instance.getStars() < price[5])
        {
            buttonDisabler(gloveButton, 2);
        }

        if (stateManager.Instance.getStars() < price[6])
        {
            buttonDisabler(kissButton, 2);
        }

        stateManager.Instance.saveStars();
    }

    public void giveStars()
    {
        stateManager.Instance.addStars(25);
        stateManager.Instance.saveStars();
    }
}