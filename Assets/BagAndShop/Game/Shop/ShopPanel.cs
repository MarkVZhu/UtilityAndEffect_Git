using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarkFramework;

public class ShopPanel : BasePanel {

    public Transform content;

	// Use this for initialization
	void Start () {
        //得到关闭按钮 监听点击事件 点击后 关闭自己 商店面板 即可
        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("ShopPanel");
        });
	}

    public override void ShowMe()
    {
        base.ShowMe();
        //显示商店面板时 初始化 商店面板中的售卖信息
        //根据 商店数据 来进行初始化
        for( int i = 0; i < GameDataMgr.Instance.shopInfos.Count; ++i )
        {
            //实例化一个 商店物品信息UI组合控件
            ShopCell cell = ResMgr.Instance.Load<GameObject>("UI/ShopCell").GetComponent<ShopCell>();
            //设置父对象
            cell.transform.parent = content;
            //设置相对缩放大小 避免显示出错
            cell.transform.localScale = Vector3.one;
            //传商店售卖物品信息给它 让它进行更新
            cell.InitInfo(GameDataMgr.Instance.shopInfos[i]);
        }
    }

}
