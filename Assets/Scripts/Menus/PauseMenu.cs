using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pause;
    [SerializeField] private GameObject playerHudGo, pauseMenuGo;
    GameObject[] lasers, Enemislasers;
    private bool isGamePaused;
    [SerializeField]
    private Button quitBtn, resumeBtn;

    private void Start()
    {
        quitBtn.onClick.AddListener(QuitGame);
        resumeBtn.onClick.AddListener(PauseTheGame);
    }

    private void OnEnable()
    {
        pause.action.started += PauseGame;
    }

    private void OnDisable()
    {
        pause.action.started -= PauseGame;
    }
    public void PauseGame(InputAction.CallbackContext obj)
    {
        PauseTheGame();
    }
    private void PauseTheGame()
    {
        if (!isGamePaused)
        {
            PlayerMovements.instance.SetPaused(true);
            lasers = GameObject.FindGameObjectsWithTag("Projectile");
            Enemislasers = GameObject.FindGameObjectsWithTag("EnemyProjectile");

            foreach (GameObject laser in lasers)
            {
                laser.SetActive(false);
            }
            foreach (GameObject laser in Enemislasers)
            {
                laser.SetActive(false);
            }

            isGamePaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            playerHudGo.SetActive(false);
            pauseMenuGo.SetActive(true);
        }
        else
        {
            PlayerMovements.instance.SetPaused(false);

            foreach (GameObject laser in lasers)
            {
                laser.SetActive(true);
            }
            foreach (GameObject laser in Enemislasers)
            {
                laser.SetActive(true);
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerHudGo.SetActive(true);
            Time.timeScale = 1;
            pauseMenuGo.SetActive(false);
            isGamePaused = false;
        }
    }

    private void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
