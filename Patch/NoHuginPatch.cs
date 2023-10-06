namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class NoHuginPatch
{
    [HarmonyPatch(typeof(Tutorial), nameof(Tutorial.ShowText))] [HarmonyPrefix]
    public static bool ApplyNoHuginEffect()
    {
        var globalKey = instance.GetGlobalKey("NoHugin");
        return !globalKey;
    }
}