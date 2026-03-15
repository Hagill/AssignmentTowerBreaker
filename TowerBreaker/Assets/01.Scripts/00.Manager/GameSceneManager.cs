using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ConstValue;

public class GameSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Player player;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image firstSkillCooldownImage;
    [SerializeField] private Image secondSkillCooldownImage;
    [SerializeField] private Image thirdSkillCooldownImage;
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

    private void Update()
    {
        if (player != null)
        {
            firstSkillCooldownImage.fillAmount = 1f- (player.CurrentFirstSkillCooldown / player.FirstSkillCooldown);
            secondSkillCooldownImage.fillAmount = 1f - (player.CurrentSecondSkillCooldown / player.SecondSkillCooldown);
            thirdSkillCooldownImage.fillAmount = 1f - (player.CurrentThirdSkillCooldown / player.ThirdSkillCooldown);
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
