using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MoreWorldModifiers.Plugin;
using static KeySlider;
using static Heightmap;
using Object = UnityEngine.Object;

namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class MoreWorldModifiers
{
    internal static GameObject panel;
    internal static Text tooltipText;

    private static void CreateButtons()
    {
        var cancseButton = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Cancel");
        var buttonCloce = Object.Instantiate(cancseButton, panel.transform).GetComponent<Button>();
        buttonCloce.transform.position = new(buttonCloce.transform.position.x + 100,
            buttonCloce.transform.position.y - 60,
            buttonCloce.transform.position.z);
        buttonCloce.name = "Cloce Advanced Modifiers";
        buttonCloce.onClick.RemoveAllListeners();
        buttonCloce.onClick = new();
        buttonCloce.GetComponentInChildren<Text>().text = "$back_button";
        buttonCloce.onClick.AddListener((() =>
        {
            panel.SetActive(false);
            ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject.SetActive(true);
        }));
        var buttonShow = Object.Instantiate(cancseButton, cancseButton.transform.parent).GetComponent<Button>();
        buttonShow.gameObject.name = "Show Advanced Modifiers";
        buttonShow.onClick.RemoveAllListeners();
        buttonShow.onClick = new();

        buttonShow.onClick.AddListener((() =>
        {
            panel.SetActive(true);
            ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject.SetActive(false);
        }));
        buttonShow.GetComponentInChildren<Text>().text = "$advancedModifiers";
        buttonShow.transform.position = buttonCloce.transform.position;
    }

    private static void CreatePanel()
    {
        panel =
            Object.Instantiate(ServerOptionsGUI.m_instance.transform.GetChild(0).gameObject,
                ServerOptionsGUI.m_instance.transform.GetChild(0).parent);
        panel.SetActive(false);
        panel.name = "New Modifiers";
        Utils.FindChild(panel.transform, "topic").GetComponent<Text>().text = "$advancedModifiers";
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            var gameObject = panel.transform.GetChild(i).gameObject;
            if (gameObject.name != "bkg" &&
                gameObject.name != "topic" &&
                gameObject.name != "Tooltips") // && gameObject.name != ""
            {
                Object.Destroy(gameObject);
            }
        }

        tooltipText = Utils.FindChild(panel.transform, "ToolTipText").GetComponent<Text>();

        panel.GetComponentInChildren<Text>().text = "$advancedModifiers";
    }

    [HarmonyPatch(typeof(ServerOptionsGUI), nameof(ServerOptionsGUI.ReadKeys)), HarmonyPrefix]
    private static void ReadKeysPatch(World world)
    {
        if (world.m_startingGlobalKeys.Count == 0 && !world.m_startingGlobalKeys.Contains("FallDamage".ToLower()))
            world.m_startingGlobalKeys.Add("FallDamage".ToLower());
    }

    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.OnNewWorldDone)), HarmonyPostfix]
    private static void OnNewWorldDone(FejdStartup __instance)
    {
        if (!__instance.m_world.m_startingGlobalKeys.Contains("FallDamage".ToLower()))
            __instance.m_world.m_startingGlobalKeys.Add("FallDamage".ToLower());
    }

    [HarmonyPatch(typeof(ServerOptionsGUI), nameof(ServerOptionsGUI.Awake)), HarmonyPostfix]
    public static void AddMoreWorldModifiers(ServerOptionsGUI __instance)
    {
        CreatePanel();
        CreateButtons();


        CreateMod("PowersBossesOnStart", new Vector3(846, 775, 0));
        CreateMod("NoStaminaCost", new Vector3(1010, 775, 0));
        CreateMod("NoDurabilityLoss", new Vector3(1174, 775, 0));
        CreateMod("AllRecipesUnlocked", new Vector3(846, 720, 0));
        CreateMod("FallDamage", new Vector3(1010, 720, 0), true);
        CreateMod("NoWet", new Vector3(1174, 720, 0), false);
        CreateSlider("MapExploration", new Vector3(960, 670, 0),
            new SliderSetting()
            {
                m_name = "$slider_Less", m_toolTip = "$ExploreMap_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new() { "ExploreMap-Less" }
            },
            new SliderSetting()
            {
                m_name = "$slider_Normal", m_toolTip = "$ExploreMap_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new()
            },
            new SliderSetting()
            {
                m_name = "$slider_More", m_toolTip = "$ExploreMap_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new() { "ExploreMap-More" }
            },
            new SliderSetting()
            {
                m_name = "$slider_High", m_toolTip = "$ExploreMap_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new() { "ExploreMap-High" }
            },
            new SliderSetting()
            {
                m_name = "$slider_All", m_toolTip = "$ExploreMap_All_ToolTip",
                m_modifierValue = WorldModifierOption.VeryEasy, m_keys = new() { "ExploreMap-All" }
            });
        CreateSlider("SkillsSpeed", new Vector3(960, 620, 0),
            new SliderSetting()
            {
                m_name = "$slider_Less", m_toolTip = "$SkillsSpeed_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new() { "SkillsSpeed-Less" }
            },
            new SliderSetting()
            {
                m_name = "$slider_Normal", m_toolTip = "$SkillsSpeed_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new()
            },
            new SliderSetting()
            {
                m_name = "$slider_More", m_toolTip = "$SkillsSpeed_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new() { "SkillsSpeed-More" }
            },
            new SliderSetting()
            {
                m_name = "$slider_High", m_toolTip = "$SkillsSpeed_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new() { "SkillsSpeed-High" }
            },
            new SliderSetting()
            {
                m_name = "$slider_All", m_toolTip = "$SkillsSpeed_All_ToolTip",
                m_modifierValue = WorldModifierOption.VeryEasy, m_keys = new() { "SkillsSpeed-All" }
            });
        CreateSlider("HigherStacks", new Vector3(960, 570, 0),
            new SliderSetting()
            {
                m_name = "$slider_Less", m_toolTip = "$HigherStacks_Less_ToolTip",
                m_modifierValue = WorldModifierOption.Hardcore, m_keys = new() { "HigherStacks-Less" }
            },
            new SliderSetting()
            {
                m_name = "$slider_Normal", m_toolTip = "$HigherStacks_Normal_ToolTip",
                m_modifierValue = WorldModifierOption.Default, m_keys = new()
            },
            new SliderSetting()
            {
                m_name = "$slider_More", m_toolTip = "$HigherStacks_More_ToolTip",
                m_modifierValue = WorldModifierOption.Easy, m_keys = new() { "HigherStacks-More" }
            },
            new SliderSetting()
            {
                m_name = "$slider_High", m_toolTip = "$HigherStacks_High_ToolTip",
                m_modifierValue = WorldModifierOption.Casual, m_keys = new() { "HigherStacks-High" }
            });
    }

    private static void CreateMod(string key, Vector3 pos, bool m_defaultOn = false)
    {
        var example = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "PlayerBasedEvents");
        KeyToggle keyToggle;
        keyToggle = Object.Instantiate(example, example.transform.parent).GetComponent<KeyToggle>();
        keyToggle.name = key;
        keyToggle.m_enabledKey = key;
        keyToggle.m_toolTip = $"${key}_Tooltip";
        keyToggle.GetComponentInChildren<Text>().text = $"${key}_DisplayName";
        keyToggle.m_defaultOn = m_defaultOn;
        keyToggle.m_toggle.isOn = m_defaultOn;


        List<KeyUI> keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(keyToggle);
        ServerOptionsGUI.m_modifiers = keys.ToArray();
        keyToggle.transform.SetParent(panel.transform);
        keyToggle.transform.position = pos;
        keyToggle.m_toolTipLabel = tooltipText;
    }

    private static void CreateSlider(string key, Vector3 pos, params SliderSetting[] settings)
    {
        var example = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Portals");
        KeySlider keySlider;
        GameObject sliderObj = Object.Instantiate(example, example.transform.parent).gameObject;
        keySlider = sliderObj.GetComponentInChildren<KeySlider>();
        sliderObj.name = key;
        keySlider.m_modifier = WorldModifiers.Events;
        sliderObj.transform.GetChild(0).GetComponent<Text>().text = $"${key}_DisplayName";
        keySlider.m_toolTipLabel = tooltipText;
        keySlider.m_settings = settings.ToList();
        keySlider.m_toolTip = $"${key}_Tooltip";
        keySlider.GetComponent<Slider>().maxValue = keySlider.m_settings.Count - 1;

        List<KeyUI> keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(keySlider);
        ServerOptionsGUI.m_modifiers = keys.ToArray();
        sliderObj.transform.SetParent(panel.transform);
        sliderObj.transform.position = pos;
    }
}