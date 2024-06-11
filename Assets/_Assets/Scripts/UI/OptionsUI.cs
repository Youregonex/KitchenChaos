using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private Transform _rebindKeyTransform;

    [Header("Bindings Buttons")]
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private Button _pauseButton;

    [Header("Bindings Text Objects")]
    [SerializeField] private TextMeshProUGUI _moveUpButtonText;
    [SerializeField] private TextMeshProUGUI _moveDownButtonText;
    [SerializeField] private TextMeshProUGUI _moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI _moveRightButtonText;
    [SerializeField] private TextMeshProUGUI _interactButtonText;
    [SerializeField] private TextMeshProUGUI _interactAlternateButtonText;
    [SerializeField] private TextMeshProUGUI _pauseButtonText;

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
    }

    public void Show() => gameObject.SetActive(true);
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
