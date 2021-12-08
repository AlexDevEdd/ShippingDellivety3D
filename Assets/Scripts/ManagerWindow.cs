using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Utils.Timing;

public class ManagerWindow : MonoBehaviour
{
    [SerializeField] public Button _carMovebutton;
    [SerializeField] private Button _buyButton;
    [SerializeField] public Button _openManagerButton;
    [SerializeField] public GameObject _windowManager;
    [SerializeField] private TimerType _timerType;
    [SerializeField] private float _remainingSeconds;  

    public bool IsClickBuyButton = false;
 
    public void OnShowManagerWindowClick()
    {
        _windowManager.SetActive(true);
        _openManagerButton.gameObject.SetActive(false);
        _carMovebutton.gameObject.SetActive(false);
    }
}
