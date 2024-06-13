using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;


    private void Awake()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            Debug.Log($"Host Started");
            KitchenGameMultiplayer.Instance.StartHost();
            Hide();
        });

        _startClientButton.onClick.AddListener(() =>
        {
            Debug.Log($"Client Started");
            KitchenGameMultiplayer.Instance.StartClient();
            Hide();
        });
    }

    private void Hide() => gameObject.SetActive(false);
}
