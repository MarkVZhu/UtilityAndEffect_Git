using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    //MARKER The UI elements on the right panel
    public Text abilityNameText;
    public Text abilityDesText;
    public Image abilityImage;

    public int buttonId;//The index of the Abilities array

    //VERSION 2.0
    public Sprite defaultSkillSprite;
    public Image LevelIcon;//MARKER right bottom connor the skill level icon

    public GameObject upgradeButton;

    private void Start()
    {
        if(AbilityManager.instance.activeAbility == null)//Defaul information
        {
            abilityNameText.text = "Selected Name";
            abilityDesText.text = "Selected Ability Description Box";
            abilityImage.sprite = defaultSkillSprite;
            LevelIcon.gameObject.SetActive(false);
        }
    }

    private void Update()//Default information
    {
        if(AbilityManager.instance.isRest)
        {
            OriginalStatus();
            Debug.Log("A");
        }
    }

    private void OriginalStatus()//Default information
    {
        abilityNameText.text = "Selected Name";
        abilityDesText.text = "Selected Ability Description Box";
        abilityImage.sprite = defaultSkillSprite;
        LevelIcon.gameObject.SetActive(false);
        AbilityManager.instance.isRest = false;
    }

    //MARKER This method will be called on the OnClick Event
    //MARKER If we press the button, we want to display each ability Information
    //CORE 指定激活的具体技能按钮
    public void PressAbilityButton()
    {
        AbilityManager.instance.activeAbility = transform.GetComponent<Ability>();

        abilityNameText.text = AbilityManager.instance.abilities[buttonId].abilityName;
        abilityDesText.text = AbilityManager.instance.abilities[buttonId].abilityDes;
        abilityImage.sprite = AbilityManager.instance.abilities[buttonId].abilitySprite;
        //Debug.Log(buttonId);

        if (buttonId == 0 || buttonId == 1)//first two ability button
        {
            LevelIcon.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
        }
        else
        {
            LevelIcon.gameObject.SetActive(true);
            upgradeButton.gameObject.SetActive(true);
        }

        //abilityLevelText.text = AbilityManager.instance.abilities[buttonId].level.ToString();
        AbilityManager.instance.DisplaySkillLevel();
    }



}
