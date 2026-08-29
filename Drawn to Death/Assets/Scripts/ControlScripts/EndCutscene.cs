using UnityEngine;
using UnityEngine.Events;
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
    [HideInInspector] public SpriteRenderer playerSprite;
    public Vector2 playerFloatSpeed;
    public float playerRotationSpeed;
    public float playerScaleSpeed;
    public float playerfadeSpeed;

    [Space(20)]
    public GameObject oodler;
    [HideInInspector] public SpriteRenderer oodlerSprite;
    public Vector2 oodlerFloatSpeed;
    public float oodlerRotationSpeed;
    public float oodlerScaleSpeed;
    public float oodlerFadeSpeed;

    [Space(20)]
    public bool actorsFloating = false;

    [Header("Misc")]
    public UnityEvent onCutsceneSkip;

    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(true);
        menuButton.SetActive(false);

        playerSprite = player.GetComponent<SpriteRenderer>();
        oodlerSprite = oodler.GetComponentInChildren<SpriteRenderer>();
    }

    public void SkipCutscene()
    {
        /*
        GameObject dialogueBox = DialogueManager.instance.GetCurrentDialogue().gameObject;
        if (dialogueBox != null)
            Destroy(dialogueBox);
        DialogueManager.instance.SetCurrentDialogueNull();
        DialogueManager.instance.dialogueActive = false;
        */

        onCutsceneSkip?.Invoke();

        actorsFloating = true;

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
        if (actorsFloating)
        {
            // Adjust translation
            player.transform.position += new Vector3(playerFloatSpeed.x * Time.deltaTime, playerFloatSpeed.y * Time.deltaTime, 0);
            oodler.transform.position += new Vector3(oodlerFloatSpeed.x * Time.deltaTime, oodlerFloatSpeed.y * Time.deltaTime, 0);

            // Adjust rotation
            player.transform.Rotate(Vector3.forward, playerRotationSpeed * Time.deltaTime);
            oodler.transform.Rotate(Vector3.forward, oodlerRotationSpeed * Time.deltaTime);

            // Adjust scale
            float delta = playerScaleSpeed * Time.deltaTime;
            player.transform.localScale = new Vector3(
                Mathf.Max(0, player.transform.localScale.x + delta),
                Mathf.Max(0, player.transform.localScale.y + delta), 
                player.transform.localScale.z
            );

            delta = oodlerScaleSpeed * Time.deltaTime;
            oodler.transform.localScale = new Vector3(
                Mathf.Max(0, oodler.transform.localScale.x + delta),
                Mathf.Max(0, oodler.transform.localScale.y + delta), 
                oodler.transform.localScale.z
                );

            // Adjust alpha
            MyUtils.SetAlpha(playerSprite, Mathf.Clamp01(playerSprite.color.a + playerfadeSpeed * Time.deltaTime));
            MyUtils.SetAlpha(oodlerSprite, Mathf.Clamp01(oodlerSprite.color.a + playerfadeSpeed * Time.deltaTime));
        }

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

        
    }

    public void StartFloatingActors()
    {
        actorsFloating = true;
    }
}
