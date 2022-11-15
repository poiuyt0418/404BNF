using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldExit : MonoBehaviour
{
    public TMP_Text objText;
    public PlayerControl player;
    [SerializeField]
    int levelSelectIndex; // go by name?
    [SerializeField]
    string[] exitRequirement;
    [SerializeField]
    float waitTime = 3;
    WaitForSecondsRealtime waitForSecondsRealtime;
    [SerializeField]
    float timeForLevel;
    float timer;
    int minutes, seconds;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeForLevel;
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        int missing = 0;
        foreach(string part in exitRequirement)
        {
            if (!player.CheckPart(part))
            {
                Debug.Log("Missing a " + part);
                missing++;
            }
        }
        if (missing > 0)
        {
            MusicControlScript.Instance.PlayError();
            return;
        }
        objText.text = "You have escaped.";
        Debug.Log(timer / (float)timeForLevel);
        int stars = Mathf.Clamp((int)(timer / (float)timeForLevel * 3 + 1), 0, 3); // 3 + ((timer > 0) ? 0 : -1) + Object.FindObjectsOfType<WorldQTEManager>()[0].Stars();
        DataManager.gameData.AddStars(levelSelectIndex,stars);
        timer = 0;
        StartCoroutine(DelayedExit());

    }

    private IEnumerator DelayedExit()
    {
        if (waitForSecondsRealtime == null)
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(waitTime);
        }
        else
        {
            waitForSecondsRealtime.waitTime = waitTime;
        }

        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        yield return waitForSecondsRealtime;
        Time.timeScale = prevTimeScale;
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if(timer > 1)
        {
            timer -= Time.deltaTime;
            minutes = (int)timer / 60;
            seconds = (int)timer % 60;
            objText.text = minutes + ":" + seconds.ToString("D2");
        } else
        {
            timer = 0;
        }
    }
}
