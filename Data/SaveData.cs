using System;
using Godot;

public class AppSaveData
{
    public int FPS { get; set; } = 60;
    public bool Vsync { get; set; } = false;
    public bool Fullscreen { get; set; } = false;
    public bool ShowFramerate { get; set; } = false;
    public string SpriteResolution { get; set; } = "_720";
    public int SpriteResolutionSelected { get; set; } = 0;
    public int FrameRateSelected { get; set; } = 0;
    public string CurrentTexturePack { get; set; } = "";

}