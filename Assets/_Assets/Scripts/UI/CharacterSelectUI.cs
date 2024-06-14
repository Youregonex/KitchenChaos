using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Services.Lobbies.Models;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();

            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        });
    }

    private void Start()
    {
        Lobby lobby = KitchenGameLobby.Instance.GetLobby();

        _lobbyNameText.text = $"Lobby name : {lobby.Name}";
        _lobbyCodeText.text = $"Lobby code : {lobby.LobbyCode}";
    }

}
