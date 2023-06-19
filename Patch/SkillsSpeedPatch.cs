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
internal static class SkillsSpeedPatch
{
    [HarmonyPatch(typeof(Player), nameof(Player.RaiseSkill)), HarmonyPrefix]
    public static void ApplySkillsSpeedEffect(Player __instance, ref float value)
    {
        if (__instance != Player.m_localPlayer) return;
        if (value == 10000) return;
        if (ZoneSystem.instance.GetGlobalKey("SkillsSpeed-Less"))
        {
            value /= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("SkillsSpeed-More"))
        {
            value *= 1.5f;
        }

        if (ZoneSystem.instance.GetGlobalKey("SkillsSpeed-High"))
        {
            value *= 2.5f;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Start)), HarmonyPostfix]
    public static void ApplySkillsSpeedEffect_All(Player __instance)
    {
        if (__instance != Player.m_localPlayer) return;
        if (ZoneSystem.instance.GetGlobalKey("SkillsSpeed-All"))
        {
            foreach (var mSkill in Player.m_localPlayer.m_skills.m_skills)
            {
                Player.m_localPlayer.m_skills.GetSkill(mSkill.m_skill).m_level = 100;
            }
        }
    }
}