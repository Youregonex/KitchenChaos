using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _messageText;


    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });    
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailToJoinGame += KitchenGameMultiplayer_OnFailToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Could not find Lobby to Quick Join!");
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to join Lobby!");
    }

    private void KitchenGameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining Lobby...");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
    }

    private void KitchenGameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Lobby...");
    }

    private void KitchenGameMultiplayer_OnFailToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();

        _messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailToJoinGame -= KitchenGameMultiplayer_OnFailToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted -= KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed -= KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted -= KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed -= KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed -= KitchenGameLobby_OnQuickJoinFailed;
    }
}
