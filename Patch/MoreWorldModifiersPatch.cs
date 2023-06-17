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
    private static bool isInitingStartTemple = false;

    [HarmonyPatch(typeof(ServerOptionsGUI), nameof(ServerOptionsGUI.Awake)), HarmonyPostfix]
    public static void AddMoreWorldModifiers(ServerOptionsGUI __instance)
    {
        //AssetBundle bundle = PrefabManager.RegisterAssetBundle()
        var cancseButton = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Cancel");
        var newPanel =
            Object.Instantiate(__instance.transform.GetChild(0).gameObject, __instance.transform.GetChild(0).parent);
        newPanel.SetActive(false);
        newPanel.name = "New Modifiers";
        for (int i = 0; i < newPanel.transform.childCount; i++)
        {
            Object.Destroy(newPanel.transform.GetChild(i).gameObject);
        }

        var buttonCloce = Object.Instantiate(cancseButton, newPanel.transform).GetComponent<Button>();
        buttonCloce.name = "Cloce Advanced Settings";
        buttonCloce.onClick.RemoveAllListeners();
        buttonCloce.onClick = new();
        buttonCloce.onClick.AddListener((() =>
        {
            newPanel.SetActive(false);
            __instance.transform.GetChild(0).gameObject.SetActive(true);
        }));
        var buttonShow = Object.Instantiate(cancseButton, cancseButton.transform.parent).GetComponent<Button>();
        buttonShow.gameObject.name = "Show Advanced Settings";
        buttonShow.onClick.RemoveAllListeners();
        buttonShow.onClick = new();
        buttonShow.onClick.AddListener((() =>
        {
            newPanel.SetActive(true);
            __instance.transform.GetChild(0).gameObject.SetActive(false);
        }));
        buttonShow.GetComponentInChildren<Text>().text = "Advanced Settings";
        buttonShow.transform.position = new(buttonShow.transform.position.x + 50, buttonShow.transform.position.y - 20,
            buttonShow.transform.position.z);
//(newPanel.transform as RectTransform).
        // foreach (var modifier in ServerOptionsGUI.m_modifiers)
        // {
        //     if (modifier is KeyToggle)
        //     {
        //         modifier.transform.position = new Vector3(modifier.transform.position.x - 50,
        //             modifier.transform.position.y - 50,
        //             modifier.transform.position.z);
        //     }
        // }
        //
        // ServerOptionsGUI.m_instance.m_doneButton.transform.position = new Vector3(
        //     ServerOptionsGUI.m_instance.m_doneButton.transform.position.x,
        //     ServerOptionsGUI.m_instance.m_doneButton.transform.position.y - 50,
        //     ServerOptionsGUI.m_instance.m_doneButton.transform.position.z);
        // cancseButton.transform.position = new Vector3(
        //     cancseButton.transform.position.x,
        //     cancseButton.transform.position.y - 50,
        //     cancseButton.transform.position.z);

        var mod1 = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "PlayerBasedEvents");
        var mod2 = Utils.FindChild(ServerOptionsGUI.m_instance.transform, "Nomap");
        var customModkeyTest1 = Object.Instantiate(mod1, mod1.transform.parent); //1086
        customModkeyTest1.transform.position = new Vector3(mod1.transform.position.x + 164, mod1.transform.position.y,
            mod1.transform.position.z);
        customModkeyTest1.name = "PowersBossesOnStart";
        var customModkeyTestKeyToggle1 = customModkeyTest1.GetComponent<KeyToggle>();
        customModkeyTestKeyToggle1.m_enabledKey = "PowersBossesOnStart";
        customModkeyTestKeyToggle1.m_toolTip = "Powers of all bosses on start";
        customModkeyTestKeyToggle1.GetComponentInChildren<Text>().text = "Powers of all bosses on start";

        var customModkeyTest2 = Object.Instantiate(mod2, mod2.transform.parent); //1086
        customModkeyTest2.transform.position = new Vector3(mod2.transform.position.x + 164, mod2.transform.position.y,
            mod2.transform.position.z);
        customModkeyTest2.name = "CustomModkeyTest2";
        var customModkeyTest2KeyToggle = customModkeyTest2.GetComponent<KeyToggle>();
        customModkeyTest2KeyToggle.m_enabledKey = "CustomModkeyTest2";
        customModkeyTest2KeyToggle.m_toolTip = "CustomModkeyTest2";
        customModkeyTest2KeyToggle.GetComponentInChildren<Text>().text = "CustomModkeyTest2";

        List<KeyUI> keys = ServerOptionsGUI.m_modifiers.ToList();
        keys.Add(customModkeyTestKeyToggle1);
        keys.Add(customModkeyTest2KeyToggle);
        ServerOptionsGUI.m_modifiers = keys.ToArray();
    }

    [HarmonyPatch(typeof(BossStone), nameof(BossStone.Start)), HarmonyPostfix]
    public static void ApplyPowersBossesOnStartEffectPatch(BossStone __instance)
    {
        __instance.StartCoroutine(ApplyPowersBossesOnStartEffect(__instance));
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