using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AnimationCurve curve;
    public Text countText;
    public GameObject joyStick;
    public GameObject winPanel;
    public GameObject fadePanel;
    public Image fadeOutImage;
    public GameObject completeText;

    float time = 0;

    private void Start() 
    {
        GameManager.instance.EnemyKillCount = 0;
    }
    
    private void LateUpdate() 
    {
        Count();
    }

    public void Count()
    {
        countText.text = GameManager.instance.EnemyKillCount + " / " + GameManager.instance.EnemyCount;
        
        if (GameManager.instance.EnemyKillCount == GameManager.instance.EnemyCount)
        {
            time += Time.deltaTime;
            winPanel.SetActive(true);
            OnWinPanel(time);
            GameManager.instance.timeLine.SetActive(true);
        }
    }

    private void OnWinPanel(float time)
    {
        float size = 2f;
        float upSizeTime = 0.5f;

        if (time <= upSizeTime)
            completeText.transform.localScale = Vector3.one * (1 * size * time);
        else
            completeText.transform.localScale = Vector3.one;
    }

    public void OnClickNext()
    {
        fadePanel.SetActive(true);
        StartCoroutine(Fade(0,1));
        Invoke(nameof(SceneLoad), 1f);
    }

    private void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0.3f;
        float percent = 0f;

        while (percent < 1f)
        {
            time += Time.deltaTime;
            percent = time;

            Color color = fadeOutImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeOutImage.color = color;

            yield return null;
        }
    } 
}
