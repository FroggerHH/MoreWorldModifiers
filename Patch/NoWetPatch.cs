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
internal static class NoWetPatch
{
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.AddStatusEffect), typeof(int), typeof(bool), typeof(int), typeof(float)), HarmonyPrefix]
    public static bool ApplyNoWetEffect(SEMan __instance, int nameHash)
    {
        if (!__instance.m_character.IsPlayer() || __instance.m_character != Player.m_localPlayer || !ZoneSystem.instance) return true;
        var globalKey = ZoneSystem.instance.GetGlobalKey("NoWet");
        if(!globalKey) return true;
        if (nameHash == "Wet".GetStableHashCode()) return false;

        return true;
    }
}