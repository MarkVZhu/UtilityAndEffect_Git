using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenu : MonoBehaviour
{
    public static SkillMenu instance;

    public Skill[] skills;
    public SkillButton[] skillButtons;
    public Image[] connerStars;

    public Image skillImage;
    public Text skillStarText;
    public Text skillDesText, skillDesHisText;
    public Text totalStarsText;//MARKER 最高处星星个数
    public Text upgradeText;//MARKER 这一列所有技能统一需要的✨数量

    public Skill activeSkill;

    [SerializeField] private int totalStars;
    public int remainingStars;
    public Sprite normalStarSprite;
    public Sprite upgradeStarSprite;

    public Skill[] arrowSkills;
    public Skill[] soliderSkills;
    public Skill[] boomSkills;
    public Skill[] wizardSkills;

    public GameObject[] arrowStars;
    public GameObject[] soliderStars;
    public GameObject[] boomStars;
    public GameObject[] wizardStars;

    public Text tipText;
    public GameObject upgradeEffect;
    public Transform canvasTrans;

    public Sprite questionSprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skillButtons[i].buttonValue = i;
        }

        remainingStars = totalStars;
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].isUpgraded)
            {
                remainingStars -= skills[i].starsNumber;
            }
        }

        CheckTowersLevel();

    }

    private void Update()
    {
        ShowStars();

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].isUpgraded)
            {
                //colorful
                skillButtons[i].transform.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            }
            else
            {
                //Dark 
                skillButtons[i].transform.GetComponent<Image>().color = new Vector4(0.1f, 0.1f, 0.1f, 1);
            }
        }
    }

    public void CheckTowersLevel()
    {
        CheckTowerStars(arrowSkills, arrowStars);
        CheckTowerStars(soliderSkills, soliderStars);
        CheckTowerStars(boomSkills, boomStars);
        CheckTowerStars(wizardSkills, wizardStars);
    }

    void CheckTowerStars(Skill[] _skills, GameObject[] _stars)//solider
    {
        for(int i = 0; i < _skills.Length; i++)
        {
            if(_skills[i].isUpgraded)
            {
                _stars[i].gameObject.GetComponent<Image>().sprite = upgradeStarSprite;
            }
            else
            {
                _stars[i].gameObject.GetComponent<Image>().sprite = normalStarSprite;
            }
        }
    }

    private void ShowStars()
    {
        totalStarsText.text = "" + remainingStars + "/60";
    }

    public void ResetAll()
    {
        for (int i = 0; i < connerStars.Length; i++)
        {
            connerStars[i].sprite = normalStarSprite;
            remainingStars = totalStars;

            skills[i].isUpgraded = false;
        }

        skillImage.sprite = questionSprite;
        skillStarText.text = "x";
        skillDesText.text = "Select one skill to check";
        skillDesHisText.text = "You can customize your skill now";
        upgradeText.text = "x";

        CheckTowersLevel();

    //public Image skillImage;
    //public Text skillStarText;
    //public Text skillDesText, skillDesHisText;
    //public Text totalStarsText;//MARKER 最高处星星个数
    //public Text upgradeText;//MARKER 这一列所有技能统一需要的✨数量

    }

    public void UpgradeSkill()//MARKER 这只是单一技能的升级，可能出现一个问题，Lv2升级了Lv没有升级//Time 7:13PM
    {
        UpgradePreviousSkill(Skill.SkillType.Arrow, arrowSkills);
        UpgradePreviousSkill(Skill.SkillType.Solider, soliderSkills);
        UpgradePreviousSkill(Skill.SkillType.Boom, boomSkills);
        UpgradePreviousSkill(Skill.SkillType.Wizard, wizardSkills);

        CheckTowersLevel();
        //int totalNumber = 0;

        //if(activeSkill.skillType == Skill.SkillType.Arrow)
        //{
        //    for(int i = 0; i < activeSkill.skillLevel; i++)
        //    {
        //        totalNumber = arrowSkills[i].starsNumber;
        //    }
        //}

        //if (activeSkill != null)
        //{
        //    if (remainingStars >= totalNumber)
        //    {
        //        //Debug.Log("We can Upgrade this skill");
        //        remainingStars -= totalNumber;
        //        for(int i = 0; i < activeSkill.skillLevel; i++)
        //        {
        //            arrowSkills[i].isUpgraded = true;
        //            arrowSkills[i].gameObject.transform.parent.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = upgradeStarSprite;
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("Not Enough Stars");
        //    }
        //}

        //-=-=-=-=-=-=-=-=-=-=-

        //if (activeSkill != null)//OPTIONAL 这是最简单的单个点击可以升级的版本，不包含升级之前的技能
        //{
        //    if (remainingStars >= activeSkill.starsNumber)
        //    {
        //        //Debug.Log("We can Upgrade this skill");
        //        remainingStars -= activeSkill.starsNumber;
        //        activeSkill.isUpgraded = true;

        //        activeSkill.gameObject.transform.parent.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = upgradeStarSprite;
        //    }
        //    else
        //    {
        //        Debug.Log("Not Enough Stars");
        //    }
        //}
    }

    private void UpgradePreviousSkill(Skill.SkillType _skillType, Skill[] _skill)
    {
        int totalNumber = 0;

        if (activeSkill != null && activeSkill.skillType == _skillType && !activeSkill.isUpgraded)
        {
            for (int i = 0; i < activeSkill.skillLevel; i++)
            {
                if (_skill[i].isUpgraded == false)
                {
                    totalNumber += _skill[i].starsNumber;
                }
            }

            if (activeSkill != null)//MARKER 这整个If Statement一开始不是放在这里面，是放出去的，但是如果放出去，按Upgrade会所有的都显示
            {
                if (remainingStars >= totalNumber)
                {
                    //Debug.Log("We can Upgrade this skill");
                    remainingStars -= totalNumber;
                    for (int i = 0; i < activeSkill.skillLevel; i++)
                    {
                        if(!_skill[i].isUpgraded)
                        {
                            _skill[i].isUpgraded = true;
                            _skill[i].gameObject.transform.parent.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = upgradeStarSprite;

                            _skill[i].transform.GetChild(0).gameObject.SetActive(true);//TIME Jul 10 - 2:59 Upgrade Effect
                            _skill[i].transform.GetChild(0).GetComponent<ParticleSystem>().Play();//Each Click will play once
                        }
                    }
                }
                else
                {
                    Debug.Log("Not Enough Stars");//MARKER Make One tip animation now
                    StartCoroutine(ActiveTipTextCo());
                }
            }
        }
    }

    IEnumerator ActiveTipTextCo()
    {
        tipText.GetComponent<Text>().color = new Vector4(0.42f, 0.08f, 0.4f, 1);
        tipText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        tipText.gameObject.SetActive(false);
    }



}
