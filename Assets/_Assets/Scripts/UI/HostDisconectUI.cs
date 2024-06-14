using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HostDisconectUI : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;


    private void Start()
    {
        _playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
        if(clientID == NetworkManager.ServerClientId)
        {
            Show();
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
}
