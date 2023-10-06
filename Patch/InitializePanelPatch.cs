using TMPro;
using UnityEngine.UI;
using static KeySlider;

namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class InitializePanelPatch
{
    internal static GameObject panel;
    internal static Transform tooltipText;
    internal static Transform tooltipTextParent;
    internal static List<string> keysAdded = new();

    private static void CreateButtons()
    {
        var cancseButton = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Cancel");
        var buttonCloce = Instantiate(cancseButton, panel.transform).GetComponent<Button>();
        buttonCloce.transform.position = new Vector3(buttonCloce.transform.position.x + 100,
            buttonCloce.transform.position.y - 60,
            buttonCloce.transform.position.z);
        buttonCloce.name = "Cloce Advanced Modifiers";
        buttonCloce.onClick.RemoveAllListeners();
        buttonCloce.onClick = new Button.ButtonClickedEvent();
        buttonCloce.GetComponentInChildren<TextMeshProUGUI>().text = "$back_button";
        buttonCloce.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject.SetActive(true);
            tooltipText.SetParent(tooltipTextParent);
        });
        var buttonShow = Instantiate(cancseButton, cancseButton.transform.parent).GetComponent<Button>();
        buttonShow.gameObject.name = "Show Advanced Modifiers";
        buttonShow.onClick.RemoveAllListeners();
        buttonShow.onClick = new Button.ButtonClickedEvent();

        buttonShow.onClick.AddListener(() =>
        {
            panel.SetActive(true);
            ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject.SetActive(false);
            tooltipText.SetParent(panel.transform);
        });
        buttonShow.GetComponentInChildren<TextMeshProUGUI>().text = "$advancedModifiers";
        buttonShow.transform.position = buttonCloce.transform.position;
    }

    private static void CreatePanel()
    {
        panel = Instantiate(ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject,
            ServerOptionsGUI.m_instance.transform.GetChild(0).parent);
        panel.name = "New Modifiers";
        panel.SetActive(false);
        var findTopic = Utils.FindChild(panel.transform, "topic");
        var topicText = findTopic.GetComponent<TextMeshProUGUI>();
        topicText.text = "$advancedModifiers";
        for (var i = 0; i < panel.transform.childCount; i++)
        {
            var gameObject = panel.transform.GetChild(i).gameObject;
            if (gameObject.name != "bkg" && gameObject.name != "topic")
                Destroy(gameObject);
        }

        tooltipText = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Tooltips");
        tooltipTextParent = tooltipText.parent;

        var panelText = panel.GetComponentInChildren<TextMeshProUGUI>();
        panelText.text = "$advancedModifiers";
    }

    [HarmonyPatch(typeof(ServerOptionsGUI), nameof(ServerOptionsGUI.Awake))] [HarmonyPostfix]
    public static void AddMoreWorldModifiers(ServerOptionsGUI __instance)
    {
        CreatePanel();
        CreateButtons();
        CreateToggles();
        CreateSliders();
    }

    private static void CreateSliders()
    {
        CreateSlider("MapExploration", new Vector3(0, 90, 0),
            new SliderSetting
            {
                m_name = "$slider_Less", m_toolTip = "$ExploreMap_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new List<string> { "ExploreMap-Less" }
            },
            new SliderSetting
            {
                m_name = "$slider_Normal", m_toolTip = "$ExploreMap_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new List<string>()
            },
            new SliderSetting
            {
                m_name = "$slider_More", m_toolTip = "$ExploreMap_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new List<string> { "ExploreMap-More" }
            },
            new SliderSetting
            {
                m_name = "$slider_High", m_toolTip = "$ExploreMap_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new List<string> { "ExploreMap-High" }
            },
            new SliderSetting
            {
                m_name = "$slider_All", m_toolTip = "$ExploreMap_All_ToolTip",
                m_modifierValue = WorldModifierOption.VeryEasy, m_keys = new List<string> { "ExploreMap-All" }
            });
        CreateSlider("SkillsSpeed", new Vector3(0, 40, 0),
            new SliderSetting
            {
                m_name = "$slider_Less", m_toolTip = "$SkillsSpeed_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new List<string> { "SkillsSpeed-Less" }
            },
            new SliderSetting
            {
                m_name = "$slider_Normal", m_toolTip = "$SkillsSpeed_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new List<string>()
            },
            new SliderSetting
            {
                m_name = "$slider_More", m_toolTip = "$SkillsSpeed_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new List<string> { "SkillsSpeed-More" }
            },
            new SliderSetting
            {
                m_name = "$slider_High", m_toolTip = "$SkillsSpeed_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new List<string> { "SkillsSpeed-High" }
            },
            new SliderSetting
            {
                m_name = "$slider_All", m_toolTip = "$SkillsSpeed_All_ToolTip",
                m_modifierValue = WorldModifierOption.VeryEasy, m_keys = new List<string> { "SkillsSpeed-All" }
            });
        CreateSlider("HigherStacks", new Vector3(0, -10, 0),
            new SliderSetting
            {
                m_name = "$slider_Less", m_toolTip = "$HigherStacks_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new List<string> { "HigherStacks-Less" }
            },
            new SliderSetting
            {
                m_name = "$slider_Normal", m_toolTip = "$HigherStacks_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new List<string>()
            },
            new SliderSetting
            {
                m_name = "$slider_More", m_toolTip = "$HigherStacks_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new List<string> { "HigherStacks-More" }
            },
            new SliderSetting
            {
                m_name = "$slider_High", m_toolTip = "$HigherStacks_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new List<string> { "HigherStacks-High" }
            });
        CreateSlider("MaxWeight", new Vector3(0, -60, 0),
            new SliderSetting
            {
                m_name = "$slider_Less", m_toolTip = "$MaxWeight_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new List<string> { "MaxWeight-Less" }
            },
            new SliderSetting
            {
                m_name = "$slider_Normal", m_toolTip = "$MaxWeight_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new List<string>()
            },
            new SliderSetting
            {
                m_name = "$slider_More", m_toolTip = "$MaxWeight_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new List<string> { "MaxWeight-More" }
            },
            new SliderSetting
            {
                m_name = "$slider_High", m_toolTip = "$MaxWeight_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new List<string> { "MaxWeight-High" }
            },
            new SliderSetting
            {
                m_name = "$slider_All", m_toolTip = "$MaxWeight_All_ToolTip",
                m_modifierValue = WorldModifierOption.VeryEasy, m_keys = new List<string> { "MaxWeight-All" }
            });
    }

    private static void CreateToggles()
    {
        CreateToggle("PowersBossesOnStart", new Vector3(-110, 240, 0));
        CreateToggle("NoStaminaCost", new Vector3(55, 240, 0));
        CreateToggle("NoDurabilityLoss", new Vector3(220, 240, 0));
        CreateToggle("AllRecipesUnlocked", new Vector3(-110, 190, 0));
        CreateToggle("NoFallDamage", new Vector3(55, 190, 0));
        CreateToggle("NoWet", new Vector3(220, 190, 0));
        CreateToggle("NoHugin", new Vector3(-110, 140, 0));
        CreateToggle("ClearWeather", new Vector3(55, 140, 0));
    }

    private static void CreateToggle(string key, Vector3 pos)
    {
        var example = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "PlayerBasedEvents");
        KeyToggle keyToggle;
        keyToggle = Instantiate(example, example.transform.parent).GetComponent<KeyToggle>();
        keyToggle.name = key;
        keyToggle.m_enabledKey = key;
        keyToggle.m_toolTip = $"${key}_Tooltip";
        keyToggle.GetComponentInChildren<TextMeshProUGUI>().text = $"${key}_DisplayName";
        //keyToggle.m_toolTipLabel = tooltipText;


        var keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(keyToggle);
        ServerOptionsGUI.m_modifiers = keys.ToArray();
        keyToggle.transform.SetParent(panel.transform);
        keyToggle.transform.localPosition = pos;
        var rectTransform = keyToggle.transform as RectTransform;
        var edge = RectTransform.Edge.Top | RectTransform.Edge.Left;
        var index = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom ? 1 : 0;
        var flag = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Right;
        var num = flag ? 1f : 0.0f;
        var anchorMin = rectTransform.anchorMin;
        anchorMin[index] = num;
        rectTransform.anchorMin = anchorMin;
        var anchorMax = rectTransform.anchorMax;
        anchorMax[index] = num;
        rectTransform.anchorMax = anchorMax;

        keysAdded.Add(key);
    }

    private static void CreateSlider(string key, Vector3 pos, params SliderSetting[] settings)
    {
        var example = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Portals");
        KeySlider keySlider;
        var sliderObj = Instantiate(example, example.transform.parent).gameObject;
        keySlider = sliderObj.GetComponentInChildren<KeySlider>();
        sliderObj.name = key;
        keySlider.m_modifier = WorldModifiers.Default;
        sliderObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"${key}_DisplayName";
        //keySlider.m_toolTipLabel = tooltipText;
        keySlider.m_settings = settings.ToList();
        keySlider.m_toolTip = $"${key}_Tooltip";
        keySlider.GetComponent<Slider>().maxValue = keySlider.m_settings.Count - 1;

        var keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(keySlider);
        ServerOptionsGUI.m_modifiers = keys.ToArray();
        sliderObj.transform.SetParent(panel.transform);
        sliderObj.transform.localPosition = pos;
        var rectTransform = sliderObj.transform as RectTransform;
        var edge = RectTransform.Edge.Top | RectTransform.Edge.Left;
        var index = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom ? 1 : 0;
        var flag = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Right;
        var num = flag ? 1f : 0.0f;
        var anchorMin = rectTransform.anchorMin;
        anchorMin[index] = num;
        rectTransform.anchorMin = anchorMin;
        var anchorMax = rectTransform.anchorMax;
        anchorMax[index] = num;
        rectTransform.anchorMax = anchorMax;
    }
}