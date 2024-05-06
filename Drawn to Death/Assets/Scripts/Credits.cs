using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    public GameObject credits;
    private float scrollSpeed = 100;
    public UnityEngine.UI.Button menuButton;

    private void MenuButton()
    {
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        menuButton.onClick.AddListener(() => { MenuButton(); });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            credits.transform.position += scrollSpeed * 3 * Vector3.up * Time.deltaTime;
        }
    }
}
