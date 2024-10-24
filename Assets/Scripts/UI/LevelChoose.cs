using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoose : MonoBehaviour
{
    [SerializeField]private string targetSceneName;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClicked);
    }

    void Start()
    {
        
    }
    
    public void Init(int targetLevel)
    {
        string sceneName = "Level_" + targetLevel;
        targetSceneName = sceneName;
    }

    private void OnClicked()
    {
        TransitionManager.Instance.Transition(targetSceneName);
    }
}
