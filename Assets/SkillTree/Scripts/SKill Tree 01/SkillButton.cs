using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillImage;
    public Text skillStarsText;
    public Text skillDesText;
    public Text skillDesHisText;

    public int buttonValue;

    public Sprite upgradeSprite;
    public Sprite normalSprite;

    public GameObject upgradeButton;//MARKER 如果已经升级，显示Own，没有升级，显示Upgrade按钮
    public GameObject ownedText;//MARKER 如果已经升级，显示Own，没有升级，显示Upgrade按钮

    //public Text upgradeText;//MARKER 这一列所有技能统一需要的✨数量

    private void Start()
    {
        upgradeButton.gameObject.SetActive(true);
        ownedText.gameObject.SetActive(false);

        for(int i = 0; i < SkillMenu.instance.skills.Length; i++)
        {
            if (SkillMenu.instance.skills[i].isUpgraded)
            {
                SkillMenu.instance.connerStars[i].sprite = upgradeSprite;
            }
            else
            {
                SkillMenu.instance.connerStars[i].sprite = normalSprite;
            }
        }
    }

    public void PressSkillButton()
    {
        showSkillInfo();
    }

    public void showSkillInfo()//MARKER 点击单个技能按钮，显示具体信息
    {
        SkillMenu.instance.activeSkill = gameObject.GetComponent<Skill>();//MARKER 将这个Button中的Skill信息赋值给SkillMenu脚本中的activeSkill，激活
        skillImage.sprite = SkillMenu.instance.skills[buttonValue].skillSprite;
        skillStarsText.text = SkillMenu.instance.skills[buttonValue].starsNumber.ToString();
        skillDesText.text = SkillMenu.instance.skills[buttonValue].skillDes;
        skillDesHisText.text = SkillMenu.instance.skills[buttonValue].skillHistory;

        ShowUpgradeText(Skill.SkillType.Solider, SkillMenu.instance.soliderSkills);
        ShowUpgradeText(Skill.SkillType.Arrow, SkillMenu.instance.arrowSkills);
        ShowUpgradeText(Skill.SkillType.Boom, SkillMenu.instance.boomSkills);
        ShowUpgradeText(Skill.SkillType.Wizard, SkillMenu.instance.wizardSkills);

        CheckStarIcon();
    }

    //CORE 显示这一Tree点击当前总✨数//Time 6:40 PM
    //MARKER 先写一个无参数的，在进行修改
    public void ShowUpgradeText(Skill.SkillType _skillType,Skill[] _skills)
    {
        if (SkillMenu.instance.activeSkill.skillType == _skillType)//PARA
        {
            int number;
            int totalNumber = 0;

            for (int i = 0; i < SkillMenu.instance.activeSkill.skillLevel; i++)
            {
                //Debug.Log("----" + SkillMenu.instance.soliderSkills[i].isUpgraded);

                //if (SkillMenu.instance.soliderSkills[i].isUpgraded == false)//OPTIONAL PARA
                if(_skills[i].isUpgraded == false)
                {
                    //number = SkillMenu.instance.soliderSkills[i].starsNumber;//OPTIONAL Single PARA
                    number = _skills[i].starsNumber;//PARA
                }
                else
                {
                    number = 0;
                }

                totalNumber += number;
            }

            SkillMenu.instance.upgradeText.text = totalNumber.ToString();
        }
    }

    public void CheckStarIcon()//MARKER 检查技能升级后右上角✨显示情况
    {
        if (SkillMenu.instance.activeSkill != null)
        {
            if (SkillMenu.instance.activeSkill.isUpgraded)
            {
                ownedText.gameObject.SetActive(true);
                upgradeButton.gameObject.SetActive(false);
            }
            else
            {
                ownedText.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(true);
            }
        }
    }

}
