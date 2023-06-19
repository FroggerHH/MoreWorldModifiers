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
internal static class AllRecipesUnlockedPatch
{
    [HarmonyPatch(typeof(Player), nameof(Player.Start)), HarmonyPostfix]
    public static void ApplyAllRecipesUnlockedEffect(Player __instance)
    {
        if(SceneManager.GetActiveScene().name != "main") return;
        var globalKey = ZoneSystem.instance.GetGlobalKey("AllRecipesUnlocked");
        if(!globalKey) return;
        
        foreach (Recipe recipe in ObjectDB.instance.m_recipes)
        {
            if (recipe.m_enabled && recipe.m_item && !__instance.m_knownRecipes.Contains(recipe.m_item.m_itemData.m_shared.m_name))
                __instance.AddKnownRecipe(recipe);
        }
        
        __instance.UpdateKnownRecipesList();
    }
}