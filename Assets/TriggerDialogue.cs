using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField]
    Dialogue_Script dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            dialogue.gameObject.SetActive(true);
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
