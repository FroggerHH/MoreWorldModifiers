namespace MoreWorldModifiers;

[HarmonyPatch]
internal static class ClearWeatherPatch
{
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))] [HarmonyPostfix]
    public static void ApplyNoHuginEffect(Game __instance)
    {
        var globalKey = instance.GetGlobalKey("ClearWeather");
        if (!globalKey) return;

        EnvMan.instance.m_debugEnv = "Clear";
    }
}