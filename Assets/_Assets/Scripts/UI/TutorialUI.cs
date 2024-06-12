using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("Keyboard Bindings")]
    [SerializeField] private TextMeshProUGUI _moveUpKeyText;
    [SerializeField] private TextMeshProUGUI _moveDownKeyText;
    [SerializeField] private TextMeshProUGUI _moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI _moveRightKeyText;
    [SerializeField] private TextMeshProUGUI _interactKeyText;
    [SerializeField] private TextMeshProUGUI _interactAlternateText;
    [SerializeField] private TextMeshProUGUI _pauseText;

    [Header("Gamepad Bindings")]
    [SerializeField] private TextMeshProUGUI _gamepadInteractKeyText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _gamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _moveUpKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _moveDownKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _moveLeftKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _moveRightKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);

        _interactKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        _gamepadInteractKeyText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        _gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
