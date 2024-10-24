using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    public GameObject Level_Pre;
    public int LevelNum;
    public Transform levelContainer;
    void Start()
    {
        for(int i = 1;i<=LevelNum; i++)
        {
            LevelChoose level = Instantiate(Level_Pre, levelContainer).GetComponent<LevelChoose>();
            level.Init(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
