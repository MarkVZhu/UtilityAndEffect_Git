using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReadExcelForSkillTree : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        BinaryDataMgr.Instance.InitData<SkillInfoContainer, SkillInfo>();

        SkillInfoContainer data = BinaryDataMgr.Instance.GetTable<SkillInfoContainer>();
        print(data.dataDic[5].skillDes);
    }

}
