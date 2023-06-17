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
using static Heightmap;
using Object = UnityEngine.Object;

namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class MoreWorldModifiers
{
    [HarmonyPatch(typeof(ServerOptionsGUI), nameof(ServerOptionsGUI.Awake)), HarmonyPostfix]
    public static void AddMoreWorldModifiers(ServerOptionsGUI __instance)
    {
        var cancseButton = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Cancel");
        var newPanel =
            Object.Instantiate(__instance.transform.GetChild(0).gameObject, __instance.transform.GetChild(0).parent);
        newPanel.SetActive(false);
        newPanel.name = "New Modifiers";
        for (int i = 0; i < newPanel.transform.childCount; i++)
        {
            var gameObject = newPanel.transform.GetChild(i).gameObject;
            if (gameObject.name != "bkg" && gameObject.name != "topic") // && gameObject.name != ""
            {
                Object.Destroy(gameObject);
            }
        }

        newPanel.GetComponentInChildren<Text>().text = "Advanced Modifiers";

        var buttonCloce = Object.Instantiate(cancseButton, newPanel.transform).GetComponent<Button>();
        buttonCloce.transform.position = new(buttonCloce.transform.position.x + 100,
            buttonCloce.transform.position.y - 60,
            buttonCloce.transform.position.z);
        buttonCloce.name = "Cloce Advanced Modifiers";
        buttonCloce.onClick.RemoveAllListeners();
        buttonCloce.onClick = new();
        buttonCloce.onClick.AddListener((() =>
        {
            newPanel.SetActive(false);
            __instance.transform.GetChild(0).gameObject.SetActive(true);
        }));
        var buttonShow = Object.Instantiate(cancseButton, cancseButton.transform.parent).GetComponent<Button>();
        buttonShow.gameObject.name = "Show Advanced Modifiers";
        buttonShow.onClick.RemoveAllListeners();
        buttonShow.onClick = new();
        buttonShow.onClick.AddListener((() =>
        {
            newPanel.SetActive(true);
            __instance.transform.GetChild(0).gameObject.SetActive(false);
        }));
        buttonShow.GetComponentInChildren<Text>().text = "Advanced Modifiers";
        buttonShow.transform.position = buttonCloce.transform.position;


        var mod1 = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "PlayerBasedEvents");
        var powersBossesOnStart = Object.Instantiate(mod1, mod1.transform.parent).GetComponent<KeyToggle>();
        powersBossesOnStart.name = "PowersBossesOnStart";
        powersBossesOnStart.m_enabledKey = "PowersBossesOnStart";
        powersBossesOnStart.m_toolTip = "Powers of all bosses on start";
        powersBossesOnStart.GetComponentInChildren<Text>().text = "Powers of all bosses on start";

        var haldorOnStart = Object.Instantiate(mod1, mod1.transform.parent).GetComponent<KeyToggle>();
        haldorOnStart.name = "HaldorOnStart";
        haldorOnStart.m_enabledKey = "HaldorOnStart";
        haldorOnStart.m_toolTip = "Haldor on starting island";
        haldorOnStart.GetComponentInChildren<Text>().text = "Haldor on start";

        var hildirOnStart = Object.Instantiate(mod1, mod1.transform.parent).GetComponent<KeyToggle>();
        hildirOnStart.name = "HildirOnStart";
        hildirOnStart.m_enabledKey = "HildirOnStart";
        hildirOnStart.m_toolTip = "Hildir  on starting island";
        hildirOnStart.GetComponentInChildren<Text>().text = "Hildir on start";

        List<KeyUI> keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(powersBossesOnStart);
        keys.Add(haldorOnStart);
        keys.Add(hildirOnStart);
        ServerOptionsGUI.m_modifiers = keys.ToArray();

        powersBossesOnStart.transform.SetParent(newPanel.transform);
        powersBossesOnStart.transform.position = new Vector3(846, 775, 0);
        haldorOnStart.transform.SetParent(newPanel.transform);
        haldorOnStart.transform.position = new Vector3(1010, 775, 0);
        hildirOnStart.transform.SetParent(newPanel.transform);
        hildirOnStart.transform.position = new Vector3(1174, 775, 0);
    }

    [HarmonyPatch(typeof(BossStone), nameof(BossStone.Start)), HarmonyPostfix]
    public static void ApplyPowersBossesOnStartEffectPatch(BossStone __instance)
    {
        __instance.StartCoroutine(ApplyPowersBossesOnStartEffect(__instance));
    }


    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.GenerateLocations), new Type[0]), HarmonyPostfix]
    public static void ApplyHaldorOnStartEffectPatch(ZoneSystem __instance)
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("HaldorOnStart");
        if (!globalKey) return;

        var location = __instance.m_locations.Find(x => x.m_prefabName == "Haldor");
        List<ZoneSystem.LocationInstance> locations = new();
        ZoneSystem.instance.FindLocations("StartTemple", ref locations);
        
        Vector2i randomZone = __instance.GetRandomZone(100);
        Vector3 randomPointInZone = __instance.GetRandomPointInZone(randomZone, 0);
        __instance.RegisterLocation(location, randomPointInZone, false);
    }

    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.GenerateLocations), new Type[0]), HarmonyPostfix]
    public static void ApplyHildirOnStartEffectPatch(ZoneSystem __instance)
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("HildirOnStart");
        if (!globalKey) return;
        Debug($"__instance.m_locations = {__instance.m_locations.Count}");

        var location = __instance.m_locations.Find(x => x.m_prefabName == "Hildir");
        List<ZoneSystem.LocationInstance> locations = new();
        ZoneSystem.instance.FindLocations("StartTemple", ref locations);
        
        Vector2i randomZone = __instance.GetRandomZone(100);
        Vector3 randomPointInZone = __instance.GetRandomPointInZone(randomZone, 0);
        __instance.RegisterLocation(location, randomPointInZone, false);
    }

    public static IEnumerator ApplyPowersBossesOnStartEffect(BossStone bossStone)
    {
        yield return new WaitWhile(() => !Player.m_localPlayer);
        var globalKey = ZoneSystem.instance.GetGlobalKey("PowersBossesOnStart");
        if (globalKey && bossStone.m_itemStand && bossStone.m_itemStand.m_nview &&
            bossStone.m_itemStand.m_nview.IsOwner())
        {
            if (!bossStone.m_itemStand.HaveAttachment())
            {
                var item = bossStone.m_itemStand.m_supportedItems[0].m_itemData;
                foreach (var drop in ObjectDB.m_instance.GetAllItems(ItemDrop.ItemData.ItemType.Trophy, ""))
                {
                    if (drop.m_itemData.m_shared.m_name == item.m_shared.m_name)
                    {
                        item.m_dropPrefab = drop.gameObject;
                        break;
                    }
                }

                Debug($"localPlayer is {Player.m_localPlayer}");
                Player.m_localPlayer.GetInventory().AddItem(item);
                bossStone.m_itemStand.m_queuedItem = item;
                bossStone.m_itemStand.CancelInvoke("UpdateAttach");
                bossStone.m_itemStand.InvokeRepeating("UpdateAttach", 0.0f, 0.1f);
                bossStone.m_itemStand.m_queuedItem = item;
            }
        }
    }
}
