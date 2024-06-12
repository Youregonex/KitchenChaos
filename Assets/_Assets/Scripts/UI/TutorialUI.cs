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


}
