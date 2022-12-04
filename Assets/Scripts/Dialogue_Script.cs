using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class Dialogue_Script : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public bool ignoreMove = true;
    PlayerControl player;
    private int index;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        textComponent.text = string.Empty;
        StartDialogue();
    }

    //void OnEnable()
    //{
    //    player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    //    textComponent.text = string.Empty;
    //    StartDialogue();
    //}

    public void Reset()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        if (player != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        }
        player.MoveDisable();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //Type each character 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }
    void NextLine()
    {
        if (index < lines.Length -1)
        {
            if(player != null && player.MoveEnabled())
            {
                player.MoveDisable();
            }
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if(!ignoreMove)
            {
                player.MoveEnable();
            }
            gameObject.SetActive(false);
        }
    
    }
}
