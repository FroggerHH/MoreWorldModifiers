using System.Reflection;
using JetBrains.Annotations;

namespace MoreWorldModifiers;

[PublicAPI]
public static class PrefabManager
{
    private static readonly Dictionary<BundleId, AssetBundle> bundleCache = new();

    private static readonly List<GameObject> prefabs = new();
    private static readonly List<GameObject> ZnetOnlyPrefabs = new();

    static PrefabManager()
    {
        Harmony harmony = new("org.bepinex.helpers.ItemManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ObjectDB), nameof(ObjectDB.CopyOtherDB)),
            postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(PrefabManager), nameof(Patch_ObjectDBInit))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ObjectDB), nameof(ObjectDB.Awake)),
            new HarmonyMethod(AccessTools.DeclaredMethod(typeof(PrefabManager), nameof(Patch_ObjectDBInit))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZNetScene), nameof(ZNetScene.Awake)),
            new HarmonyMethod(AccessTools.DeclaredMethod(typeof(PrefabManager), nameof(Patch_ZNetSceneAwake))));
    }

    public static AssetBundle RegisterAssetBundle(string assetBundleFileName, string folderName = "assets")
    {
        BundleId id = new() { assetBundleFileName = assetBundleFileName, folderName = folderName };
        if (!bundleCache.TryGetValue(id, out var assets))
            assets = bundleCache[id] =
                Resources.FindObjectsOfTypeAll<AssetBundle>().FirstOrDefault(a => a.name == assetBundleFileName) ??
                AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + $".{folderName}." +
                                               assetBundleFileName));

        return assets;
    }

    public static GameObject
        RegisterPrefab(string assetBundleFileName, string prefabName, string folderName = "assets")
    {
        return RegisterPrefab(RegisterAssetBundle(assetBundleFileName, folderName), prefabName);
    }

    public static GameObject RegisterPrefab(AssetBundle assets, string prefabName, bool addToObjectDb = false)
    {
        return RegisterPrefab(assets.LoadAsset<GameObject>(prefabName), addToObjectDb);
    }

    public static GameObject RegisterPrefab(GameObject prefab, bool addToObjectDb = false)
    {
        if (addToObjectDb)
            prefabs.Add(prefab);
        else
            ZnetOnlyPrefabs.Add(prefab);

        return prefab;
    }

    [HarmonyPriority(Priority.VeryHigh)]
    private static void Patch_ObjectDBInit(ObjectDB __instance)
    {
        foreach (var prefab in prefabs)
        {
            if (!__instance.m_items.Contains(prefab)) __instance.m_items.Add(prefab);

            void RegisterStatusEffect(StatusEffect? statusEffect)
            {
                if (statusEffect is not null && !__instance.GetStatusEffect(statusEffect.name.GetStableHashCode()))
                    __instance.m_StatusEffects.Add(statusEffect);
            }

            var shared = prefab.GetComponent<ItemDrop>().m_itemData.m_shared;
            RegisterStatusEffect(shared.m_attackStatusEffect);
            RegisterStatusEffect(shared.m_consumeStatusEffect);
            RegisterStatusEffect(shared.m_equipStatusEffect);
            RegisterStatusEffect(shared.m_setStatusEffect);
        }

        __instance.UpdateItemHashes();
    }

    [HarmonyPriority(Priority.VeryHigh)]
    private static void Patch_ZNetSceneAwake(ZNetScene __instance)
    {
        foreach (var prefab in prefabs.Concat(ZnetOnlyPrefabs)) __instance.m_prefabs.Add(prefab);
    }

    private struct BundleId
    {
        [UsedImplicitly] public string assetBundleFileName;
        [UsedImplicitly] public string folderName;
    }
}