using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public Sprite abilitySprite;
    //public int abilityId;

    [TextArea(1, 3)]
    public string abilityDes;
    public bool isUpgrade;

    public Ability[] previousAbility;


    //VERSION 2.0
    public int level;
    // public int requireLevel;
    // public int maxLevel;

}
