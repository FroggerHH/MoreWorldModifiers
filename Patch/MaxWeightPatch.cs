namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class MaxWeightPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void ApplyMapExplorationEffect()
    {
        if (!Game.instance.m_firstSpawn) return;
        if (instance.GetGlobalKey("MaxWeight-Less")) Player.m_localPlayer.m_maxCarryWeight /= 1.5f;

        if (instance.GetGlobalKey("MaxWeight-More")) Player.m_localPlayer.m_maxCarryWeight *= 1.5f;

        if (instance.GetGlobalKey("MaxWeight-High")) Player.m_localPlayer.m_maxCarryWeight *= 2.5f;

        if (instance.GetGlobalKey("MaxWeight-All")) Player.m_localPlayer.m_maxCarryWeight = 999999;
    }
}