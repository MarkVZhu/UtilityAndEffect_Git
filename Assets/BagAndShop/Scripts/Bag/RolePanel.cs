using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class RolePanel : BasePanel
{
    public ItemCell itemHead;
    public ItemCell itemNeck;
    public ItemCell itemWeapon;
    public ItemCell itemCloth;
    public ItemCell itemTrousers;
    public ItemCell itemShoes;

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        
        switch(btnName)
        {
            case "btnClose":
                UIManager.Instance.HidePanel("RolePanel");
                break;
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdateRolePanel();
    }

    /// <summary>
    /// 更新角色面板
    /// </summary>
    public void UpdateRolePanel()
    {
        List<ItemInfo> nowEquips = GameDataMgr.Instance.playerInfo.nowEquips;
        Item itemInfo;
        itemHead.InitInfo(null);
        itemNeck.InitInfo(null);
        itemWeapon.InitInfo(null);
        itemCloth.InitInfo(null);
        itemTrousers.InitInfo(null);
        itemShoes.InitInfo(null);

        //更新格子信息 应该显示当前装备的物品
        for ( int i = 0; i < nowEquips.Count; ++i  )
        {
            itemInfo = GameDataMgr.Instance.GetItemInfo(nowEquips[i].id);
            //根据装备类型 判断应该更新哪一个格子
            switch (itemInfo.equipType)
            {
                case (int)E_Item_Type.Head:
                    itemHead.InitInfo(nowEquips[i]);
                    break;
                case (int)E_Item_Type.Neck:
                    itemNeck.InitInfo(nowEquips[i]);
                    break;
                case (int)E_Item_Type.Weapon:
                    itemWeapon.InitInfo(nowEquips[i]);
                    break;
                case (int)E_Item_Type.Cloth:
                    itemCloth.InitInfo(nowEquips[i]);
                    break;
                case (int)E_Item_Type.Trousers:
                    itemTrousers.InitInfo(nowEquips[i]);
                    break;
                case (int)E_Item_Type.Shoes:
                    itemShoes.InitInfo(nowEquips[i]);
                    break;
            }
        }

    }
}
