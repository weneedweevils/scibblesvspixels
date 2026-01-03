using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialVariables : MonoBehaviour
{
    private PlayerInput playerInput;

    public DialogueVariable dash;
    public DialogueVariable lifesteal;
    public DialogueVariable melee;
    public DialogueVariable rally;
    public DialogueVariable revive;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = CustomInput.instance.playerInput;
    }

    public void Update()
    {
        AssignVariable(dash, playerInput.actions["Dash"]);
        AssignVariable(lifesteal, playerInput.actions["LifeSteal"]);
        AssignVariable(melee, playerInput.actions["Attack"]);
        AssignVariable(rally, playerInput.actions["Rally"]);
        AssignVariable(revive, playerInput.actions["Revive"]);
    }

    public void AssignVariable(DialogueVariable variable, InputAction action)
    {
        if (action != null && variable != null)
        {
            variable.text = action.GetBindingDisplayString().ToUpper();
        }
    }
}
