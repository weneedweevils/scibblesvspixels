 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene {Splash_Screen, Menu, Intructions, Level_1, Level_2, Level_3, Ded, End, Credits}
public class MenuManager : MonoBehaviour, IDataPersistence
{
    [Header("Next Scene")]
    public Scene nextScene;
    public bool newGame = false;
    public bool loadGame = false;

    [Header("Slide Animation")]
    public float slideDistance = 16f;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 direction;
    private bool hover;

    [Header("References")]
    public TextMeshProUGUI text;
    public Button button;

    private void Start()
    {
        // Initialize positional variables
        originalPosition = text.rectTransform.localPosition;
        targetPosition = originalPosition;
        direction = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (hover)
        {
            targetPosition = originalPosition + slideDistance * Vector3.right;
        }
        else
        {
            targetPosition = originalPosition;
        }

        if (button.interactable)
        {
            // Calculate the direction to move towards target position + some momentum
            direction = 0.9f * direction + 0.9f * (targetPosition - text.rectTransform.localPosition);
            text.rectTransform.localPosition += direction * Time.deltaTime;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GotoScene()
    {
        if (newGame)
        {
            DataPersistenceManager.instance.NewGame();
            nextScene = Scene.Level_1;
        }
        GotoScene(nextScene);
    }

    public static void GotoScene(Scene scene)
    {
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
        GotoScene(nextScene);
    }

    public void OnHovered()
    {
        text.color = new Color(255,255,255,0.50f);
        hover = true;
    }

    public void OnHoveredExit()
    {
        text.color = new Color(255, 255, 255, 1f);
        hover = false;
    }
}
