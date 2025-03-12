using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager
{
    private static DataManager instance = new DataManager();

    public static DataManager Instance => instance;

    private InputInfo inputInfo;

    public InputInfo InputInfo => inputInfo;

    private string jsonStr;

    private DataManager()
    {
        inputInfo = new InputInfo();

        jsonStr = Resources.Load<TextAsset>("InputAssetSample").text;
    }

    public InputActionAsset GetActionAsset()
    {
        //上键
        string str = jsonStr.Replace("<up>", inputInfo.up);
        //下
        str = str.Replace("<down>", inputInfo.down);
        //左
        str = str.Replace("<left>", inputInfo.left);
        //右
        str = str.Replace("<right>", inputInfo.right);
        //开火
        str = str.Replace("<fire>", inputInfo.fire);
        //跳跃
        str = str.Replace("<jump>", inputInfo.jump);

        return InputActionAsset.FromJson(str);
    }
}
