using BepInEx;

namespace MoreWorldModifiers;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string ModName = "MoreWorldModifiers",
        ModAuthor = "Frogger",
        ModVersion = "1.2.0",
        ModGUID = $"com.{ModAuthor}.{ModName}";


    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion);
        LocalizationManager.Localizer.Load();
    }
}