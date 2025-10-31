using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;

public class TutorialVariables : MonoBehaviour
{
    private PlayerInput playerInput;

    public DialogueVariable dash;
    public DialogueVariable lifesteal;
    public DialogueVariable melee;
    public DialogueVariable rally;
    public DialogueVariable revive;

    [Space(20)]
    [SerializeField] private TMP_SpriteAsset iconAtlas;

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
        //Debug.Log(dash.text);
        //Debug.Log(melee.text);

    }

    public void AssignVariable(DialogueVariable variable, InputAction action)
    {
        if (action != null && variable != null)
        {
            variable.text = TryIcon(action.GetBindingDisplayString().ToUpper(), GetCurrentDevicePrefix());
        }
    }

    public string TryIcon(string displayString, string device)
    {
        if (device.Equals("Xbox") || device.Equals("PlayStation"))
        {
            //Debug.Log(device);
            string spriteName = device + "_" + displayString;
            //Debug.Log(spriteName);
            if (SpriteExists(spriteName))
            {

                return string.Format("<sprite name=\"{0}\">", spriteName);
            }
           
        }
        return displayString;
    }

    public bool SpriteExists(string spriteName)
    {
        if (iconAtlas == null)
        {
            Debug.LogWarningFormat("TMP Sprite Asset {0} is not assigned.", spriteName);
            return false;
        }

        int index = iconAtlas.GetSpriteIndexFromName(spriteName);
        return index != -1;
    }

    private string GetCurrentDevicePrefix()
    {
        string scheme = playerInput.currentControlScheme;

        if (scheme == null) return "Unknown";

        if (scheme.Contains("Gamepad")) return DetectGamepadType();
        if (scheme.Contains("Keyboard") || scheme.Contains("Mouse")) return "Keyboard&Mouse";

        return "Unknown";
    }

    private string DetectGamepadType()
    {
        foreach (var device in playerInput.devices)
        {
            if (device is UnityEngine.InputSystem.XInput.XInputController) return "Xbox";
            if (device is UnityEngine.InputSystem.DualShock.DualShockGamepad) return "PlayStation";
            if (device is Gamepad) return "Gamepad";
        }
        return "Gamepad";
    }
}