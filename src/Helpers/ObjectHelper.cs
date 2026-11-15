using Godot;
using Godot.Collections;

namespace Utils;

public partial class ObjectHelper : Node
{
	private static ObjectHelper Instance { get; set; }
	private static readonly Dictionary<string, PackedScene> Cache = new();
	private static readonly ObjectData Objects =
		GD.Load<ObjectData>("res://Data/Objects.tres");

	public override void _Ready()
	{
		Instance = this;
	}

	public static T Create<T>(string sceneName = "", Vector2 position = default, NodePath parentPath = null)
		where T : Node
	{
		var parent = parentPath == null
			? Instance
			: Instance.GetNodeOrNull(parentPath);

		if (parent == null)
		{
			GD.PrintErr($"ObjectHelper: parent not found at path: {parentPath}");
			return null;
		}

		if (!Cache.ContainsKey(sceneName))
		{
			var path = Objects.Object[sceneName];
			Cache[sceneName] = GD.Load<PackedScene>(path);
		}

		var scene = Cache[sceneName];
		var instance = scene.Instantiate<T>();

		if (instance is Node2D node2D)
			node2D.Position = position;
		if (instance is CanvasLayer canvasLayer)
			canvasLayer.Offset = position;
		
		parent.AddChild(instance);
		return instance;
	}
}
