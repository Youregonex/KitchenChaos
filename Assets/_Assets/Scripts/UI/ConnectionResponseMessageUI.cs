using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ConnectionResponseMessageUI : MonoBehaviour
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

        Hide();
    }

    private void KitchenGameMultiplayer_OnFailToJoinGame(object sender, System.EventArgs e)
    {
        Show();

        _messageText.text = NetworkManager.Singleton.DisconnectReason;

        if(NetworkManager.Singleton.DisconnectReason == "")
        {
            _messageText.text = "Failed to connect";
        }
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
    }
}
