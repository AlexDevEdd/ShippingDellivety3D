#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Version 1.2

public class UIGenerator : MonoBehaviour
{
    public GameObject canvasRoot;
    public List<GameObject> ignorePanels;
    public List<GameObject> ignorePanelsWithName;
    public List<GameObject> ignoreItAndInsideIt;
    public bool isUseGameObjects = true;
    public bool isUseImages = true;
    public bool areYouUsingSafeArea = true;
    public bool isShowCode;
    [NaughtyAttributes.ShowIf("isShowCode")]
    [NaughtyAttributes.ResizableTextArea]
    public string resultCode;
    public Object codeFile;

    private string yourUIClassName = "GeneratedUI";

    [NaughtyAttributes.Button]
    void GenerateClasses()
    {
        if (codeFile == null)
        {
            Debug.LogError("Please set output codeFile");
            return;
        }

        yourUIClassName = codeFile.name;
        string preDefCode = "using UnityEngine;\n";
        preDefCode += "using UnityEngine.UI;\n\n";
        preDefCode += $"public abstract class {yourUIClassName} : MonoBehaviour\n" + "{\n";
        //preDefCode += $"\t#region Singleton Init\n";
        //preDefCode += $"\tprivate static {yourUIClassName} _instance;\n\n";
        //preDefCode += $"\tvoid Awake() // Init in order\n";
        //preDefCode += "\t{\n";
        //preDefCode += "\t\tif (_instance == null)\n";
        //preDefCode += "\t\t\tInit();\n";
        //preDefCode += "\t\telse if (_instance != this)\n";
        //preDefCode += "\t\t{\n";
        //preDefCode += "\t\t\tDebug.Log($\"Destroying { gameObject.name}, caused by one singleton instance\");\n";
        //preDefCode += "\t\t\tDestroy(gameObject);\n";
        //preDefCode += "\t\t}\n";
        //preDefCode += "\t}\n\n";
        //preDefCode += $"\tpublic static {yourUIClassName} Instance // Init not in order\n";
        //preDefCode += "\t{\n";
        //preDefCode += "\t\tget\n";
        //preDefCode += "\t\t{\n";
        //preDefCode += "\t\t\tif (_instance == null)\n";
        //preDefCode += "\t\t\t\tInit();\n";
        //preDefCode += "\t\t\treturn _instance;\n";
        //preDefCode += "\t\t}\n";
        //preDefCode += "\t\tprivate set { _instance = value; }\n";
        //preDefCode += "\t}\n\n";
        //preDefCode += "\tstatic void Init() // Init script\n";
        //preDefCode += "\t{\n";
        //preDefCode += $"\t\t_instance = FindObjectOfType<{yourUIClassName}>();\n";
        //preDefCode += "\t\tif (_instance != null)\n";
        //preDefCode += "\t\t\t_instance.Initialize();\n";
        //preDefCode += "\t}\n";
        //preDefCode += "\t#endregion\n\n";
        string definitions = "\tpublic static bool isDebug;\n";
        definitions += "\tpublic static Color debugColor;\n";
        definitions += "\tpublic GameObject canvas;\n\n";
        string postDefCode = "";

        if (canvasRoot == null)
            canvasRoot = gameObject;

        if (areYouUsingSafeArea)
            canvasRoot = transform.GetChild(0).gameObject;
        int menuCount = canvasRoot.transform.childCount;
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);

