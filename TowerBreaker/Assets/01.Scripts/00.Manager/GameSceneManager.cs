using TMPro;
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
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI popupTitleText;

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
        popupTitleText.text = "Pause";

        Color color;
        if (ColorUtility.TryParseHtmlString("#494BCF", out color))
        {
            popupTitleText.color = color;
        }
    }

    public void OnGameContinueClicked()
    {
        popupPanel.SetActive(false);
        gameManager.GameContinue();
    }

    public void OnMainMenuClicked()
    {
        gameManager.GameExit();
        SceneChangeManager.ChangeScene(MainMenuScene);
    }

    public void ShowGameOverPopup()
    {
        popupPanel.SetActive(true);
        continueButton.gameObject.SetActive(false);
        popupTitleText.text = "GameOver";
        popupTitleText.color = Color.red;
    }

    public void ChangeHp(float currentHp)
    {
        hpText.text = $"Hp : {Mathf.RoundToInt(currentHp)}";
    }
}
