using UnityEngine.UI;
using UnityEngine;


public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;

    private void Awake()
    {
        _timerImage.fillAmount = 0f;
    }

    private void Update()
    {
        _timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
