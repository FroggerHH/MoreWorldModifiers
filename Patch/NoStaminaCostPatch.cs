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
internal static class NoStaminaCostPatch
{
    [HarmonyPatch(typeof(Player), nameof(Player.UseStamina)), HarmonyPrefix]
    public static bool ApplyNoStaminaCostEffect_UseStamina(Player __instance)
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("NoStaminaCost");
        return !globalKey;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.HaveStamina)), HarmonyPrefix]
    public static bool ApplyNoStaminaCostEffect_HaveStamina(Player __instance, ref bool __result)
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("NoStaminaCost");
        if (!globalKey) return true;

        __result = true;
        return false;
    }
}