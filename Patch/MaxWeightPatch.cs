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
internal static class MaxWeightPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
    public static void ApplyMapExplorationEffect()
    {
        if (!Game.instance.m_firstSpawn) return;
        if (ZoneSystem.instance.GetGlobalKey("MaxWeight-Less"))
        {
            Player.m_localPlayer.m_maxCarryWeight /= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("MaxWeight-More"))
        {
            Player.m_localPlayer.m_maxCarryWeight *= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("MaxWeight-High"))
        {
            Player.m_localPlayer.m_maxCarryWeight *= 2.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("MaxWeight-All"))
        {
            Player.m_localPlayer.m_maxCarryWeight = 999999;
        }
    }
}