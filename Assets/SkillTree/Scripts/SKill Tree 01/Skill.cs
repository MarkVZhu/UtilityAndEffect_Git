using UnityEngine;

public class Skill : MonoBehaviour
{
    public Sprite skillSprite;//技能图
    public int starsNumber;//技能需要消耗的技能点

    [TextArea(1,3)]
    public string skillDes;//技能描述
    public string skillHistory;//技能背景介绍
    public int skillLevel;//技能等级

    public bool isUpgraded;//技能是否解锁/升级

    public enum SkillType//技能类枚举 - 比如远程，近战，炸药类，法术类
    {
        Arrow,
        Solider,
        Boom,
        Wizard
    }

    public SkillType skillType;//技能类

}


