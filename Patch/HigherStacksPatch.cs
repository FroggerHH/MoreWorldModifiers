namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class HigherStacksPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void ApplyHigherStacksEffect()
    {
        if (!Game.instance.m_firstSpawn) return;

        var less = instance.GetGlobalKey("HigherStacks-Less");
        var more = instance.GetGlobalKey("HigherStacks-More");
        var high = instance.GetGlobalKey("HigherStacks-High");
        foreach (var item_ in ObjectDB.instance.m_items)
        {
            var item = item_.GetComponent<ItemDrop>();
            var size = item.m_itemData.m_shared.m_maxStackSize;
            if (size == 1) continue;
            if (less) item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt(size / 1.5f);

            if (more) item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt(size * 1.5f);

            if (high) item.m_itemData.m_shared.m_maxStackSize = Mathf.FloorToInt(size * 2.5f);
        }
    }
}