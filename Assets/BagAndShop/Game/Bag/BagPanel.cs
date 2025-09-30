using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包的页签状态枚举
/// </summary>
public enum E_Bag_Type
{
    Item = 1,
    Equip,
    Gem,
}

/// <summary>
/// 背包面板
/// </summary>
public class BagPanel : BasePanel
{

    public Transform content;

    private List<ItemCell> list = new List<ItemCell>();

	// Use this for initialization
	void Start () {
        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("BagPanel");
        });

        //为Toggle添加事件监听 来触发 数据更新

        GetControl<Toggle>("togItem").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("togEquip").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("togGem").onValueChanged.AddListener(ToggleValueChange);

    }

    public override void ShowMe()
    {
        base.ShowMe();
        ChangeType(E_Bag_Type.Item);
    }

    /// <summary>
    /// 当单选框 页签 状态改变时 处理的逻辑函数
    /// </summary>
    /// <param name="value"></param>
    private void ToggleValueChange(bool value)
    {
        if(GetControl<Toggle>("togItem").isOn)
        {
            ChangeType(E_Bag_Type.Item);
        }
        else if (GetControl<Toggle>("togEquip").isOn)
        {
            ChangeType(E_Bag_Type.Equip);
        }
        else
        {
            ChangeType(E_Bag_Type.Gem);
        }
    }

    /// <summary>
    /// 页签切换的函数
    /// </summary>
    /// <param name="type"></param>
    public void ChangeType(E_Bag_Type type)
    {
        //根据传进来的页签信息  来进行数据更新
        //默认值 是道具列表信息
        List<ItemInfo> tempInfo = GameDataMgr.Instance.playerInfo.items;
        switch(type)
        {
            case E_Bag_Type.Equip:
                tempInfo = GameDataMgr.Instance.playerInfo.equips;
                break;
            case E_Bag_Type.Gem:
                tempInfo = GameDataMgr.Instance.playerInfo.gems;
                break;
        }

        //更新sv的内容
        //先删除之前的格子
        for( int i = 0; i < list.Count; ++i )
            Destroy(list[i].gameObject);
        list.Clear();
        //再更新现在的数据
        //动态创建 ItemCell 预设体 并且把他存到list里面  方便下一次更新的时候 删除
        for( int i = 0; i < tempInfo.Count; ++i )
        {
            //实例化预设体 并且得到身上的ItemCell脚本
            ItemCell cell = ResMgr.Instance.Load<GameObject>("UI/ItemCell").GetComponent<ItemCell>();
            //设置它的父对象
            cell.transform.parent = content;
            //初始化数据
            cell.InitInfo(tempInfo[i]);
            //把他存进list
            list.Add(cell);
        }
    }

}
