// Unity libraries
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementScreen : MonoBehaviour
{
    [SerializeField]
    Sprite[] achievementLevels;

    [SerializeField]
    SpriteRenderer[] medals;

    [SerializeField]
    Text[] secrets;

    int offset;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Achieve P2")
        {
            offset = 6;
            if (AchievementManager.Instance.achievementList[2 + offset].state > 0)
            {
                secrets[0].text = "CLICK WRONG\nHANDS";
            }
            if (AchievementManager.Instance.achievementList[3 + offset].state > 0)
            {
                secrets[1].text = "NO RIGHT\nANSWERS";
            }
            if (AchievementManager.Instance.achievementList[4 + offset].state > 0)
            {
                secrets[2].text = "STAY AT\nRESULTS";
            }
            if (AchievementManager.Instance.achievementList[5 + offset].state > 0)
            {
                secrets[3].text = "GET ALL\nACHIEVEMENTS";
            }
        }

        for(int i = 0; i < 6; ++i)
        {
            medals[i].sprite = achievementLevels[AchievementManager.Instance.achievementList[i + offset].state];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
