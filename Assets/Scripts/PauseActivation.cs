using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseActivation : MonoBehaviour
{
    [SerializeField] private Button resume;
    private void Start()
    {
        if (Gamepad.current != null)
        {
            resume.Select();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        {
            if (change == InputDeviceChange.Added && Gamepad.current != null)
            {
                resume.Select();
            }
            else if (change == InputDeviceChange.Removed && Gamepad.current == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
