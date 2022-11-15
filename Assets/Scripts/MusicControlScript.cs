using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MusicControlScript : MonoBehaviour
{
    public static MusicControlScript instance;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip qteSuccess;

    MusicControlScript()
    {
        
    }
    
    public static MusicControlScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MusicControlScript();
            }
            return instance;
        }
    }

    public void PlayQTE()
    {
        audioSource.clip = qteSuccess;
        audioSource.pitch = 2;
        audioSource.Play();
    }

    public void PlayError()
    {
        audioSource.clip = qteSuccess;
        audioSource.pitch = 1;
        audioSource.Play();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