            var gos = menu.GetComponentsInChildren<Transform>();
            var images = menu.GetComponentsInChildren<Image>();
            postDefCode += $"\t[System.Serializable]\n\tpublic class {menuNameBig}\n" + "\t{\n";
            postDefCode += $"\t\tpublic GameObject current;\n";
            postDefCode += $"\t\tpublic Animator currentAnimator;\n\n";
            if (isUseGameObjects)
            {
                foreach (var go in gos)
                {
                    if (IsPanelIgnored(go.gameObject))
                        continue;
                    if (IsIgnoreHard(go))
                        continue;

                    if (go.gameObject.GetComponent<Button>() != null ||
                         go.gameObject.GetComponent<Text>() != null ||
                         go.gameObject.GetComponent<InputField>() != null ||
                         go.gameObject.GetComponent<Image>() != null ||
                         go.gameObject.GetComponent<Toggle>() != null ||
                         go.gameObject.GetComponent<Slider>() != null)
                        continue;

                    if (go.gameObject.name != "Panel" && go.gameObject.name[0] != '_' && go.parent.gameObject != canvasRoot)
                    {
                        var goName = go.gameObject.name.Replace(" ", "");
                        goName = char.ToLower(goName[0]) + goName.Substring(1);
                        postDefCode += $"\t\tpublic GameObject {goName};\n";
                    }
                }
            }
            bool isHasBackground = false;
            if (isUseImages)
            {
                foreach (var image in images)
                {
                    if (IsPanelIgnored(image.gameObject))
                        continue;
                    if (IsIgnoreHard(image.transform))
                        continue;

                    if (image.GetComponent<Button>() != null ||
                        image.GetComponent<Text>() != null ||
                        image.GetComponent<InputField>() != null ||
                        image.GetComponent<Toggle>() != null ||
                        image.GetComponent<Slider>() != null)
                        continue;

                    if (image.name[0] != '_')
                    {
                        var imageName = image.name.Replace(" ", "");
                        imageName = char.ToLower(imageName[0]) + imageName.Substring(1);
                        postDefCode += $"\t\tpublic Image {imageName};\n";
                        if (imageName == "background")
                            isHasBackground = true;
                    }
                }
            }
            else
                images = new Image[0];
            var buttons = menu.GetComponentsInChildren<Button>();
            if (isUseImages)
            {
                if (buttons.Length > 0)
                    postDefCode += $"\n";
            }
            foreach (var button in buttons)
            {
                if (IsPanelIgnored(button.gameObject))
                    continue;
                if (IsIgnoreHard(button.transform))
                    continue;

                if (button.name[0] != '_')
                {
                    var buttonName = button.name.Replace(" ", "");
                    buttonName = char.ToLower(buttonName[0]) + buttonName.Substring(1);
                    postDefCode += $"\t\tpublic Button {buttonName};\n";
                }
            }
            var texts = menu.GetComponentsInChildren<Text>();
            if (texts.Length > 0)
                postDefCode += $"\n";
            foreach (var text in texts)
            {
                if (IsPanelIgnored(text.gameObject))
                    continue;
                if (IsIgnoreHard(text.transform))
                    continue;

                if (text.name[0] != '_')
                {
                    var textName = text.name.Replace(" ", "");
                    textName = char.ToLower(textName[0]) + textName.Substring(1);
                    postDefCode += $"\t\tpublic Text {textName};\n";
                }
            }
            var inputs = menu.GetComponentsInChildren<InputField>();
            if (inputs.Length > 0)
                postDefCode += $"\n";
            foreach (var input in inputs)
            {
                if (IsPanelIgnored(input.gameObject))
                    continue;
                if (IsIgnoreHard(input.transform))
                    continue;

                if (input.name[0] != '_')
                {
                    var inputName = input.name.Replace(" ", "");
                    inputName = char.ToLower(inputName[0]) + inputName.Substring(1);
                    postDefCode += $"\t\tpublic InputField {inputName};\n";
                }
            }
            var toggles = menu.GetComponentsInChildren<Toggle>();
            if (toggles.Length > 0)
                postDefCode += $"\n";
            foreach (var toggle in toggles)
            {
                if (IsPanelIgnored(toggle.gameObject))
                    continue;
                if (IsIgnoreHard(toggle.transform))
                    continue;

                if (toggle.name[0] != '_')
                {
                    var toggleName = toggle.name.Replace(" ", "");
                    toggleName = char.ToLower(toggleName[0]) + toggleName.Substring(1);
                    postDefCode += $"\t\tpublic Toggle {toggleName};\n";
                }
            }
            var sliders = menu.GetComponentsInChildren<Slider>();
            if (sliders.Length > 0)
                postDefCode += $"\n";
            foreach (var slider in sliders)
            {
                if (IsPanelIgnored(slider.gameObject))
                    continue;
                if (IsIgnoreHard(slider.transform))
                    continue;

                if (slider.name[0] != '_')
                {
                    var toggleName = slider.name.Replace(" ", "");
                    toggleName = char.ToLower(toggleName[0]) + toggleName.Substring(1);
                    postDefCode += $"\t\tpublic Slider {toggleName};\n";
                }
            }

            //#region Init()
            //postDefCode += "\n\t\tpublic void Init()\n\t\t{\n\n";
            //postDefCode += "\t\t}\n";
            //#endregion /Init()


