using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
