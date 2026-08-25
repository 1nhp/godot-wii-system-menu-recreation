using Godot;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UI.Components;

public partial class ResolutionManager : Node
{
	private readonly Dictionary<string, Texture2D> _cache = new();
	public static ResolutionManager Instance { get; set; }

	public string resolution = "_720";
	
	public override void _Ready()
	{
		Instance = this;
		ApplyResolution(GetTree().CurrentScene, "_720");
		ApplyResolution(Cursor.Instance, "_720");
	
		SceneManager.Instance.SceneChanged += OnSceneChanged;
	}

	public void SetResolution(string resolutionS = "_720")
	{
		ApplyResolution(GetTree().CurrentScene, resolutionS);
		ApplyResolution(Cursor.Instance, resolutionS);
		resolution = resolutionS;
	}
	
	private async void OnSceneChanged(Node newScene)
	{
		ApplyResolution(newScene, "_720");
	}

	private void ApplyResolution(Node node, string targetSuffix)
	{
		if (node is TextureRect textureRect && textureRect.Texture != null)
		{
			textureRect.Texture = ReplaceTexture(textureRect.Texture, targetSuffix);
		}
		else if (node is Button button)
		{
			var styleBox = button.GetThemeStylebox("normal") as StyleBoxTexture;

			if (styleBox != null && styleBox.Texture != null)
			{
				var newStyleBox = styleBox.Duplicate() as StyleBoxTexture;
				newStyleBox.Texture = ReplaceTexture(styleBox.Texture, targetSuffix);
				newStyleBox.RegionRect = new Rect2(Vector2.Zero, newStyleBox.Texture.GetSize());
				
				button.AddThemeStyleboxOverride("normal", newStyleBox);
				button.AddThemeStyleboxOverride("pressed", newStyleBox);
				button.AddThemeStyleboxOverride("hover", newStyleBox);
			}
		}

		foreach (Node child in node.GetChildren())
			ApplyResolution(child, targetSuffix);
	}

	private Texture2D ReplaceTexture(Texture2D texture, string targetSuffix)
	{
		if (texture == null) return null;

		string oldPath = texture.ResourcePath;
		string newPath = GetVariantPath(oldPath, targetSuffix);
		
		if (oldPath == newPath) return texture;
		Texture2D newTexture = LoadTexture(newPath);
		
		if (newTexture == null) return texture;
		return newTexture;
	}

	private Texture2D LoadTexture(string path)
	{
		if (_cache.TryGetValue(path, out Texture2D texture))
			return texture;
		
		texture = GD.Load<Texture2D>(path);
		return texture;
	}

	private static string GetVariantPath(string path, string targetSuffix)
	{
		string baseName = path.GetBaseName();
		
		baseName = Regex.Replace(
			baseName,
			@"_(720|1080|4K)(_(720|1080|4K))?$",
			""
		);

		return $"{baseName}{targetSuffix}.{path.GetExtension()}";
	}
}
