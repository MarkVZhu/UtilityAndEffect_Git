using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image blackImage;
    private float alpha;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void SceneTransition(string _sceneName)
    {
        StartCoroutine(FadeOut(_sceneName));
    }

    IEnumerator FadeIn()
    {
        alpha = 1;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime;
            blackImage.color = new Vector4(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut(string _sceneName)
    { 
        alpha = 0;
        while(alpha < 1)
        {
            alpha += Time.deltaTime;
            blackImage.color = new Vector4(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(_sceneName);
    }
}
