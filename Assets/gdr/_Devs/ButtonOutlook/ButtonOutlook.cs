using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonOutlook : MonoBehaviour
{
    [System.Serializable]
    public class Outlook
    {
        public bool isInteractable;
        public Sprite sprite;
        public Color color = Color.white;
        public List<GameObject> gosToActivate;
        public List<Text> texts;
    }
    [Header("Settings")]
    public List<Outlook> outlooks;
    public int defaultOutlookId;
    [NaughtyAttributes.ReadOnly]
    public Button button;
    [Header("State")]
    [NaughtyAttributes.ReadOnly]
    [SerializeField]
    private Outlook currentOutlook;
    [NaughtyAttributes.ReadOnly]
    [SerializeField]
    private int currentOutlookId;

    private bool isButtonAttached = false;
    private List<GameObject> childObjects;
    private bool isCheckedOutlookInitialization = false;

    private void Awake()
    {
        if (!CheckInitialization())
            return;
        SetDefaultOutlook();
    }

    public void SetOutlook(int id, bool forced = false)
    {
        if (!CheckInitialization())
            return;

        id = Mathf.Clamp(id, 0, outlooks.Count - 1);
        if (!forced && id == currentOutlookId)
            return;

        if (!isButtonAttached)
        {
            button = GetComponent<Button>();
            if (button != null)
                isButtonAttached = true;
            else
            {
                Debug.LogError($"Button not attached for ButtonOutlook {gameObject.name}");
                return;
            }
            childObjects = GetComponentsInChildren<Transform>().Select(x => x.gameObject).ToList();
            childObjects.Remove(gameObject);
        }

        currentOutlook = outlooks[id];
        button.interactable = currentOutlook.isInteractable;
        button.image.sprite = currentOutlook.sprite;
        button.image.color = currentOutlook.color;
        foreach (var go in childObjects)
            go.SetActive(false);
        foreach (var go in currentOutlook.gosToActivate)
            if (go != null)
                go.SetActive(true);
    }

    public void ChangeCurrentOutlookText(int textId, string text)
    {
        if (currentOutlook == null ||
            currentOutlook.texts == null ||
            currentOutlook.texts.Count <= textId ||
            currentOutlook.texts[textId] == null)
            return;

        currentOutlook.texts[textId].text = text;
    }

    [NaughtyAttributes.Button]
    private void SetDefaultOutlook()
    {
        currentOutlookId = Mathf.Clamp(defaultOutlookId, 0, outlooks.Count - 1);
        var outlookId = defaultOutlookId;
        SetOutlook(outlookId, true);
    }

    private bool CheckInitialization()
    {
        if (!isCheckedOutlookInitialization)
        {
            if (outlooks == null || outlooks.Count == 0)
            {
                Debug.LogError($"Outlooks not initialized");
                return false;
            }
            else
                isCheckedOutlookInitialization = true;
        }
        return true;
    }
}
