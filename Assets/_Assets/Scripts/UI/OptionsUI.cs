using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private Transform _rebindKeyTransform;

    [Header("Keyboard Bindings Buttons")]
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private Button _pauseButton;

    [Header("Keyboard Bindings Text Objects")]
    [SerializeField] private TextMeshProUGUI _moveUpButtonText;
    [SerializeField] private TextMeshProUGUI _moveDownButtonText;
    [SerializeField] private TextMeshProUGUI _moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI _moveRightButtonText;
    [SerializeField] private TextMeshProUGUI _interactButtonText;
    [SerializeField] private TextMeshProUGUI _interactAlternateButtonText;
    [SerializeField] private TextMeshProUGUI _pauseButtonText;

    [Header("Gamepad Bindings Buttons")]
    [SerializeField] private Button _gamepadInteractButton;
    [SerializeField] private Button _gamepadInteractAlternateButton;
    [SerializeField] private Button _gamepadPauseButton;

    [Header("Gamepad Bindings Text Objects")]
    [SerializeField] private TextMeshProUGUI _gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractAlternateButtonText;
    [SerializeField] private TextMeshProUGUI _gamepadPauseButtonText;

    private Action OnCloseButtonAction;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _soundEffectsButton.onClick.AddListener(() => 
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();

            OnCloseButtonAction();
        });

        _moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);
        });

        _moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);
        });

        _moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Left);
        });

        _moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);
        });

        _interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });

        _interactAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteractAlternate);
        });

        _pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });

        _gamepadInteractButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Gamepad_Interact);
        });

        _gamepadInteractAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.GamePad_InteractAlternate);
        });

        _gamepadPauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Gamepad_Pause);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        _soundEffectText.text = $"Sound Effects : {Mathf.Round(SoundManager.Instance.GetVolume() * 10f)}";
        _musicText.text = $"Music : {Mathf.Round(MusicManager.Instance.GetVolume() * 10f)}";

        _moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        _gamepadInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _gamepadInteractAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        _gamepadPauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Show(Action OnCloseButtonAction)
    {
        this.OnCloseButtonAction = OnCloseButtonAction;

        gameObject.SetActive(true);

        _soundEffectsButton.Select();
    }

    public void Hide() => gameObject.SetActive(false);

    private void ShowPressToRebindKey() => _rebindKeyTransform.gameObject.SetActive(true);
    private void HidePressToRebindKey() => _rebindKeyTransform.gameObject.SetActive(false);

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();

        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
