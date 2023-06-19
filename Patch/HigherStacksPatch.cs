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
internal static class HigherStacksPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
    public static void ApplyHigherStacksEffect()
    {
        if (!Game.instance.m_firstSpawn) return;

        var less = ZoneSystem.instance.GetGlobalKey("HigherStacks-Less");
        var more = ZoneSystem.instance.GetGlobalKey("HigherStacks-More");
        var high = ZoneSystem.instance.GetGlobalKey("HigherStacks-High");
        foreach (var item_ in ObjectDB.instance.m_items)
        {
            var item = item_.GetComponent<ItemDrop>();
            var size = item.m_itemData.m_shared.m_maxStackSize;
            if (less)
            {
                item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt((float)size / 1.5f);
            }

            if (more)
            {
                item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt((float)size * 1.5f);
            }

            if (high)
            {
                item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt((float)size * 2.5f);
            }
        }
    }
}