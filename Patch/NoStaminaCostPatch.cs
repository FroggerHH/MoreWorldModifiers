namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class NoStaminaCostPatch
{
    [HarmonyPatch(typeof(Player), nameof(Player.UseStamina))] [HarmonyPrefix]
    public static bool ApplyNoStaminaCostEffect_UseStamina(Player __instance)
    {
        var globalKey = instance.GetGlobalKey("NoStaminaCost");
        return !globalKey;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.HaveStamina))] [HarmonyPrefix]
    public static bool ApplyNoStaminaCostEffect_HaveStamina(Player __instance, ref bool __result)
    {
        var globalKey = instance.GetGlobalKey("NoStaminaCost");
        if (!globalKey) return true;

        __result = true;
        return false;
    }
}