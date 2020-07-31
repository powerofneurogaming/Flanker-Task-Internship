// Unity libraries
using UnityEngine;

// Allows Achievement objects to be serialized
[System.Serializable]

// Class for individual Achievement objects
public class Achievement
{
    // Variables for the Achievement class
    public int state;
    public string friendlyName;
    public string privateName;

    // Achievement constructor
    public Achievement(string pName, string fName)
    {
        // internal achievement name in code, used for PlayerPrefs
        privateName = pName;

        // Currently unused outside of debug output; can be used for achievement
        // pop-ups in future iterations of game
        friendlyName = fName;

        // Dynamically assign achievement state based on private name
        state = PlayerPrefs.GetInt(pName + stateManager.Instance.playerName, 0);
    }
}
