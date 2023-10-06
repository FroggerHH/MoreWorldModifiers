namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class RemoveKeysPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void RemoveKeys(Game __instance)
    {
        if (!__instance.m_firstSpawn) return;

        var startingGlobalKeys = WorldGenerator.instance.m_world.m_startingGlobalKeys;
        var keys = InitializePanelPatch.keysAdded;

        foreach (var key in keys)
        {
            if (startingGlobalKeys.Contains(key.ToLower())) continue;

            instance.RemoveGlobalKey(key);
        }
    }
}