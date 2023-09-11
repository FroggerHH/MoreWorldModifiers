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
internal static class ClearWeatherPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer)), HarmonyPostfix]
    public static void ApplyNoHuginEffect(Game __instance)
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("ClearWeather");
        if (!globalKey) return;

        EnvMan.instance.m_debugEnv = "Clear";
    }
}