using UnityEngine;
[System.Serializable]

// Class for storing Achievements
public class Achievement
{
    // Variables for the Question class
    public int state;
    public string friendlyName;
    public string privateName;

    public Achievement(string pName, string fName)
    {
        privateName = pName;
        friendlyName = fName;
        state = PlayerPrefs.GetInt(pName + SetPrefabs.name, 0);
    }
}
