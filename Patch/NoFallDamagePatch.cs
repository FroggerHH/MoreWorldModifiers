namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class NoFallDamagePatch
{
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyFallDamage))] [HarmonyPrefix]
    public static bool ApplyNoStaminaCostEffect_UseStamina(SEMan __instance, ref float baseDamage, ref float damage)
    {
        if (!__instance.m_character.IsPlayer() || __instance.m_character != Player.m_localPlayer) return true;
        var globalKey = instance.GetGlobalKey("NoFallDamage");
        if (globalKey)
        {
            damage = 0;
            baseDamage = 0;

            return false;
        }

        return true;
    }
}