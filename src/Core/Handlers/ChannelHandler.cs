using Godot;
using UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace WSMR.Core.Handlers;

public partial class ChannelHandler : Node
{
    [Export] public FlowContainer ChannelGrid;

    public override void _Ready()
    {
        LoadChannels();
        GetChannelAssetPaths();
        InstantiateChannels();
    }

    public List<string> ChannelFiles = new List<string> { };
    public List<string> Scenes = new List<string> { };

    public void LoadChannels()
    {
        string[] files = DirAccess.GetFilesAt("user://channels");
        foreach (string file in files)
        {
            var path = "user://channels/" + file;
            var result = ProjectSettings.LoadResourcePack(path);
        }
    }

    public void GetChannelAssetPaths()
    {
        string[] channelDirs = DirAccess.GetDirectoriesAt("res://Data/Channels/");
        foreach (string dirName in channelDirs)
        {
            string channelFolderPath = $"res://Data/Channels/{dirName}";
            string[] files = DirAccess.GetFilesAt(channelFolderPath);

            foreach (string file in files)
            {
                string cleanFileName = file.Replace(".remap", "").Replace(".import", "");
                string fullAssetPath = $"{channelFolderPath}/{cleanFileName}";

                if (!ChannelFiles.Contains(fullAssetPath))
                {
                    ChannelFiles.Add(fullAssetPath);
                }
            }

        }
        GD.Print(string.Join(", ", ChannelFiles));
    }

    public void InstantiateChannels(){
        List<string> sceneFiles = ChannelFiles
            .Where(file => file.EndsWith("icon.tscn", StringComparison.OrdinalIgnoreCase))
            .ToList();
        GD.Print(string.Join(", ", sceneFiles));

        foreach (string scenePath in sceneFiles)
        {
            for(int i = 0; i < 5; i++){
                var ChannelButton = ObjectHelper.Create<ChannelButton>("ChannelButton", new Vector2(0, 0), ChannelGrid.GetPath());
                var Mask = ChannelButton.GetNode<TextureRect>("TextureRect");

                PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath);

                Node instance = packedScene.Instantiate();
                Mask.AddChild(instance);
            }
        }
    }
}
