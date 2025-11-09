using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillUIManager : MonoBehaviour
{
    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void BackMenuButton()
    {
        sceneFader.SceneTransition("Main");
    }


}
