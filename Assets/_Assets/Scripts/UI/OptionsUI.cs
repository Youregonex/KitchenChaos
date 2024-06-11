using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private TextMeshProUGUI _soundEffectText;
    [SerializeField] private TextMeshProUGUI _musicText;

    private void Awake()
    {
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
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _soundEffectText.text = $"Sound Effects : {Mathf.Round(SoundManager.Instance.GetVolume() * 10f)}";
        _musicText.text = $"Music : {Mathf.Round(MusicManager.Instance.GetVolume() * 10f)}";
    }
}
