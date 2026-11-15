using Godot;
using System;
namespace Core.Managers;

public partial class SoundManager : Node
{
	public static SoundManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void PlaySound(
		NodePath node,
		float minPitch = 1f,
		float maxPitch = 1f,
		bool randomPitch = false)
	{
		var soundNode = GetNodeOrNull(node);

		switch (soundNode)
		{
			case AudioStreamPlayer player:
				Play(player, minPitch, maxPitch, randomPitch);
				break;

			case AudioStreamPlayer2D player2D:
				Play(player2D, minPitch, maxPitch, randomPitch);
				break;
		}
	}

	public void StopSound(NodePath node)
	{
		var soundNode = GetNodeOrNull(node);

		switch (soundNode)
		{
			case AudioStreamPlayer player:
				player.Stop();
				GD.Print("aa shit");
				break;

			case AudioStreamPlayer2D player2D:
				player2D.Stop();
				break;
		}
	}

	private static void Play(
		AudioStreamPlayer player,
		float minPitch,
		float maxPitch,
		bool randomPitch)
	{
		if (randomPitch)
			player.PitchScale = (float)GD.RandRange(minPitch, maxPitch);

		player.Play();
	}

	private static void Play(
		AudioStreamPlayer2D player,
		float minPitch,
		float maxPitch,
		bool randomPitch)
	{
		if (randomPitch)
			player.PitchScale = (float)GD.RandRange(minPitch, maxPitch);

		player.Play();
	}
}
