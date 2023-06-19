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
internal static class FallDamagePatch
{
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyFallDamage)), HarmonyPostfix]
    public static void ApplyNoStaminaCostEffect_UseStamina(SEMan __instance, float baseDamage, ref float damage)
    {
        if (!__instance.m_character.IsPlayer() || __instance.m_character != Player.m_localPlayer) return;
        var globalKey = ZoneSystem.instance.GetGlobalKey("FallDamage");
        if(globalKey) return;
        damage = 0;
    }
}