            #region SetActive()
            postDefCode += "\n\t\tpublic void SetPanelActive(bool value, bool isInstant = false)\n\t\t{\n";
            postDefCode += $"\t\t\tif (currentAnimator != null)\n";
            postDefCode += "\t\t\t{\n";
            postDefCode += $"\t\t\t\tif (value)\n";
            postDefCode += $"\t\t\t\t\tGenericUI.Show(currentAnimator);\n";
            postDefCode += $"\t\t\t\telse\n";
            postDefCode += $"\t\t\t\t" + "{\n";
            postDefCode += $"\t\t\t\t\tGenericUI.Hide(currentAnimator);\n";
            postDefCode += $"\t\t\t\t\tif (isInstant)\n";
            postDefCode += $"\t\t\t\t\t\tcurrentAnimator.Play(\"Idle\");\n";
            postDefCode += $"\t\t\t\t" + "}\n";
            postDefCode += "\t\t\t}\n";
            postDefCode += "\t\t\telse\n";
            postDefCode += "\t\t\t{\n";
            postDefCode += "\t\t\t\tif (value)\n";

            if (isHasBackground)
                postDefCode += "\t\t\t\t{\n";

            postDefCode += "\t\t\t\t\tcurrent.transform.GetChild(0).localScale = Vector3.one;\n";

            if (isHasBackground)
                postDefCode += "\t\t\t\t\tbackground.transform.localScale = Vector3.one;\n\t\t\t\t}\n";

            postDefCode += "\t\t\t\telse\n";

            if (isHasBackground)
                postDefCode += "\t\t\t\t{\n";

            postDefCode += "\t\t\t\t\tcurrent.transform.GetChild(0).localScale = Vector3.zero;\n";

            if (isHasBackground)
                postDefCode += "\t\t\t\t\tbackground.transform.localScale = Vector3.zero;\n\t\t\t\t}\n";

            postDefCode += "\t\t\t}\n";
            postDefCode += "\t\t}";
            #endregion /SetActive()

