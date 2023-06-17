using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MoreWorldModifiers;

[PublicAPI]
public static class PrefabManager
{
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

    private struct BundleId
    {
        [UsedImplicitly] public string assetBundleFileName;
        [UsedImplicitly] public string folderName;
    }

    private static readonly Dictionary<BundleId, AssetBundle> bundleCache = new();

    public static AssetBundle RegisterAssetBundle(string assetBundleFileName, string folderName = "assets")
    {
        BundleId id = new() { assetBundleFileName = assetBundleFileName, folderName = folderName };
        if (!bundleCache.TryGetValue(id, out AssetBundle assets))
        {
            assets = bundleCache[id] =
                Resources.FindObjectsOfTypeAll<AssetBundle>().FirstOrDefault(a => a.name == assetBundleFileName) ??
                AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + $".{folderName}." +
                                               assetBundleFileName));
        }

        return assets;
    }

    private static readonly List<GameObject> prefabs = new();
    private static readonly List<GameObject> ZnetOnlyPrefabs = new();

    public static GameObject
        RegisterPrefab(string assetBundleFileName, string prefabName, string folderName = "assets") =>
        RegisterPrefab(RegisterAssetBundle(assetBundleFileName, folderName), prefabName);

    public static GameObject RegisterPrefab(AssetBundle assets, string prefabName, bool addToObjectDb = false) =>
        RegisterPrefab(assets.LoadAsset<GameObject>(prefabName), addToObjectDb);

    public static GameObject RegisterPrefab(GameObject prefab, bool addToObjectDb = false)
    {
        if (addToObjectDb)
        {
            prefabs.Add(prefab);
        }
        else
        {
            ZnetOnlyPrefabs.Add(prefab);
        }

        return prefab;
    }

    [HarmonyPriority(Priority.VeryHigh)]
    private static void Patch_ObjectDBInit(ObjectDB __instance)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (!__instance.m_items.Contains(prefab))
            {
                __instance.m_items.Add(prefab);
            }

            void RegisterStatusEffect(StatusEffect? statusEffect)
            {
                if (statusEffect is not null && !__instance.GetStatusEffect(statusEffect.name.GetStableHashCode()))
                {
                    __instance.m_StatusEffects.Add(statusEffect);
                }
            }

            ItemDrop.ItemData.SharedData shared = prefab.GetComponent<ItemDrop>().m_itemData.m_shared;
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
        foreach (GameObject prefab in prefabs.Concat(ZnetOnlyPrefabs))
        {
            __instance.m_prefabs.Add(prefab);
        }
    }
}