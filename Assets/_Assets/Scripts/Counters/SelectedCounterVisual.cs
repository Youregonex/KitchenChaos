using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectArray;

    private void Start()
    {
        //Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == _baseCounter)
            ShowVisual();
        else
            HideVisual();
    }

    private void ShowVisual()
    {
        foreach (GameObject visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void HideVisual()
    {
        foreach (GameObject visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }

}
