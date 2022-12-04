using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [SerializeField]
    GameObject dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        Debug.Log(!dialogue == null);
        if (!(dialogue == null))
        {
            dialogue.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
