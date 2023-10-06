using UnityEngine.SceneManagement;

namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class AllRecipesUnlockedPatch
{
    [HarmonyPatch(typeof(Player), nameof(Player.Start))] [HarmonyPostfix]
    public static void ApplyAllRecipesUnlockedEffect(Player __instance)
    {
        if (SceneManager.GetActiveScene().name != "main") return;
        if (__instance != Player.m_localPlayer) return;
        var globalKey = instance.GetGlobalKey("AllRecipesUnlocked");
        if (!globalKey) return;

        foreach (var recipe in ObjectDB.instance.m_recipes)
            if (recipe.m_enabled && recipe.m_item
                                 && !__instance.m_knownRecipes.Contains(recipe.m_item.m_itemData.m_shared.m_name))
                __instance.AddKnownRecipe(recipe);

        __instance.UpdateKnownRecipesList();
    }
}