using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

/// <summary>
/// 道具装备详细信息面板
/// </summary>
public class TipsPanel : BasePanel
{
    /// <summary>
    /// 初始化tips面板信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ItemInfo info)
    {
        //根据道具信息的数据 来更新格子对象
        Item itemData = GameDataMgr.Instance.GetItemInfo(info.id);
        //使用我们的道具表中的数据
        //图标
        //通过 道具ID得到的 道具表中的 数据信息  就可以得到对应的道具ID 用的图标是什么 加载它来用 即可
        GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>("Icon/" + itemData.icon);
        //数量
        GetControl<Text>("txtNum").text = "数量：" + info.num.ToString();
        //名字
        GetControl<Text>("txtName").text = itemData.name;
        //描述
        GetControl<Text>("txtTips").text = itemData.tips;
    }
}
