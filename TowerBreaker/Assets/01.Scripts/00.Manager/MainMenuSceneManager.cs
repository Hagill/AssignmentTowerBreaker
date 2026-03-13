using UnityEngine;
using UnityEngine.UI;
using static ConstValue;

public class MainMenuSceneManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button gameButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeInventoryButton;

    [Header("인벤토리 팝업")]
    [SerializeField] private GameObject inventoryUI;

    void Start()
    {
        if (gameButton != null)
        {
            gameButton.onClick.AddListener(OnGameClicked);
        }

        if (inventoryButton != null)
        {
            inventoryButton.onClick.AddListener(OnInventoryClicked);
        }

        if (closeInventoryButton != null)
        {
            closeInventoryButton.onClick.AddListener(OnCloseClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void OnGameClicked()
    {
        SceneChangeManager.ChangeScene(GameScene);
    }

    public void OnInventoryClicked()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(true);
        }
    }

    public void OnCloseClicked()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
