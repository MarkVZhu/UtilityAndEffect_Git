using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

public class BagMainPanel : BasePanel
{
	// Use this for initialization
	void Start () {
        GetControl<Button>("btnRole").onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<BagPanel>("BagPanel");
            UIManager.Instance.ShowPanel<RolePanel>("RolePanel");
        });

        //监听商店按钮事件 点击后 打开商店面板
        GetControl<Button>("btnShop").onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<ShopPanel>("ShopPanel");
        });

        //监听添加金钱按钮
        GetControl<Button>("btnAddMoney").onClick.AddListener(() =>
        {
            EventCenter.Instance.EventTrigger(E_EventType.E_Money_Change, 1000);
        });

        //监听添加宝石按钮
        GetControl<Button>("btnAddGem").onClick.AddListener(() =>
        {
            EventCenter.Instance.EventTrigger(E_EventType.E_Gem_Change, 1000);
        });
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //更新名字 等级 钱等等基础信息
        GetControl<Text>("txtName").text = GameDataMgr.Instance.playerInfo.name;
        GetControl<Text>("txtLev").text = GameDataMgr.Instance.playerInfo.lev.ToString();
        GetControl<Text>("txtMoney").text = GameDataMgr.Instance.playerInfo.money.ToString();
        GetControl<Text>("txtGem").text = GameDataMgr.Instance.playerInfo.gem.ToString();
        GetControl<Text>("txtPro").text = GameDataMgr.Instance.playerInfo.pro.ToString();

        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Money_Change, UpdatePanel);
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Gem_Change, UpdatePanel);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Money_Change, UpdatePanel);
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_Gem_Change, UpdatePanel);
    }

    //当货币发生改变时 用来监听 更新的函数
    private void UpdatePanel(int money)
    {
        GetControl<Text>("txtMoney").text = GameDataMgr.Instance.playerInfo.money.ToString();
        GetControl<Text>("txtGem").text = GameDataMgr.Instance.playerInfo.gem.ToString();
    }
}
