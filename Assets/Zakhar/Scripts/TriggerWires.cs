using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWires : MonoBehaviour
{
    public GameObject WireCanvas;

    private void Awake() 
    {
        WireCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            WireCanvas.SetActive(true);
        }
    }
}
