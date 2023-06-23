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
        if (!__instance.m_firstSpawn) return;
        var globalKey = ZoneSystem.instance.GetGlobalKey("ClearWeather");
        if (!globalKey) return;

        EnvMan.instance.m_environments.Remove(
            EnvMan.instance.m_environments.Find(x => x.m_name == "Mistlands_thunder"));
        EnvMan.instance.m_environments.Remove(
            EnvMan.instance.m_environments.Find(x => x.m_name == "Mistlands_rain"));
        EnvMan.instance.m_environments.Remove(
            EnvMan.instance.m_environments.Find(x => x.m_name == "ThunderStorm"));
        EnvMan.instance.m_environments.Remove(
            EnvMan.instance.m_environments.Find(x => x.m_name == "LightRain"));
        EnvMan.instance.m_environments.Remove(
            EnvMan.instance.m_environments.Find(x => x.m_name == "Rain"));
    }
}