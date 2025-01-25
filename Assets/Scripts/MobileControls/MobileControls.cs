using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;
    List<ClickTracker> buttons = new List<ClickTracker>();

    public static MobileControls Manager;

    void Awake()
    {
        //Assign this script to static variable, so it can be accessed from other scripts. Make sure there is only one MobileControls in the Scene.
        Manager = this;

        canvas = GetComponent<Canvas>();
    }

    public int AddButton(ClickTracker button)
    {
        buttons.Add(button);

        return buttons.Count - 1;
    }

    public Vector2 GetJoystick(string joystickName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == joystickName)
            {
                return buttons[i].GetInputAxis();
            }
        }

        Debug.LogError("Joystick " + joystickName + " not found.");

        return Vector2.zero;
    }

    public bool GetJoystickUp(string joystickName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == joystickName)
            {
                return buttons[i].GetAxisUpDown();
            }
        }

        Debug.LogError("Joystick " + joystickName + " not found.");

        return false;
    }
    public bool GetJoystickDown(string joystickName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == joystickName)
            {
                return buttons[i].GetAxisDownDown();
            }
        }

        Debug.LogError("Joystick " + joystickName + " not found.");

        return false;
    }
    public bool GetJoystickRight(string joystickName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == joystickName)
            {
                return buttons[i].GetAxisRightDown();
            }
        }

        Debug.LogError("Joystick " + joystickName + " not found.");

        return false;
    }
    public bool GetJoystickLeft(string joystickName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == joystickName)
            {
                return buttons[i].GetAxisLeftDown();
            }
        }

        Debug.LogError("Joystick " + joystickName + " not found.");

        return false;
    }


    public bool GetMobileButton(string buttonName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == buttonName)
            {
                return buttons[i].GetHoldStatus();
            }
        }

        Debug.LogError("Button " + buttonName + " not found.");

        return false;
    }

    public bool GetMobileButtonDown(string buttonName)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].buttonName == buttonName)
            {
                return buttons[i].GetClickedStatus();
            }
        }

        Debug.LogError("Button " + buttonName + " not found.");

        return false;
    }
}