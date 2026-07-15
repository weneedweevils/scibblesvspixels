using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndCutscene : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject menuButton;
    public GameObject credits;
    public Button skipButton;
    public float scrollSpeed;
    private bool scrollCredits = false;
    public float scrollStopPosition = 320f;

    [Header("Input")]
    public PlayerInput playerInput;

    [Header("Actors")]
    public GameObject player;
    public Vector2 playerFloatSpeed;
    public float playerRotationSpeed;
    public GameObject oodler;
    public Vector2 oodlerFloatSpeed;
    public float oodlerRotationSpeed;
    public bool actorsFloating = false;

    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(true);
        menuButton.SetActive(false);
    }

    public void SkipCutscene()
    {
        // TBD
        StartCreditsScroll();
    }

    public void StartCreditsScroll()
    {
        skipButton.gameObject.SetActive(false);
        menuButton.SetActive(true);
        scrollCredits = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scrollCredits)
        {
            if (credits.gameObject.transform.GetChild(0).transform.position.y >= scrollStopPosition)
            {
                scrollSpeed = 0;
                return;
            }
            credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;

            if (playerInput.actions["ScrollFaster"].IsPressed())
            {
                credits.transform.position += scrollSpeed * 3 * Vector3.up * Time.deltaTime;
            }
        }

        if (actorsFloating)
        {
            player.transform.position += new Vector3(playerFloatSpeed.x * Time.deltaTime, playerFloatSpeed.y * Time.deltaTime, 0);
            oodler.transform.position += new Vector3(oodlerFloatSpeed.x * Time.deltaTime, oodlerFloatSpeed.y * Time.deltaTime, 0);

            player.transform.Rotate(Vector3.forward, playerRotationSpeed * Time.deltaTime);
            oodler.transform.Rotate(Vector3.forward, oodlerRotationSpeed * Time.deltaTime);
        }
    }

    public void StartFloatingActors()
    {
        actorsFloating = true;
    }
}
