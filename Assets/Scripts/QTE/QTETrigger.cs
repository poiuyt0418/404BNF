using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    public QTEScriptableObject qte;
    public WorldQTEManager manager;
    public string partType;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
            manager = Object.FindObjectsOfType<WorldQTEManager>()[0];
    }

    void OnTriggerEnter()
    {
        manager.TriggerQTE(qte, partType);
        Destroy(gameObject);
        TriggerEffect();
    }

    void TriggerEffect()
    {
        //add some sort of animation cue?
        //gameObject->action
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
