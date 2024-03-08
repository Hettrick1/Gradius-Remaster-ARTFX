using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FinalScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private GameObject highScoreGO;
    [SerializeField] private Button retryBtn;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(Gamepad.current != null)
        {
            retryBtn.Select();
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
                retryBtn.Select();
            }
            else if (change == InputDeviceChange.Removed && Gamepad.current == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public void DisplayScore()
    {
        scoreGO.SetActive(true);
    }
    public void DisplayHighScore()
    {
        highScoreGO.SetActive(true);
    }
}
