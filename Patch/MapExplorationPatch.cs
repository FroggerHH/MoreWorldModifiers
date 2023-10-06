namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class MapExplorationPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void ApplyMapExplorationEffect()
    {
        if (!Game.instance.m_firstSpawn) return;
        if (instance.GetGlobalKey("ExploreMap-Less")) Minimap.instance.m_exploreRadius /= 1.5f;

        if (instance.GetGlobalKey("ExploreMap-More")) Minimap.instance.m_exploreRadius *= 1.5f;

        if (instance.GetGlobalKey("ExploreMap-High")) Minimap.instance.m_exploreRadius *= 2.5f;

        if (instance.GetGlobalKey("ExploreMap-All")) Minimap.instance.ExploreAll();
    }
}