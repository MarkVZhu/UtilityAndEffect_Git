using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

public class ShopCell : BasePanel {

    private ShopCellInfo info;

	// Use this for initialization
	void Start () {
        GetControl<Button>("btnBuy").onClick.AddListener(BuyItem);
	}

    /// <summary>
    /// 买物品的函数处理
    /// </summary>
    private void BuyItem()
    {
        //金币够不够
        if( info.priceType == 1 && 
            GameDataMgr.Instance.playerInfo.money >= info.price )
        {
            //添加物品给玩家
            GameDataMgr.Instance.playerInfo.AddItem(info.itemInfo);
            //事件分发钱改变了
            EventCenter.Instance.EventTrigger(E_EventType.E_Money_Change, -info.price);

            //提示
            TipMgr.Instance.ShowOneBtnTip("购买成功");
        }
        //宝石够不够
        else if (info.priceType == 2 &&
            GameDataMgr.Instance.playerInfo.gem >= info.price)
        {
            //添加物品给玩家
            GameDataMgr.Instance.playerInfo.AddItem(info.itemInfo);
            //事件分发钱改变了
            EventCenter.Instance.EventTrigger(E_EventType.E_Money_Change, -info.price);

            //提示
            TipMgr.Instance.ShowOneBtnTip("购买成功");
        }
        //货币不足
        else
        {
            //提示
            TipMgr.Instance.ShowOneBtnTip("货币不足");
        }
    }

    /// <summary>
    /// 初始化 商店物品 复合控件的显示信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ShopCellInfo info)
    {
        this.info = info;
        Debug.Log("info" + info.id);

        //根据售卖的道具id 得到道具表信息
        Item item = GameDataMgr.Instance.GetItemInfo(info.itemInfo.id);
        //图标
        GetControl<Image>("imgIcon").sprite = ResMgr.Instance.Load<Sprite>("Icon/" + item.icon);
        //个数
        GetControl<Text>("txtNum").text = info.itemInfo.num.ToString();
        //名字
        GetControl<Text>("txtName").text = item.name;
        //价格图标
        GetControl<Image>("imgPType").sprite = ResMgr.Instance.Load<Sprite>("Icon/" + (info.priceType == 1 ? "5": "6"));
        //价格
        GetControl<Text>("txtPrice").text = info.price.ToString();
        //描述信息
        GetControl<Text>("txtTips").text = info.tips;
    }
}
