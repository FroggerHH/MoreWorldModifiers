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
internal static class PowersBossesOnStartPatch
{
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