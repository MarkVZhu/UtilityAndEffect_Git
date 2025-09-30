using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MarkFramework;
using UnityEngine;

public class GameDataMgr : BaseManager<GameDataMgr>
{
    private Dictionary<int, Item> itemInfos = new Dictionary<int, Item>();

    public List<ShopCellInfo> shopInfos;

    /// <summary>
    /// 玩家数据
    /// </summary>
    public Player playerInfo;

    //玩家信息存储路径
    private static string PlayerInfo_Url = Application.persistentDataPath + "/PlayerInfo.txt";

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        //加载Resources文件夹下的json文件 获取它的内容
        string info = ResMgr.Instance.Load<TextAsset>("Json/ItemInfo").text;
        Debug.Log(PlayerInfo_Url);
        //根据json文件的内容 解析成对应的数据结构 并存储起来
        Items items = JsonUtility.FromJson<Items>(info);
        Debug.Log(items.info.Count);
        for( int i = 0; i < items.info.Count; ++i )
        {
            itemInfos.Add(items.info[i].id, items.info[i]);
        }

        //初始化 角色信息
        if( File.Exists(PlayerInfo_Url) )
        {
            //读出指定路径的文件的字节数组
            byte[] bytes = File.ReadAllBytes(PlayerInfo_Url);
            //把字节数组转成字符串
            string json = Encoding.UTF8.GetString(bytes);
            //再把字符串转成玩家的数据结构
            playerInfo = JsonUtility.FromJson<Player>(json);

            Debug.Log(playerInfo.name);
        }
        else
        {
            //没有玩家数据时  初始化一个默认数据
            playerInfo = new Player();
            //并且存储它
            SavePlayerInfo();
        }


        //加载Resources文件夹下的json文件 获取它的内容
        string shopInfo = ResMgr.Instance.Load<TextAsset>("Json/ShopInfo").text;
        Debug.Log(shopInfo);
        //根据json文件的内容 解析成对应的数据结构 并存储起来
        Shops shopsInfo = JsonUtility.FromJson<Shops>(shopInfo);
        //记录下 加载解析出来的商店信息
        shopInfos = shopsInfo.info;

        //监听钱改变的事件
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Money_Change, ChangeMoney);
        //监听钱改变的事件
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_Gem_Change, ChangeGem);
    }

    /// <summary>
    /// 事件监听 监听钱是否有变化
    /// </summary>
    /// <param name="money"></param>
    private void ChangeMoney(int money)
    {
        //减钱
        playerInfo.ChangeMoney(money);
        //存储数据
        SavePlayerInfo();
    }

    /// <summary>
    /// 事件监听 监听宝石是否有变化
    /// </summary>
    /// <param name="money"></param>
    private void ChangeGem(int gem)
    {
        //减钱
        playerInfo.ChangeGem(gem);
        //存储数据
        SavePlayerInfo();
    }

    /// <summary>
    /// 保存玩家信息
    /// </summary>
    public void SavePlayerInfo()
    {
        //并且存储它
        string json = JsonUtility.ToJson(playerInfo);
        File.WriteAllBytes(PlayerInfo_Url, Encoding.UTF8.GetBytes(json));
    }

    /// <summary>
    /// 根据道具ID 得到道具的详细信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemInfo(int id)
    {
        if (itemInfos.ContainsKey(id))
            return itemInfos[id];
        return null;
    }

}

/// <summary>
/// 玩家基础信息
/// </summary>
public class Player
{
    public string name;
    public int lev;
    public int money;
    public int gem;
    public int pro;
    public List<ItemInfo> items;
    public List<ItemInfo> equips;
    public List<ItemInfo> gems;
    //当前装备的物品
    public List<ItemInfo> nowEquips;

    public Player()
    {
        name = "唐老湿";
        lev = 1;
        money = 9999;
        gem = 0;
        pro = 99;
        items = new List<ItemInfo>() { new ItemInfo() { id = 3, num = 99 } };
        equips = new List<ItemInfo>() { new ItemInfo() { id = 1, num = 1 }, new ItemInfo() { id = 2, num = 1 } };
        gems = new List<ItemInfo>();

        nowEquips = new List<ItemInfo>();
    }

    /// <summary>
    /// 钱的改变
    /// </summary>
    /// <param name="money"></param>
    public void ChangeMoney(int money)
    {
        //判断钱够不够减  避免减成负数
        if (money < 0 && this.money < money)
            return;
        //玩家的钱
        this.money += money;
    }

    /// <summary>
    /// 宝石的改变
    /// </summary>
    /// <param name="gem"></param>
    public void ChangeGem(int gem)
    {
        //判断宝石够不够减  避免减成负数
        if (gem < 0 && this.gem < gem)
            return;
        //玩家的宝石
        this.gem += gem;
    }

    /// <summary>
    /// 添加物品给玩家
    /// </summary>
    /// <param name="info"></param>
    public void AddItem(ItemInfo info)
    {
        Item item = GameDataMgr.Instance.GetItemInfo(info.id);
        switch(item.type)
        {
            //道具
            case (int)E_Bag_Type.Item:
                items.Add(info);
                break;
            //装备
            case (int)E_Bag_Type.Equip:
                equips.Add(info);
                break;
            //宝石
            case (int)E_Bag_Type.Gem:
                gems.Add(info);
                break;
        }
    }
}

/// <summary>
/// 玩家拥有的道具基础信息
/// </summary>
[System.Serializable]
public class ItemInfo
{
    public int id;
    public int num;
}

/// <summary>
/// 临时结构体 用来表示 道具表信息的数据结构
/// </summary>
public class Items
{
    public List<Item> info;
}

/// <summary>
/// 道具的基础信息 数据结构
/// </summary>
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string icon;
    public int type;
    public int equipType;
    public int price;
    public string tips;
}


/// <summary>
/// 作为json读取的中间数据结构 用来装载json内容
/// </summary>
public class Shops
{
    public List<ShopCellInfo> info;
}

/// <summary>
/// 商店售卖物品信息的数据
/// </summary>
[System.Serializable]
public class ShopCellInfo
{
    public int id;
    public ItemInfo itemInfo;
    public int priceType;
    public int price;
    public string tips;
}