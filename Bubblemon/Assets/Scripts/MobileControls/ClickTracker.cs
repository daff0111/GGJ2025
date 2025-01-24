using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClickTracker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public string buttonName = "";
    public bool isJoystick = false;
    public float movementLimit = 1;
    public float movementThreshold = 0.1f;

    //Reference variables
    RectTransform rt;
    Vector3 startPos;
    Vector2 clickPos;

    //Input variables
    Vector2 inputAxis = Vector2.zero;
    bool isHolding = false;
    bool isClicked = false;

    void Start()
    {
        //Add this button to the list
        MobileControls.Manager.AddButton(this);

        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition3D;
    }
    void Update()
    {
        if (isJoystick && !isHolding)
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");
            //Simulate Joystick movement with Input axis
            rt.anchoredPosition = startPos + (new Vector3(inputAxis.x, inputAxis.y, 0) * (rt.sizeDelta.x / 2));
        }

    }

    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;

        if (!isJoystick)
        {
            isClicked = true;
            StartCoroutine(StopClickEvent());
        }
        else
        {
            //Initialize Joystick movement
            clickPos = eventData.pressPosition;
        }
    }

    WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    //Wait for next update then release the click event
    IEnumerator StopClickEvent()
    {
        yield return waitForEndOfFrame;

        isClicked = false;
    }

    //Joystick movement
    public void OnDrag(PointerEventData eventData)
    {
        if (isJoystick)
        {
            Vector3 movementVector = Vector3.ClampMagnitude((eventData.position - clickPos) / MobileControls.Manager.canvas.scaleFactor, (rt.sizeDelta.x * movementLimit) + (rt.sizeDelta.x * movementThreshold));
            Vector3 movePos = startPos + movementVector;
            rt.anchoredPosition = movePos;
            //Update inputAxis
            float inputX = 0;
            float inputY = 0;
            if (Mathf.Abs(movementVector.x) > rt.sizeDelta.x * movementThreshold)
            {
                inputX = (movementVector.x - (rt.sizeDelta.x * movementThreshold * (movementVector.x > 0 ? 1 : -1))) / (rt.sizeDelta.x * movementLimit);
            }
            if (Mathf.Abs(movementVector.y) > rt.sizeDelta.x * movementThreshold)
            {
                inputY = (movementVector.y - (rt.sizeDelta.x * movementThreshold * (movementVector.y > 0 ? 1 : -1))) / (rt.sizeDelta.x * movementLimit);
            }
            inputAxis = new Vector2(inputX, inputY);
        }
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;

        if (isJoystick)
        {
            //Reset Joystick position
            rt.anchoredPosition = startPos;
            inputAxis = Vector2.zero;
        }
    }

    public Vector2 GetInputAxis()
    {
        return inputAxis;
    }

    public bool GetClickedStatus()
    {
        return isClicked;
    }

    public bool GetHoldStatus()
    {
        return isHolding;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ClickTracker))]
public class ClickTracker_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        ClickTracker script = (ClickTracker)target;

        script.buttonName = EditorGUILayout.TextField("Button Name", script.buttonName);
        script.isJoystick = EditorGUILayout.Toggle("Is Joystick", script.isJoystick);
        if (script.isJoystick)
        {
            script.movementLimit = EditorGUILayout.FloatField("Movement Limit", script.movementLimit);
            script.movementThreshold = EditorGUILayout.FloatField("Movement Threshold", script.movementThreshold);
        }
    }
}
#endif