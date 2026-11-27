using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EndCutscene : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject menuButton;
    public GameObject credits;
    private RectTransform creditsTransform;
    public Button skipButton;
    public Button skipCredits;
    public float scrollSpeed;
    private bool scrollCredits = false;
    public float scrollStopPosition = 600f;
    public GameObject lastDialogueTrigger;

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
    private bool skippedCutscene = false;

    [Header("Misc")]
    public UnityEvent onCutsceneSkip;

    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(true);
        menuButton.SetActive(false);
        skipCredits.gameObject.SetActive(false);

        playerSprite = player.GetComponent<SpriteRenderer>();
        oodlerSprite = oodler.GetComponentInChildren<SpriteRenderer>();
        playerInput = CustomInput.instance.playerInput;
        creditsTransform = credits.GetComponent<RectTransform>();
        

       

    }

    /// <summary>
    /// Skip the cutscene
    /// </summary>
    public void SkipCutscene()
    {
        onCutsceneSkip?.Invoke();
        actorsFloating = true;
        skippedCutscene = true;
    }

    public void SkipCredits()
    {
        creditsTransform.anchoredPosition= new Vector2(creditsTransform.anchoredPosition.x,creditsTransform.rect.height + Screen.height);
        skipCredits.gameObject.SetActive(false);

    }

    

    /// <summary>
    /// Start the scrolling credits sequence
    /// </summary>
    public void StartCreditsScroll()
    {
    
        // Activate menu button
        menuButton.SetActive(true);
        skipCredits.gameObject.SetActive(true);
        // enable credit scroll
        scrollCredits = true;

        BasicMusicScript.instance.setIntensity(30);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(actorsFloating){
            // Adjust rotation
            player.transform.Rotate(Vector3.forward, playerRotationSpeed * Time.deltaTime);
            oodler.transform.Rotate(Vector3.forward, oodlerRotationSpeed * Time.deltaTime);

            if (lastDialogueTrigger.activeSelf||skippedCutscene) // and all dialogue is done
            {
                // Disable skip cutscene button
                skipButton.gameObject.SetActive(false);
                // Adjust translation
                player.transform.position += new Vector3(playerFloatSpeed.x * Time.deltaTime, playerFloatSpeed.y * Time.deltaTime, 0);
                oodler.transform.position += new Vector3(oodlerFloatSpeed.x * Time.deltaTime, oodlerFloatSpeed.y * Time.deltaTime, 0);

             

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
                //Debug.Log(DialogueManager.instance);

                if ((DialogueManager.instance.GetCurrentDialogue()==null || skippedCutscene) && player.transform.position.x < -11f && !scrollCredits) 
                {
                    StartCreditsScroll();
                    
                }
            
            }

            if (scrollCredits)
            {
                playerInput.SwitchCurrentActionMap("UI");
                 
                // Check if end of credits has been reched
                if (creditsTransform.anchoredPosition.y < creditsTransform.rect.height + Screen.height)
                {
                    Debug.Log("Credits are scrolling");
                    // Scroll credits
                    credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
                }
                else
                {
                    SkipCredits();
                }
               
            }
        }
    }

    /// <summary>
    /// Enable floating actors
    /// </summary>
    public void StartFloatingActors()
    {
        actorsFloating = true;
    }
}
