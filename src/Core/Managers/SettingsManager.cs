using Godot;
namespace WSMR.Core.Managers;
using Godot.Collections;
public partial class SettingsManager : Node
{
    public static SettingsManager Instance { get; private set; }
    public AppSaveData CurrentSettings { get; private set; } = new AppSaveData();
    public Array<string> TexturePacksList { get; private set; } = new();
        
    public override void _Ready()
    {
        Instance = this;
        LoadAndApplySettings();
    }

    public void LoadSettings()
    {
        CurrentSettings = SavingSystem.Load();
    }

    public void FetchTexturePacks()
    {
        TexturePacksList.Clear();
        const string path = "user://texture_packs/";

        using var dir = DirAccess.Open(path);

        if (dir == null)
        {
            GD.PushWarning($"Could not open texture packs directory: {path}");
            return;
        }

        dir.ListDirBegin();

        string fileName;
        while ((fileName = dir.GetNext()) != "")
        {
            if (dir.CurrentIsDir())
                continue;

            GD.Print($"Found file: {fileName}");
            TexturePacksList.Add(fileName);
            GD.Print(TexturePacksList);
        }

        dir.ListDirEnd();
    }
    
    public bool ApplyTexturePack(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        string path = $"user://texture_packs/{fileName}";

        if (!FileAccess.FileExists(path))
        {
            GD.PushWarning($"Texture pack not found: {path}");
            return false;
        }

        bool success = ProjectSettings.LoadResourcePack(path, replaceFiles: true);

        if (!success)
        {
            GD.PushWarning($"Failed to load texture pack: {path}");
            return false;
        }

        CurrentSettings.CurrentTexturePack = fileName;
        SaveSettings();

        return true;
    }
    
    public void SaveSettings()
    {
        SavingSystem.Save(CurrentSettings);
    }
    
    public void LoadAndApplySettings()
    {
        CurrentSettings = SavingSystem.Load();
        
        Engine.MaxFps = CurrentSettings.FPS;
        FPSCounter.Instance.Toggle(CurrentSettings.ShowFramerate);
        IsVsyncEnabled = CurrentSettings.Vsync;
        IsFullscreenEnabled = CurrentSettings.Fullscreen;
        
        string path = $"user://texture_packs/{CurrentSettings.CurrentTexturePack}";
        ProjectSettings.LoadResourcePack(path, replaceFiles: true);
    }
    
    public bool IsVsyncEnabled
    {
        get => DisplayServer.WindowGetVsyncMode() != DisplayServer.VSyncMode.Disabled;
        set => DisplayServer.WindowSetVsyncMode(value ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
    }
    
    public bool IsFullscreenEnabled
    {
        get => DisplayServer.WindowGetMode() != DisplayServer.WindowMode.Windowed;
        set => DisplayServer.WindowSetMode(value ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
    }
}