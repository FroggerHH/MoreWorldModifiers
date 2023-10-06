namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class NoDurabilityLossPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void ApplyNoDurabilityLossEffect()
    {
        if (!Game.instance.m_firstSpawn) return;
        var globalKey = instance.GetGlobalKey("NoDurabilityLoss");
        if (!globalKey) return;

        foreach (var item in ObjectDB.instance.m_items)
        {
            item.GetComponent<ItemDrop>().m_itemData.m_shared.m_useDurability = false;
            item.GetComponent<ItemDrop>().m_itemData.m_shared.m_durabilityDrain = 0;
            item.GetComponent<ItemDrop>().m_itemData.m_shared.m_useDurabilityDrain = 0;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.PlacePiece))] [HarmonyPostfix]
    public static void ApplyNoDurabilityLossEffect_Hammer(Player __instance)
    {
        var globalKey = instance.GetGlobalKey("NoDurabilityLoss");
        if (!globalKey) return;

        var rightItem = __instance.GetRightItem();
        if (rightItem != null) rightItem.m_durability = rightItem.m_shared.m_maxDurability;
    }
}