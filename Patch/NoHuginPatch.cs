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
internal static class NoHuginPatch
{
    [HarmonyPatch(typeof(Tutorial), nameof(Tutorial.ShowText)), HarmonyPrefix]
    public static bool ApplyNoHuginEffect()
    {
        var globalKey = ZoneSystem.instance.GetGlobalKey("NoHugin");
        return !globalKey;
    }
}