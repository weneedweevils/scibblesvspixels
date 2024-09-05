using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene {Splash_Screen, Menu, Intructions, Level_1, Level_2, Level_3, Level_4, Ded, End, Credits}
public class MenuManager : MonoBehaviour, IDataPersistence
{
    [Header("Next Scene")]
    public Scene nextScene;
    public bool newGame = false;
    public bool loadGame = false;
    public float transitionTime;

    [Header("References")]
    public TextMeshProUGUI text;
    public Animator transition;

    public void QuitGame()
    {
        StartCoroutine(LoadScene(nextScene, transition, transitionTime));
        Application.Quit();
    }

    public void GotoScene()
    {
        if (newGame)
        {
            DataPersistenceManager.instance.NewGame();
            nextScene = Scene.Level_1;
        }
        StartCoroutine(LoadScene(nextScene, transition, transitionTime));
    }

    public static IEnumerator LoadScene(Scene scene, Animator transition = null, float transitionTime = 0f)
    {
        if (transition != null)
        {
            transition.gameObject.SetActive(true);
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        SceneManager.LoadScene((int)scene);
    }

    public void LoadData(GameData data)
    {
        if (loadGame)
        {
            nextScene = data.scene;
        }
        return;
    }

    public void SaveData(ref GameData data)
    {
        return;
    }

    public void ShowCredits()
    {
        nextScene = Scene.Credits;
        StartCoroutine(LoadScene(nextScene, transition, transitionTime));
    }

    public void OnHovered()
    {
        text.color = new Color(255,255,255,0.50f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIHover");
    }

    public void OnHoveredExit()
    {
        text.color = new Color(255, 255, 255, 1f);
    }

    public void PlayAcceptSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
    }

    public void PlayLoadSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UILoad");
    }

    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIBack");
    }

}
