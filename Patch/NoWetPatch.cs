namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class NoWetPatch
{
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.AddStatusEffect), typeof(int), typeof(bool), typeof(int), typeof(float))]
    [HarmonyPrefix]
    public static bool ApplyNoWetEffect(SEMan __instance, int nameHash)
    {
        if (!__instance.m_character.IsPlayer() || __instance.m_character != Player.m_localPlayer
                                               || !instance) return true;
        var globalKey = instance.GetGlobalKey("NoWet");
        if (!globalKey) return true;
        if (nameHash == "Wet".GetStableHashCode()) return false;

        return true;
    }
}