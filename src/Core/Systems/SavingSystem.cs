using Godot;
using Newtonsoft.Json;

public partial class SavingSystem : Node
{
    private const string SavePath = "user://savedata.json";

    public static void Save(AppSaveData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        file.StoreString(json);
    }

    public static AppSaveData Load()
    {
        if (!FileAccess.FileExists(SavePath))
            return new AppSaveData();

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();

        return JsonConvert.DeserializeObject<AppSaveData>(json)
               ?? new AppSaveData();
    }
}