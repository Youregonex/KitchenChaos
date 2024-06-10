using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter _cuttingCounter;
    [SerializeField] private Image _barImage;


    private void Start()
    {
        _cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
        _barImage.fillAmount = 0f;

        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChengedEEventArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;

        if(e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
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
