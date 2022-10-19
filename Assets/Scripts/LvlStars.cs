using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlStars : MonoBehaviour
{
    [SerializeField]
    int levelIndex;
    [SerializeField]
    GameObject[] starsImage;
    int stars;
    // Start is called before the first frame update
    void Start()
    {
        stars = PlayerPrefs.GetInt("Level" + levelIndex, 0);
        CoverStars();
    }

    void CoverStars()
    {
        for(int i = 3; i > stars; i--)
        {
            Destroy(starsImage[Mathf.Abs(i-1)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
