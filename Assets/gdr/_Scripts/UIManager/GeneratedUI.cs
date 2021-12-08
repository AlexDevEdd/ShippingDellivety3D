using UnityEngine;
using UnityEngine.UI;

public abstract class GeneratedUI : MonoBehaviour
{
	public static bool isDebug;
	public static Color debugColor;
	public GameObject canvas;

	public TapToStartPanel tapToStartPanel;
	public LevelRewardPanel levelRewardPanel;
	public LevelLosePanel levelLosePanel;
	public PlayerStatusPanel playerStatusPanel;
	public UpgradesPanel upgradesPanel;

	[System.Serializable]
	public class TapToStartPanel
	{
		public GameObject current;
		public Animator currentAnimator;

		public Image background;

		public Button tapToPlayButton;

		public Text tapToPlayText;

		public void SetPanelActive(bool value, bool isInstant = false)
		{
			if (currentAnimator != null)
			{
				if (value)
					GenericUI.Show(currentAnimator);
				else
				{
					GenericUI.Hide(currentAnimator);
					if (isInstant)
						currentAnimator.Play("Idle");
				}
			}
			else
			{
				if (value)
				{
					current.transform.GetChild(0).localScale = Vector3.one;
					background.transform.localScale = Vector3.one;
				}
				else
				{
					current.transform.GetChild(0).localScale = Vector3.zero;
					background.transform.localScale = Vector3.zero;
				}
			}
		}

