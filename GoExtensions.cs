using UnityEngine;

namespace MoreWorldModifiers;

public static class GoExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component =>
        gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
}
