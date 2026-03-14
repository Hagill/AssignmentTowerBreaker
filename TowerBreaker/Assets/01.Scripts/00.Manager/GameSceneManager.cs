using UnityEngine;
using UnityEngine.UI;
using static ConstValue;

public class GameSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject popupPanel;

    private void Awake()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameStartWithWaiting();

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnGamePauseClicked);
        }

        if(continueButton != null)
        {
            continueButton.onClick.AddListener(OnGameContinueClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }
    }

    public void OnGamePauseClicked()
    {
        gameManager.GamePause();
        popupPanel.SetActive(true);
    }

    public void OnGameContinueClicked()
    {
        popupPanel.SetActive(false);
        gameManager.GameContinue();
    }

    public void OnMainMenuClicked()
    {
        SceneChangeManager.ChangeScene(MainMenuScene);
    }
}
