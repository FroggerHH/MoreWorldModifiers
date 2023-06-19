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
internal static class MapExplorationPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
    public static void ApplyMapExplorationEffect()
    {
        if (!Game.instance.m_firstSpawn) return;
        if (ZoneSystem.instance.GetGlobalKey("ExploreMap-Less"))
        {
            Minimap.instance.m_exploreRadius /= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("ExploreMap-More"))
        {
            Minimap.instance.m_exploreRadius *= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("ExploreMap-High"))
        {
            Minimap.instance.m_exploreRadius *= 2.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("ExploreMap-All"))
        {
            Minimap.instance.ExploreAll();
        }
    }
}