            #region Link
            postDefCode += "\n\n\t\tpublic void Link()\n\t\t{\n";
            postDefCode += $"\t\t\tCollectorScript.Instance.InitProperty(ref current, $\"{menuNameBig}\");\n";
            postDefCode += $"\t\t\tif (current == null) Debug.Log(\"Cant init current panel\");\n";
            postDefCode += $"\t\t\tCollectorScript.Instance.InitProperty(ref currentAnimator, $\"{menuNameBig}\");\n";
            postDefCode += $"\t\t\tif (currentAnimator == null) Debug.Log(\"{menuNameBig}: No panel animator found\");\n";
            if (isUseGameObjects)
            {
                foreach (var go in gos)
                {
                    if (IsPanelIgnored(go.gameObject))
                        continue;
                    if (IsIgnoreHard(go.transform))
                        continue;

                    if (go.GetComponent<Button>() != null ||
                        go.GetComponent<Text>() != null ||
                        go.GetComponent<InputField>() != null ||
                        go.GetComponent<Image>() != null ||
                        go.GetComponent<Slider>() != null ||
                        go.GetComponent<Toggle>() != null)
                        continue;

                    if (go.gameObject.name != "Panel" && go.name[0] != '_' && go.parent.gameObject != canvasRoot)
                    {
                        var goNameBig = go.name.Replace(" ", "");
                        var goName = char.ToLower(goNameBig[0]) + goNameBig.Substring(1);
                        postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {goName}, $\"{goNameBig}\", $\"{menuNameBig}\");\n";
                        postDefCode += $"\t\t\tif ({goName} == null) Debug.Log(\"Cant init {goName}\");\n";
                    }
                }
            }
            foreach (var image in images)
            {
                if (IsPanelIgnored(image.gameObject))
                    continue;
                if (IsIgnoreHard(image.transform))
                    continue;

                if (image.GetComponent<Button>() != null ||
                    image.GetComponent<Text>() != null ||
                    image.GetComponent<InputField>() != null ||
                    image.GetComponent<Toggle>() != null ||
                    image.GetComponent<Slider>() != null)
                    continue;

                if (image.name[0] != '_')
                {
                    var imageNameBig = image.name.Replace(" ", "");
                    var imageName = char.ToLower(imageNameBig[0]) + imageNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {imageName}, $\"{imageNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({imageName} == null) Debug.Log(\"Cant init {imageName}\");\n";
                }
            }
            foreach (var button in buttons)
            {
                if (IsPanelIgnored(button.gameObject))
                    continue;
                if (IsIgnoreHard(button.transform))
                    continue;

                if (button.name[0] != '_')
                {
                    var buttonNameBig = button.name.Replace(" ", "");
                    var buttonName = char.ToLower(buttonNameBig[0]) + buttonNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {buttonName}, $\"{buttonNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({buttonName} == null) Debug.Log(\"Cant init {buttonName}\");\n";
                }
            }
            foreach (var text in texts)
            {
                if (IsPanelIgnored(text.gameObject))
                    continue;
                if (IsIgnoreHard(text.transform))
                    continue;

                if (text.name[0] != '_')
                {
                    var textNameBig = text.name.Replace(" ", "");
                    var textName = char.ToLower(textNameBig[0]) + textNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {textName}, $\"{textNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({textName} == null) Debug.Log(\"Cant init {textName}\");\n";
                }
            }
            foreach (var input in inputs)
            {
                if (IsPanelIgnored(input.gameObject))
                    continue;
                if (IsIgnoreHard(input.transform))
                    continue;

                if (input.name[0] != '_')
                {
                    var inputNameBig = input.name.Replace(" ", "");
                    var inputName = char.ToLower(inputNameBig[0]) + inputNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {inputName}, $\"{inputNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({inputName} == null) Debug.Log(\"Cant init {inputName}\");\n";
                }
            }
            foreach (var toggle in toggles)
            {
                if (IsPanelIgnored(toggle.gameObject))
                    continue;
                if (IsIgnoreHard(toggle.transform))
                    continue;

                if (toggle.name[0] != '_')
                {
                    var toggleNameBig = toggle.name.Replace(" ", "");
                    var toggleName = char.ToLower(toggleNameBig[0]) + toggleNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {toggleName}, $\"{toggleNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({toggleName} == null) Debug.Log(\"Cant init {toggleName}\");\n";
                }
            }
            foreach (var slider in sliders)
            {
                if (IsPanelIgnored(slider.gameObject))
                    continue;
                if (IsIgnoreHard(slider.transform))
                    continue;

                if (slider.name[0] != '_')
                {
                    var sliderNameBig = slider.name.Replace(" ", "");
                    var sliderName = char.ToLower(sliderNameBig[0]) + sliderNameBig.Substring(1);
                    postDefCode += $"\t\t\tCollectorScript.Instance.InitPropertyWhereAnyParentIs(ref {sliderName}, $\"{sliderNameBig}\", $\"{menuNameBig}\");\n";
                    postDefCode += $"\t\t\tif ({sliderName} == null) Debug.Log(\"Cant init {sliderName}\");\n";
                }
            }
            postDefCode += "\t\t}";
            #endregion /Link

