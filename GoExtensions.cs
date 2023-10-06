namespace MoreWorldModifiers;

public static class GoExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
}