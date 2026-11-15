using Godot;
using System;
namespace Core.Managers;

public partial class MusicManager : Node
{
	public static MusicManager Instance { get; private set; }
	
	
	public override void _Ready()
	{
		Instance = this;
	}

	public void PlayMusic(NodePath node)
	{
		var soundNode = GetNodeOrNull(node);
		
		switch (soundNode)
		{
			case AudioStreamPlayer player:
				if (player.Playing)
					return;

				Play(player);
				break;

			case AudioStreamPlayer2D player2D:
				if (player2D.Playing)
					return;

				Play(player2D);
				break;

			default:
				GD.PushWarning(
					$"MusicManager W001: No compatible audio player found at '{node}'."
				);
				break;
		}
	}

	public void StopMusic(NodePath node)
	{
		var musicNode = GetNodeOrNull(node);

		switch (musicNode)
		{
			case AudioStreamPlayer player:
				player.Stop();
				break;

			case AudioStreamPlayer2D player2D:
				player2D.Stop();
				break;
		}
	}

	public void PauseMusic(NodePath node, bool pause)
	{
		var musicNode = GetNodeOrNull(node);

		switch (musicNode)
		{
			case AudioStreamPlayer player:
				player.StreamPaused = pause;
				break;

			case AudioStreamPlayer2D player2D:
				player2D.StreamPaused = pause;
				break;
		}
	}
	
	private static void Play(AudioStreamPlayer player)
	{
		player.Play();
	}
	
	private static void Play(AudioStreamPlayer2D player)
	{
		player.Play();
	}
}