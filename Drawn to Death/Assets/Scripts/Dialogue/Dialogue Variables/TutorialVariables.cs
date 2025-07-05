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

    [Space(20)]
    public IconMap[] iconMap;
    public Dictionary<string, string> icons;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = CustomInput.instance.playerInput;

        icons = new Dictionary<string, string>();
        foreach (IconMap map in iconMap)
        {
            if (!icons.ContainsKey(map.keyword))
            {
                icons.Add(map.keyword, map.iconName);
            }
        }
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
            variable.text = TryIcon(action.GetBindingDisplayString().ToUpper());
        }
    }

    public string TryIcon(string key)
    {
        if (icons.ContainsKey(key))
        {
            return string.Format("<sprite name=\"{0}\">", icons[key]);
        }
        return key;
    }
}

[System.Serializable]
public class IconMap
{
    public string keyword;
    public string iconName;
}