using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button _quickJoinLobbyButton;
    [SerializeField] private Button _joinByCodeButton;
    [SerializeField] private TMP_InputField _codeInputField;
    [SerializeField] private TMP_InputField _playerNameInputField;
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    [SerializeField] private Transform _lobbyContainer;
    [SerializeField] private Transform _lobbyTemplate;


    private void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();

            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _createLobbyButton.onClick.AddListener(() =>
        {
            _lobbyCreateUI.Show();
        });

        _quickJoinLobbyButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });

        _joinByCodeButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinWithCode(_codeInputField.text);
        });

        _lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _playerNameInputField.text = KitchenGameMultiplayer.Instance.GetPlayerName();

        _playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            KitchenGameMultiplayer.Instance.SetPlayerName(newText);
        });

        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in _lobbyContainer)
        {
            if (child == _lobbyTemplate)
                continue;

            Destroy(child.gameObject);
        }

        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);

            lobbyTransform.gameObject.SetActive(true);

            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);

        }
    }

}
