// Unity libraries
using UnityEngine;
using UnityEngine.UI;

// Handles shop UI, buying items
public class ShopInterface : MonoBehaviour
{
    // AudioSource, etc for sound effects
    AudioSource sfxSource;
    public AudioClip typekey;
    public AudioClip register;

    // Buttons for time trial mode unlockable
    [SerializeField]
    GameObject timeButton;
    [SerializeField]
    GameObject timeInfo;

    // Buttons for endless mode unlockable
    [SerializeField]
    GameObject endlessButton;
    [SerializeField]
    GameObject endlessInfo;

    // Buttons for nighttime BG unlockable
    [SerializeField]
    GameObject nightButton;
    [SerializeField]
    GameObject nightInfo;

    // Buttons for long fuse consumable
    [SerializeField]
    GameObject fuseButton;
    [SerializeField]
    GameObject fuseInfo;

    // Buttons for stopwatch consumable
    [SerializeField]
    GameObject watchButton;
    [SerializeField]
    GameObject watchInfo;

    // Buttons for helping hand consumable
    [SerializeField]
    GameObject gloveButton;
    [SerializeField]
    GameObject gloveInfo;

    // Buttons for good luck kiss consumable
    [SerializeField]
    GameObject kissButton;
    [SerializeField]
    GameObject kissInfo;

    // Item info panel objects
    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    GameObject infoName;
    [SerializeField]
    GameObject infoText;
    [SerializeField]
    GameObject infoSprite;

    // Arrays for displaying correct item info
    [SerializeField]
    float[] spriteSizes;
    [SerializeField]
    string[] itemNames;
    [SerializeField]
    string[] itemBlurbs;
    [SerializeField]
    Sprite[] itemSprites;

    // Item prices
    [SerializeField]
    int[] price;

    // Set up shop screen
    void Start()
    {
        // Connect AudioSource to sound manager
        sfxSource = SoundManager.Instance.audioSource;

        // Hide info panel until user clicks info button
        infoPanel.SetActive(false);

        // Rest of this function disables UI buttons if items are unavailable or user cannot afford
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

    // Shop button disabler
    public void buttonDisabler(GameObject button, int disableType)
    {
        // If button is already disabled, return
        if(button.GetComponent<Button>().interactable == false)
        {
            return;
        }

        button.GetComponent<Button>().interactable = false;

        // Set text depending on why button was disabled
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

    // Set up and show info panel when opened
    public void showInfo(int which)
    {
        sfxSource.PlayOneShot(typekey, stateManager.Instance.volume);

        infoPanel.SetActive(true);
        infoName.GetComponent<Text>().text = itemNames[which];
        infoText.GetComponent<Text>().text = itemBlurbs[which];
        infoSprite.transform.localScale = new Vector2(spriteSizes[which], spriteSizes[which]);
        infoSprite.GetComponent<SpriteRenderer>().sprite = itemSprites[which];
    }

    // Hide info panel when closed
    public void hideInfo()
    {
        sfxSource.PlayOneShot(typekey, stateManager.Instance.volume);

        infoPanel.SetActive(false);
    }

    // Star adder for debug button
    public void giveStars()
    {
        stateManager.Instance.addStars(25);
        stateManager.Instance.saveStars();
    }

    // Purchasing logic for Time Trial
    public void buyTT()
    {
        // If Time Trial is already purchased or somehow this function is run when you cannot afford it, do nothing
        if (stateManager.Instance.timeTrial == true || stateManager.Instance.getStars() < price[0])
        {
            return;
        }

        // Play cash register sound
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);

        // Subtract stars, disable button, and unlock item
        stateManager.Instance.removeStars(price[0]);
        stateManager.Instance.timeTrial = true;
        buttonDisabler(timeButton, 0);
        PlayerPrefs.SetInt("timeTrial_" + stateManager.Instance.playerName, true ? 1 : 0);

        // Update all shop item buttons in case you can't afford them now
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

    // Same as Time Trial but for Endless Mode
    public void buyEM()
    {
        if (stateManager.Instance.endlessMode == true || stateManager.Instance.getStars() < price[1])
        {
            return;
        }
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
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

    // Same as Time Trial but for night BG
    public void buyNight()
    {
        if (stateManager.Instance.nightPurchased == true || stateManager.Instance.getStars() < price[2])
        {
            return;
        }

        // This item requires setting two booleans - one for the item being purchased and one for it being enabled in settings
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
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

    // Same as Time Trial but for fuses
    public void buyFuse()
    {
        if (stateManager.Instance.longFuse >= 3 || stateManager.Instance.getStars() < price[3])
        {
            return;
        }

        // Item is a consumable, not an unlockable, so the button is not disabled by default
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
        stateManager.Instance.removeStars(price[3]);
        stateManager.Instance.longFuse++;
        if (stateManager.Instance.longFuse >= 3)
        {
            buttonDisabler(fuseButton, 1);
        }
        PlayerPrefs.SetInt("longFuse_" + stateManager.Instance.playerName, stateManager.Instance.longFuse);

        // It still might be disabled due to lack of stars, however
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

    // Same as buyFuse
    public void buyStopwatch()
    {
        if (stateManager.Instance.stopwatch >= 3 || stateManager.Instance.getStars() < price[4])
        {
            return;
        }
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
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

    // Same as buyFuse
    public void buyGloves()
    {
        if (stateManager.Instance.goodGloves >= 3 || stateManager.Instance.getStars() < price[5])
        {
            return;
        }
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
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

    // Same as buyFuse
    public void buyKiss()
    {
        if (stateManager.Instance.goodLuckKiss >= 3 || stateManager.Instance.getStars() < price[6])
        {
            return;
        }
        sfxSource.PlayOneShot(register, stateManager.Instance.volume);
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
}