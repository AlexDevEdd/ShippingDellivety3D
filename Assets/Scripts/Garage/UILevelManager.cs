using System;
using UnityEngine;
using UnityEngine.UI;

public  class UILevelManager : Singleton<UILevelManager>
{
    [SerializeField] private GameObject _windowDeliveryInfo;
    [Space(10)]
    [Header("First slot")]
    [SerializeField] private Button _firstStartMovebutton;
    [SerializeField] private Button _firstDelliveryInfoButton;
    [SerializeField] private Button _firstClosePanelButton;
    [SerializeField] private GameObject _firstCardeliveryInfoPanel;
    [SerializeField] private Text _firstCarTimerText;
    [Space(10)]
    [Header("Second Slot")]
    [SerializeField] private Button _secondStartMovebutton;
    [SerializeField] private Button _secondDelliveryInfoButton;
    [SerializeField] private Button _secondClosePanelButton;
    [SerializeField] private GameObject _secondCardeliveryInfoPanel;
    [SerializeField] private Text _secondCarTimerText;
    [Space(10)]
    [Header("Third Slot")]
    [SerializeField] private Button _thirdStartMovebutton;
    [SerializeField] private Button _thirdDelliveryInfoButton;
    [SerializeField] private Button _thirdClosePanelButton;
    [SerializeField] private GameObject _thirdCardeliveryInfoPanel;
    [SerializeField] private Text _thirdCarTimerText;
    [Space(10)]
    [Header("Timer")]
    [SerializeField] private Text _timerText;
    [SerializeField] private Slider _serviceSliderFirst;
    [SerializeField] private Slider _serviceSliderSecond;
    [SerializeField] private Slider _serviceSliderThird;

    public event Action OnStartMovingFirstCar = delegate { };
    public event Action OnOpenDelliveryInfoFirstCar = delegate { };
    public event Action OnCloseFirstDelliveryPanel = delegate { };

    public event Action OnStartMovingSecondCar = delegate { };
    public event Action OnOpenDelliveryInfoSecondCar = delegate { };
    public event Action OnCloseSecondDelliveryPanel = delegate { };

    public event Action OnStartMovingThirdCar = delegate { };
    public event Action OnOpenDelliveryInfoThirdCar = delegate { };
    public event Action OnCloseThirdDelliveryPanel = delegate { };

    public Text TimerText { get => _timerText; }
    public GameObject WindowDeliveryInfo => _windowDeliveryInfo;
    public Button FirstStartMovebutton => _firstStartMovebutton;
    public Button FirstDelliveryInfoButton => _firstDelliveryInfoButton;
    public Button FirstClosePanelButton => _firstClosePanelButton;
    public GameObject FirstCardeliveryInfoPanel => _firstCardeliveryInfoPanel;
    public Text FirstCarTimerText  => _firstCarTimerText;
    public Slider ServiceSliderFirst { get => _serviceSliderFirst; set => _serviceSliderFirst = value; }

    public Button SecondStartMovebutton => _secondStartMovebutton;
    public Button SecondDelliveryInfoButton => _secondDelliveryInfoButton;
    public Button SecondClosePanelButton => _secondClosePanelButton; 
    public GameObject SecondCardeliveryInfoPanel => _secondCardeliveryInfoPanel; 
    public Text SecondCarTimerText => _secondCarTimerText;
    public Slider ServiceSliderSecond { get => _serviceSliderSecond; set => _serviceSliderSecond = value; }

    public Button ThirdStartMovebutton => _thirdStartMovebutton;
    public Button ThirdDelliveryInfoButton => _thirdDelliveryInfoButton;
    public Button ThirdClosePanelButton  => _thirdClosePanelButton;
    public GameObject ThirdCardeliveryInfoPanel => _thirdCardeliveryInfoPanel; 
    public Text ThirdCarTimerText  => _thirdCarTimerText;
    public Slider ServiceSliderThird { get => _serviceSliderThird; set => _serviceSliderThird = value; }


    protected override void Awake()
    {
        _firstStartMovebutton.onClick.AddListener(OnStartMovingFirstCarButton);
        _firstDelliveryInfoButton.onClick.AddListener(OnOpenDelliveryInfoFirstCarButton);
        _firstClosePanelButton.onClick.AddListener(OnCloseFirstDelliveryPanelButton);

        _secondStartMovebutton.onClick.AddListener(OnStartMovingSeconCarButton);
        _secondDelliveryInfoButton.onClick.AddListener(OnOpenDelliveryInfoSecondCarButton);
        _secondClosePanelButton.onClick.AddListener(OnCloseSecondDelliveryPanelButton);

        _thirdStartMovebutton.onClick.AddListener(OnStartMovingThirdCarButton);
        _thirdDelliveryInfoButton.onClick.AddListener(OnOpenDelliveryInfoThirdCarButton);
        _thirdClosePanelButton.onClick.AddListener(OnCloseThirdDelliveryPanelButton);

    }

    private void OnCloseThirdDelliveryPanelButton() => OnCloseThirdDelliveryPanel.Invoke();
    private void OnOpenDelliveryInfoThirdCarButton() => OnOpenDelliveryInfoThirdCar.Invoke();
    private void OnStartMovingThirdCarButton() => OnStartMovingThirdCar.Invoke();

    private void OnCloseFirstDelliveryPanelButton() => OnCloseFirstDelliveryPanel.Invoke();
    private void OnOpenDelliveryInfoFirstCarButton() => OnOpenDelliveryInfoFirstCar.Invoke();
    private void OnStartMovingFirstCarButton() => OnStartMovingFirstCar.Invoke();

    private void OnCloseSecondDelliveryPanelButton() => OnCloseSecondDelliveryPanel.Invoke();
    private void OnOpenDelliveryInfoSecondCarButton() => OnOpenDelliveryInfoSecondCar.Invoke();
    private void OnStartMovingSeconCarButton() => OnStartMovingSecondCar.Invoke();


    private void OnDisable()
    {
        _firstStartMovebutton.onClick.RemoveAllListeners();
        _firstDelliveryInfoButton.onClick.RemoveAllListeners();
        _firstClosePanelButton.onClick.RemoveAllListeners();

        _secondStartMovebutton.onClick.RemoveAllListeners();
        _secondDelliveryInfoButton.onClick.RemoveAllListeners();
        _secondClosePanelButton.onClick.RemoveAllListeners();

        _thirdStartMovebutton.onClick.RemoveAllListeners();
        _thirdDelliveryInfoButton.onClick.RemoveAllListeners();
        _thirdClosePanelButton.onClick.RemoveAllListeners();
    }
}