		public void Link()
		{
			CollectorScript.Instance.InitProperty(ref current, $"TapToStartPanel");
			if (current == null) Debug.Log("Cant init current panel");
			CollectorScript.Instance.InitProperty(ref currentAnimator, $"TapToStartPanel");
			if (currentAnimator == null) Debug.Log("TapToStartPanel: No panel animator found");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref background, $"Background", $"TapToStartPanel");
			if (background == null) Debug.Log("Cant init background");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref tapToPlayButton, $"TapToPlayButton", $"TapToStartPanel");
			if (tapToPlayButton == null) Debug.Log("Cant init tapToPlayButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref tapToPlayText, $"TapToPlayText", $"TapToStartPanel");
			if (tapToPlayText == null) Debug.Log("Cant init tapToPlayText");
		}
	}

	[System.Serializable]
	public class LevelRewardPanel
	{
		public GameObject current;
		public Animator currentAnimator;

		public Image background;
		public Image mainArea;
		public Image rewardLevelImage;
		public Image moneyImage;

		public Button levelRewardX3Button;
		public Button levelRewardButton;

		public Text levelRewardTitleText;
		public Text levelRewardTitleText_2;
		public Text levelRewardMoneyText;
		public Text levelRewardX3Text;

		public void SetPanelActive(bool value, bool isInstant = false)
		{
			if (currentAnimator != null)
			{
				if (value)
					GenericUI.Show(currentAnimator);
				else
				{
					GenericUI.Hide(currentAnimator);
					if (isInstant)
						currentAnimator.Play("Idle");
				}
			}
			else
			{
				if (value)
				{
					current.transform.GetChild(0).localScale = Vector3.one;
					background.transform.localScale = Vector3.one;
				}
				else
				{
					current.transform.GetChild(0).localScale = Vector3.zero;
					background.transform.localScale = Vector3.zero;
				}
			}
		}

		public void Link()
		{
			CollectorScript.Instance.InitProperty(ref current, $"LevelRewardPanel");
			if (current == null) Debug.Log("Cant init current panel");
			CollectorScript.Instance.InitProperty(ref currentAnimator, $"LevelRewardPanel");
			if (currentAnimator == null) Debug.Log("LevelRewardPanel: No panel animator found");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref background, $"Background", $"LevelRewardPanel");
			if (background == null) Debug.Log("Cant init background");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref mainArea, $"MainArea", $"LevelRewardPanel");
			if (mainArea == null) Debug.Log("Cant init mainArea");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref rewardLevelImage, $"RewardLevelImage", $"LevelRewardPanel");
			if (rewardLevelImage == null) Debug.Log("Cant init rewardLevelImage");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moneyImage, $"MoneyImage", $"LevelRewardPanel");
			if (moneyImage == null) Debug.Log("Cant init moneyImage");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardX3Button, $"LevelRewardX3Button", $"LevelRewardPanel");
			if (levelRewardX3Button == null) Debug.Log("Cant init levelRewardX3Button");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardButton, $"LevelRewardButton", $"LevelRewardPanel");
			if (levelRewardButton == null) Debug.Log("Cant init levelRewardButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardTitleText, $"LevelRewardTitleText", $"LevelRewardPanel");
			if (levelRewardTitleText == null) Debug.Log("Cant init levelRewardTitleText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardTitleText_2, $"LevelRewardTitleText_2", $"LevelRewardPanel");
			if (levelRewardTitleText_2 == null) Debug.Log("Cant init levelRewardTitleText_2");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardMoneyText, $"LevelRewardMoneyText", $"LevelRewardPanel");
			if (levelRewardMoneyText == null) Debug.Log("Cant init levelRewardMoneyText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelRewardX3Text, $"LevelRewardX3Text", $"LevelRewardPanel");
			if (levelRewardX3Text == null) Debug.Log("Cant init levelRewardX3Text");
		}
	}

	[System.Serializable]
	public class LevelLosePanel
	{
		public GameObject current;
		public Animator currentAnimator;


		public Button levelLoseRespawnButton;
		public Button levelLoseRestartButton;

		public Text levelLoseTitleText;
		public Text levelLoseMoneyText;
		public Text levelLoseRespawnText;
		public Text levelLoseRestartText;

		public void SetPanelActive(bool value, bool isInstant = false)
		{
			if (currentAnimator != null)
			{
				if (value)
					GenericUI.Show(currentAnimator);
				else
				{
					GenericUI.Hide(currentAnimator);
					if (isInstant)
						currentAnimator.Play("Idle");
				}
			}
			else
			{
				if (value)
					current.transform.GetChild(0).localScale = Vector3.one;
				else
					current.transform.GetChild(0).localScale = Vector3.zero;
			}
		}

		public void Link()
		{
			CollectorScript.Instance.InitProperty(ref current, $"LevelLosePanel");
			if (current == null) Debug.Log("Cant init current panel");
			CollectorScript.Instance.InitProperty(ref currentAnimator, $"LevelLosePanel");
			if (currentAnimator == null) Debug.Log("LevelLosePanel: No panel animator found");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseRespawnButton, $"LevelLoseRespawnButton", $"LevelLosePanel");
			if (levelLoseRespawnButton == null) Debug.Log("Cant init levelLoseRespawnButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseRestartButton, $"LevelLoseRestartButton", $"LevelLosePanel");
			if (levelLoseRestartButton == null) Debug.Log("Cant init levelLoseRestartButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseTitleText, $"LevelLoseTitleText", $"LevelLosePanel");
			if (levelLoseTitleText == null) Debug.Log("Cant init levelLoseTitleText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseMoneyText, $"LevelLoseMoneyText", $"LevelLosePanel");
			if (levelLoseMoneyText == null) Debug.Log("Cant init levelLoseMoneyText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseRespawnText, $"LevelLoseRespawnText", $"LevelLosePanel");
			if (levelLoseRespawnText == null) Debug.Log("Cant init levelLoseRespawnText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref levelLoseRestartText, $"LevelLoseRestartText", $"LevelLosePanel");
			if (levelLoseRestartText == null) Debug.Log("Cant init levelLoseRestartText");
		}
	}

	[System.Serializable]
	public class PlayerStatusPanel
	{
		public GameObject current;
		public Animator currentAnimator;

		public GameObject moneyArea;
		public GameObject settingsArea;
		public GameObject vibrationParent;
		public GameObject healthArea;
		public GameObject progressArea;
		public GameObject progressCircle;
		public Image playerDamageImage;
		public Image moneyIcon;
		public Image healthIcon;
		public Image progressBg;
		public Image progressFill;
		public Image progressOutline;
		public Image progressCircleOutline;
		public Image progressCircleGoal;

		public Button settingsAnimation;
		public Button settingsButton;

		public Text moneyText;
		public Text healthText;

		public void SetPanelActive(bool value, bool isInstant = false)
		{
			if (currentAnimator != null)
			{
				if (value)
					GenericUI.Show(currentAnimator);
				else
				{
					GenericUI.Hide(currentAnimator);
					if (isInstant)
						currentAnimator.Play("Idle");
				}
			}
			else
			{
				if (value)
					current.transform.GetChild(0).localScale = Vector3.one;
				else
					current.transform.GetChild(0).localScale = Vector3.zero;
			}
		}

		public void Link()
		{
			CollectorScript.Instance.InitProperty(ref current, $"PlayerStatusPanel");
			if (current == null) Debug.Log("Cant init current panel");
			CollectorScript.Instance.InitProperty(ref currentAnimator, $"PlayerStatusPanel");
			if (currentAnimator == null) Debug.Log("PlayerStatusPanel: No panel animator found");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moneyArea, $"MoneyArea", $"PlayerStatusPanel");
			if (moneyArea == null) Debug.Log("Cant init moneyArea");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref settingsArea, $"SettingsArea", $"PlayerStatusPanel");
			if (settingsArea == null) Debug.Log("Cant init settingsArea");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref vibrationParent, $"VibrationParent", $"PlayerStatusPanel");
			if (vibrationParent == null) Debug.Log("Cant init vibrationParent");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref healthArea, $"HealthArea", $"PlayerStatusPanel");
			if (healthArea == null) Debug.Log("Cant init healthArea");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressArea, $"ProgressArea", $"PlayerStatusPanel");
			if (progressArea == null) Debug.Log("Cant init progressArea");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressCircle, $"ProgressCircle", $"PlayerStatusPanel");
			if (progressCircle == null) Debug.Log("Cant init progressCircle");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref playerDamageImage, $"PlayerDamageImage", $"PlayerStatusPanel");
			if (playerDamageImage == null) Debug.Log("Cant init playerDamageImage");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moneyIcon, $"MoneyIcon", $"PlayerStatusPanel");
			if (moneyIcon == null) Debug.Log("Cant init moneyIcon");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref healthIcon, $"HealthIcon", $"PlayerStatusPanel");
			if (healthIcon == null) Debug.Log("Cant init healthIcon");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressBg, $"ProgressBg", $"PlayerStatusPanel");
			if (progressBg == null) Debug.Log("Cant init progressBg");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressFill, $"ProgressFill", $"PlayerStatusPanel");
			if (progressFill == null) Debug.Log("Cant init progressFill");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressOutline, $"ProgressOutline", $"PlayerStatusPanel");
			if (progressOutline == null) Debug.Log("Cant init progressOutline");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressCircleOutline, $"ProgressCircleOutline", $"PlayerStatusPanel");
			if (progressCircleOutline == null) Debug.Log("Cant init progressCircleOutline");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref progressCircleGoal, $"ProgressCircleGoal", $"PlayerStatusPanel");
			if (progressCircleGoal == null) Debug.Log("Cant init progressCircleGoal");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref settingsAnimation, $"SettingsAnimation", $"PlayerStatusPanel");
			if (settingsAnimation == null) Debug.Log("Cant init settingsAnimation");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref settingsButton, $"SettingsButton", $"PlayerStatusPanel");
			if (settingsButton == null) Debug.Log("Cant init settingsButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moneyText, $"MoneyText", $"PlayerStatusPanel");
			if (moneyText == null) Debug.Log("Cant init moneyText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref healthText, $"HealthText", $"PlayerStatusPanel");
			if (healthText == null) Debug.Log("Cant init healthText");
		}
	}

	[System.Serializable]
	public class UpgradesPanel
	{
		public GameObject current;
		public Animator currentAnimator;

		public GameObject horizontal;
		public Image amountUpgradePricePanel;
		public Image moveUpgradePricePanel;
		public Image earningsUpgradePricePanel;

		public Button amountUpgradeButton;
		public Button moveUpgradeButton;
		public Button earningsUpgradeButton;

		public Text amountUpgradeCostText;
		public Text amountUpgradeLevelText;
		public Text amountUpgradeNameText;
		public Text moveUpgradeCostText;
		public Text moveUpgradeLevelText;
		public Text moveUpgradeNameText;
		public Text earningsUpgradeCostText;
		public Text earningsUpgradeLevelText;
		public Text earningsUpgradeNameText;

		public void SetPanelActive(bool value, bool isInstant = false)
		{
			if (currentAnimator != null)
			{
				if (value)
					GenericUI.Show(currentAnimator);
				else
				{
					GenericUI.Hide(currentAnimator);
					if (isInstant)
						currentAnimator.Play("Idle");
				}
			}
			else
			{
				if (value)
					current.transform.GetChild(0).localScale = Vector3.one;
				else
					current.transform.GetChild(0).localScale = Vector3.zero;
			}
		}

		public void Link()
		{
			CollectorScript.Instance.InitProperty(ref current, $"UpgradesPanel");
			if (current == null) Debug.Log("Cant init current panel");
			CollectorScript.Instance.InitProperty(ref currentAnimator, $"UpgradesPanel");
			if (currentAnimator == null) Debug.Log("UpgradesPanel: No panel animator found");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref horizontal, $"Horizontal", $"UpgradesPanel");
			if (horizontal == null) Debug.Log("Cant init horizontal");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref amountUpgradePricePanel, $"AmountUpgradePricePanel", $"UpgradesPanel");
			if (amountUpgradePricePanel == null) Debug.Log("Cant init amountUpgradePricePanel");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moveUpgradePricePanel, $"MoveUpgradePricePanel", $"UpgradesPanel");
			if (moveUpgradePricePanel == null) Debug.Log("Cant init moveUpgradePricePanel");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref earningsUpgradePricePanel, $"EarningsUpgradePricePanel", $"UpgradesPanel");
			if (earningsUpgradePricePanel == null) Debug.Log("Cant init earningsUpgradePricePanel");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref amountUpgradeButton, $"AmountUpgradeButton", $"UpgradesPanel");
			if (amountUpgradeButton == null) Debug.Log("Cant init amountUpgradeButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moveUpgradeButton, $"MoveUpgradeButton", $"UpgradesPanel");
			if (moveUpgradeButton == null) Debug.Log("Cant init moveUpgradeButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref earningsUpgradeButton, $"EarningsUpgradeButton", $"UpgradesPanel");
			if (earningsUpgradeButton == null) Debug.Log("Cant init earningsUpgradeButton");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref amountUpgradeCostText, $"AmountUpgradeCostText", $"UpgradesPanel");
			if (amountUpgradeCostText == null) Debug.Log("Cant init amountUpgradeCostText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref amountUpgradeLevelText, $"AmountUpgradeLevelText", $"UpgradesPanel");
			if (amountUpgradeLevelText == null) Debug.Log("Cant init amountUpgradeLevelText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref amountUpgradeNameText, $"AmountUpgradeNameText", $"UpgradesPanel");
			if (amountUpgradeNameText == null) Debug.Log("Cant init amountUpgradeNameText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moveUpgradeCostText, $"MoveUpgradeCostText", $"UpgradesPanel");
			if (moveUpgradeCostText == null) Debug.Log("Cant init moveUpgradeCostText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moveUpgradeLevelText, $"MoveUpgradeLevelText", $"UpgradesPanel");
			if (moveUpgradeLevelText == null) Debug.Log("Cant init moveUpgradeLevelText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref moveUpgradeNameText, $"MoveUpgradeNameText", $"UpgradesPanel");
			if (moveUpgradeNameText == null) Debug.Log("Cant init moveUpgradeNameText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref earningsUpgradeCostText, $"EarningsUpgradeCostText", $"UpgradesPanel");
			if (earningsUpgradeCostText == null) Debug.Log("Cant init earningsUpgradeCostText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref earningsUpgradeLevelText, $"EarningsUpgradeLevelText", $"UpgradesPanel");
			if (earningsUpgradeLevelText == null) Debug.Log("Cant init earningsUpgradeLevelText");
			CollectorScript.Instance.InitPropertyWhereAnyParentIs(ref earningsUpgradeNameText, $"EarningsUpgradeNameText", $"UpgradesPanel");
			if (earningsUpgradeNameText == null) Debug.Log("Cant init earningsUpgradeNameText");
		}
	}

	void Initialize()
	{
		// Init data here
		enabled = true;
	}

	[NaughtyAttributes.Button]
	public void LinkAll()
	{
		CollectorScript.Instance.InitProperty(ref canvas, $"MainCanvas");
		if (canvas == null) Debug.Log("Cant init canvas");
		tapToStartPanel.Link();
		levelRewardPanel.Link();
		levelLosePanel.Link();
		playerStatusPanel.Link();
		upgradesPanel.Link();
	}

	public void PositionGameReady()
	{
		tapToStartPanel.current.transform.localPosition = Vector3.zero;
		levelRewardPanel.current.transform.localPosition = Vector3.zero;
		levelLosePanel.current.transform.localPosition = Vector3.zero;
		playerStatusPanel.current.transform.localPosition = Vector3.zero;
		upgradesPanel.current.transform.localPosition = Vector3.zero;
	}

	public void PositionEditorOnly()
	{
		tapToStartPanel.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * 1 / canvas.transform.lossyScale.x, 0f, 0f);
		levelRewardPanel.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * 2 / canvas.transform.lossyScale.x, 0f, 0f);
		levelLosePanel.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * 3 / canvas.transform.lossyScale.x, 0f, 0f);
		playerStatusPanel.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * 4 / canvas.transform.lossyScale.x, 0f, 0f);
		upgradesPanel.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * 5 / canvas.transform.lossyScale.x, 0f, 0f);
	}

	public void ShowAllUI()
	{
		tapToStartPanel.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
		levelRewardPanel.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
		levelLosePanel.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
		playerStatusPanel.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
		upgradesPanel.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
	}

	public void HideAllUI()
	{
		tapToStartPanel.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);
		levelRewardPanel.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);
		levelLosePanel.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);
		playerStatusPanel.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);
		upgradesPanel.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);
	}
}