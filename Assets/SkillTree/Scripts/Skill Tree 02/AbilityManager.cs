using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using DG.Tweening;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Ability[] abilities;
    public AbilityButton[] abilityButtons;

    //public AbilityButton activeButton;//MARKER The button you press 
    public Ability activeAbility;

    public int totalPoints;
    public int remainPoints;
    public Text pointsText;


    public Sprite normalSprite;
    public Sprite upgradedSprite;

    //VERSION 2.0
    [Header("Each Ab button Level Text")]
    public Text[] abilityLevelTexts;  
    [Header("Ability Level UI Text on the right")]
    public Text abilityLevelText;

    public bool isRest;//Default is false

    public GameObject NotEnoughHintGo;
    public GameObject preUpgradeGo;
    public GameObject CombinationGo;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        remainPoints = totalPoints;

        UpdateAbilityImage();
        DisplayPoints();
        DisplaySkillLevel();

        NotEnoughHintGo.gameObject.SetActive(false);
        preUpgradeGo.gameObject.SetActive(false);
        CombinationGo.gameObject.SetActive(false);
    }

    public void DisplayPoints()//MARKER called this method at the beginning or PressUpgrade button
    {
        pointsText.text = remainPoints + "/15";
    }

    /// <summary>
    /// VERSION 2.0 We hopes each press will increase the Skill Level
    /// </summary>
    public void PressUpgrade()
    {
        //if (!activeAbility.isUpgrade && remainPoints >= 1)
        //{
        //    for (int i = 0; i < activeAbility.previousAbility.Length; i++)
        //    {
        //        //TODO I consider the first two abilities as default
        //        //if(activeAbility.previousAbility.Length == 0)
        //        //{
        //        //    remainPoints -= 1;
        //        //    activeAbility.isUpgrade = true;
        //        //}

        //        if (activeAbility.previousAbility[i].isUpgrade)//MARKER except first two skills
        //        {
        //            remainPoints -= 1;
        //            activeAbility.isUpgrade = true;
        //            activeAbility.level++;
        //        }
        //        else
        //        {
        //            Debug.Log("You should upgrade several basic abilities first");
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.Log("Not enough Skill Points");
        //}

        for(int i = 0; i < activeAbility.previousAbility.Length; i++)
        {
            if (activeAbility.previousAbility[i].isUpgrade && remainPoints > 0)
            {
                remainPoints -= 1;
                activeAbility.isUpgrade = true;
                activeAbility.level++;//MARKER ADD one level
                break;
            }

            //MARKER HAVE to drag outside 
            //if (!activeAbility.previousAbility[i].isUpgrade)
            //{
            //    Debug.Log("Upgrade upper abilities first");
            //    StartCoroutine(ShowHintCo(preUpgradeGo));
            //}
        }

        //if (remainPoints<= 0)
        //{
        //    Debug.Log("Not Enough points Now!");
        //    StartCoroutine(ShowHintCo(NotEnoughHintGo));
        //}

        //CORE MAKE the Hint function
        int upgradeNumber = 0;

        for(int i = 0; i < activeAbility.previousAbility.Length; i++)
        {
            if(activeAbility.previousAbility[i].isUpgrade)
            {
                Debug.Log("---CAN UPGRADE---");
            }
            else
            {
                upgradeNumber++;
                //Debug.Log("Upgrade upper abilities first");
                //StartCoroutine(ShowHintCo(preUpgradeGo));
            }
        }

        //if( upgradeNumber == activeAbility.previousAbility.Length)//MARKER Make sure your all previous abilities are not upgraded
        //{
        //    Debug.Log("Upgrade upper abilities first.");
        //    StartCoroutine(ShowHintCo(preUpgradeGo));
        //}
        //CORE MAKE the Hint function
        if(activeAbility.previousAbility.Length == 0) {
            return;
        } else if(upgradeNumber == activeAbility.previousAbility.Length && remainPoints <= 0)
        {
            Debug.Log("Match two conditions");
            StartCoroutine(ShowHintCo(CombinationGo));
        } else if (remainPoints <= 0)
        {
            Debug.Log("Not Enough points Now!");
            StartCoroutine(ShowHintCo(NotEnoughHintGo));
        } else if (upgradeNumber == activeAbility.previousAbility.Length)//MARKER Make sure your all previous abilities are not upgraded
        {
            Debug.Log("Upgrade upper abilities first.");
            StartCoroutine(ShowHintCo(preUpgradeGo));
        }

        DisplayPoints();
        UpdateAbilityImage();
        DisplaySkillLevel();

        if(activeAbility != null)
        {
            //activeAbility.transform.DOPunchPosition(new Vector3(0, -20, 0), 0.2f, 4, 0.5f);
            //activeAbility.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }
        else
        {
            return;
        }

    }

    /// <summary>
    /// VERSION 1.0 Press the upgrade button only upgrade once and no Level increase
    /// </summary>
    //public void PressUpgrade()
    //{
    //    if(!activeAbility.isUpgrade && remainPoints >= 1)
    //    {
    //        for(int i = 0; i < activeAbility.previousAbility.Length; i++)
    //        {
    //            //TODO I consider the first two abilities as default
    //            //if(activeAbility.previousAbility.Length == 0)
    //            //{
    //            //    remainPoints -= 1;
    //            //    activeAbility.isUpgrade = true;
    //            //}

    //            if (activeAbility.previousAbility[i].isUpgrade)//MARKER except first two skills
    //            {
    //                remainPoints -= 1;
    //                activeAbility.isUpgrade = true;
    //                activeAbility.level++;
    //            }
    //            else
    //            {
    //                Debug.Log("You should upgrade several basic abilities first");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Not enough Skill Points");
    //    }

    //    DisplayPoints();
    //    UpdateAbilityImage();
    //    DisplaySkillLevel();
    //}

    void UpdateAbilityImage()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            if(abilities[i].isUpgrade)
            {
                abilities[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                abilities[i].transform.GetChild(0).GetComponent<Image>().sprite = upgradedSprite;
            }
            else
            {
                abilities[i].GetComponent<Image>().color = new Vector4(0.15f, 0.45f, 0.45f, 1);
                abilities[i].transform.GetChild(0).GetComponent<Image>().sprite = normalSprite;
            }
        }

    }

    public void ResetButton()
    {
        isRest = true;

        activeAbility = null;

        for (int i = 2; i < abilities.Length; i++)
        {
            abilities[i].isUpgrade = false;
            abilities[i].level = 0;
            abilityLevelTexts[i].text = "";
            remainPoints = totalPoints;
        }

        UpdateAbilityImage();
        DisplayPoints();

    }

    public void DisplaySkillLevel()
    {
        if(activeAbility == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    abilityLevelTexts[i].text = "";
                    abilityLevelText.text = "";
                }
                else
                {
                    if (abilities[i].isUpgrade)
                    {
                        //show the Text
                        abilityLevelTexts[i].text = abilities[i].level.ToString();
                        abilityLevelText.text = activeAbility.level.ToString();//VERSION 2.0 UI Level TEXT
                    }
                    else
                    {
                        //Keep the Text Empty
                        abilityLevelTexts[i].text = "";
                    }
                }

            }
        }
    }


    IEnumerator ShowHintCo(GameObject _gameObject)
    {
        _gameObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _gameObject.gameObject.SetActive(false);
    }

}