            // postDefCode += "\n\t}\n" + $"\tpublic {menuNameBig} {menuName};\n\n";
            postDefCode += "\n\t}\n\n";
            definitions += $"\tpublic {menuNameBig} {menuName};\n";
        }

        definitions += $"\n";

        postDefCode += "\tvoid Initialize()\n";
        postDefCode += "\t{\n";
        postDefCode += "\t\t// Init data here\n";
        postDefCode += "\t\tenabled = true;\n";
        postDefCode += "\t}\n\n";

        postDefCode += "\t[NaughtyAttributes.Button]\n\tpublic void LinkAll()" + "\n\t{\n";
        if (areYouUsingSafeArea)
            postDefCode += $"\t\tCollectorScript.Instance.InitProperty(ref canvas, $\"{canvasRoot.transform.parent.name}\");\n";
        else
            postDefCode += $"\t\tCollectorScript.Instance.InitProperty(ref canvas, $\"{canvasRoot.name}\");\n";
        postDefCode += $"\t\tif (canvas == null) Debug.Log(\"Cant init canvas\");\n";
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu.transform))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);

            postDefCode += $"\t\t{menuName}.Link();\n";
        }
        postDefCode += "\t}";


        // postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void PositionGameReady()" + "\n\t{\n";
        postDefCode += "\n\n\tpublic void PositionGameReady()" + "\n\t{\n";
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu.transform))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
            postDefCode += $"\t\t{menuName}.current.transform.localPosition = Vector3.zero;\n";
        }
        postDefCode += "\t}";

        int screenId = 1;
        // postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void PositionEditorOnly()" + "\n\t{\n";
        postDefCode += "\n\n\tpublic void PositionEditorOnly()" + "\n\t{\n";
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu.transform))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
            // Debug.Log($"{Camera.main.pixelWidth}, {Camera.main.pixelHeight}");
            postDefCode += $"\t\t{menuName}.current.transform.localPosition = new Vector3(Camera.main.pixelWidth * {screenId} / canvas.transform.lossyScale.x, 0f, 0f);\n";
            screenId++;
        }
        postDefCode += "\t}";

        //postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void HideAll()" + "\n\t{\n";
        //for (int i = 0; i < menuCount; i++)
        //{
        //    var menu = canvasRoot.transform.GetChild(i);
        //    if (IsPanelIgnored(menu.gameObject))
        //        continue;

        //    var menuNameBig = menu.name.Replace(" ", "");
        //    var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
        //    postDefCode += $"\t\t{menuName}.SetActive(false);\n";
        //}
        //postDefCode += "\t}";

        //postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void InitAll()" + "\n\t{\n";
        //for (int i = 0; i < menuCount; i++)
        //{
        //    var menu = canvasRoot.transform.GetChild(i);
        //    if (IsPanelIgnored(menu.gameObject))
        //        continue;

        //    var menuNameBig = menu.name.Replace(" ", "");
        //    var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
        //    postDefCode += $"\t\t{menuName}.Init();\n";
        //}
        //postDefCode += "\t}";

        // postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void ShowAllUI()" + "\n\t{\n";
        postDefCode += "\n\n\tpublic void ShowAllUI()" + "\n\t{\n";
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu.transform))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
            postDefCode += $"\t\t{menuName}.current.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);\n";
        }
        postDefCode += "\t}";

        // postDefCode += "\n\n\t[NaughtyAttributes.Button]\n\tpublic void HideAllUI()" + "\n\t{\n";
        postDefCode += "\n\n\tpublic void HideAllUI()" + "\n\t{\n";
        for (int i = 0; i < menuCount; i++)
        {
            var menu = canvasRoot.transform.GetChild(i);
            if (IsPanelIgnored(menu.gameObject))
                continue;
            if (IsIgnoreHard(menu.transform))
                continue;

            var menuNameBig = menu.name.Replace(" ", "");
            var menuName = char.ToLower(menuNameBig[0]) + menuNameBig.Substring(1);
            postDefCode += $"\t\t{menuName}.current.transform.GetChild(0).localScale = new Vector3(0f, 0f, 1f);\n";
        }
        postDefCode += "\t}\n}";

        this.resultCode = preDefCode + definitions + postDefCode;
        Debug.Log(this.resultCode);

        if (areYouUsingSafeArea)
            canvasRoot = canvasRoot.transform.parent.gameObject;
    }

    [NaughtyAttributes.Button]
    void CompileIntoFile()
    {
        GenerateClasses();
        var assetPath = AssetDatabase.GetAssetPath(codeFile);
        var dataPath = Application.dataPath;
        dataPath = dataPath.Replace("Assets", assetPath);
        Debug.Log(dataPath);
        if (System.IO.File.Exists(dataPath))
        {
            Debug.Log("Exist!");
            System.IO.File.WriteAllText(dataPath, string.Empty);
            using (var file = System.IO.File.Open(dataPath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(file);
                sw.Write(this.resultCode);
                sw.Close();
                file.Close();
            }
        }
        AssetDatabase.Refresh();
    }

    private bool IsPanelIgnored(GameObject panel)
    {
        foreach (var item in ignorePanels)
        {
            if (item != null && panel == item)
                return true;
        }
        foreach (var item in ignorePanelsWithName)
        {
            if (item != null && panel.name == item.name)
                return true;
        }
        return false;
    }

    private bool IsIgnoreHard(Transform target)
    {
        if (ignoreItAndInsideIt.Find(x => x != null && x.name == target.name) != null)
            return true;
        else
        {
            while(target != null)
            {
                target = target.parent;
                if (target != null)
                {
                    if (ignoreItAndInsideIt.Find(x => x != null && x.name == target.name) != null)
                        return true;
                }
            }
        }
        return false;
    }
}
#endif
