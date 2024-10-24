using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public string StartScene;
    private CanvasGroup fadeCanvasGroup;
    private bool isFade;

    private static TransitionManager _instance;
    public static TransitionManager Instance{get => _instance;}

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
        }
        _instance = this;
    }

    void Start()
    {
        fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        Transition(StartScene);
    }

    public void Transition(string sceneName)
    {
        StartCoroutine(Transition2(sceneName));
    }

    private IEnumerator Transition2(string sceneName)
    {
        yield return Fade(1);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
        yield return LoadSceneSetActive(sceneName);
        yield return Fade(0);
    }

    
    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        
        SceneManager.SetActiveScene(newScene);
    }

    
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        float speed = Mathf.Abs(targetAlpha - fadeCanvasGroup.alpha) / 1f;
        while(!Mathf.Approximately(targetAlpha, fadeCanvasGroup.alpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }

}
