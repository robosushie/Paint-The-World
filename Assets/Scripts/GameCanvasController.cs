using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GameCanvasController : MonoBehaviour
{
    public InputActionReference xPress;
    public InputActionReference yPress;

    public GameObject colorCanvas;
    public GameObject mainCanvas;

    // Start is called before the first frame update
    void Start()
    {
        colorCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        xPress.action.started += ButtonPressed;
        yPress.action.started += ButtonPressed2;
    }

    private void ButtonPressed2(InputAction.CallbackContext obj)
    {
        colorCanvas.SetActive(false);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }

    private void ButtonPressed(InputAction.CallbackContext obj)
    {
        mainCanvas.SetActive(false);
        colorCanvas.SetActive(!colorCanvas.activeSelf);
    }

}
