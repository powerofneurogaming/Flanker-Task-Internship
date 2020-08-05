using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    // 'Pseudo-singleton' - uses static instance of GameManager to allow for access to GameManager object
    // !! IS NOT PERSISTENT ACROSS MULTIPLE SCENES !!
    public static AchievementPopup Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    SpriteRenderer medal;

    [SerializeField]
    Text achText;

    [SerializeField]
    Text achTier;

    [SerializeField]
    Sprite[] medals;

    [SerializeField]
    string[] levels;

    [SerializeField]
    GameObject achievePanel;

    Animator achAnim;

    private void Start()
    {
        achAnim = achievePanel.GetComponent<Animator>();
        achAnim.StopPlayback();
        if (AchievementManager.Instance.achTrigger.Count != 0)
        {
            StartCoroutine(achPop());
        }
    }

    public IEnumerator achPop()
    {       
        while(AchievementManager.Instance.achTrigger.Count > 0)
        {
            medal.sprite = medals[AchievementManager.Instance.achTrigger[0].state];
            achText.text = AchievementManager.Instance.achTrigger[0].friendlyName;
            achTier.text = levels[AchievementManager.Instance.achTrigger[0].state];
            achAnim.Play("slide-in");
            achAnim.StopPlayback();
            yield return new WaitForSeconds(3.0f);
            achAnim.Play("slide-out");
            yield return new WaitForSeconds(1.0f);
            achAnim.StopPlayback();
            AchievementManager.Instance.achTrigger.RemoveAt(0);
        }
    }
}
