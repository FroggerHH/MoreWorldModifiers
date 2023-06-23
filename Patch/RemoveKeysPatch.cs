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
internal static class RemoveKeysPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
    public static void RemoveKeys(Game __instance)
    {
        if (!__instance.m_firstSpawn) return;

        var startingGlobalKeys = WorldGenerator.instance.m_world.m_startingGlobalKeys;
        var keys = MoreWorldModifiers.keysAdded;

        foreach (var key in keys)
        {
            if (startingGlobalKeys.Contains(key.ToLower())) continue;

            ZoneSystem.instance.RemoveGlobalKey(key);
        }
    }
}