using Godot;

public partial class ShootEffect : Line2D
{
	public override void _Process(double delta)
	{
		Modulate = Modulate with { A = Modulate.A - 0.01f };
		if (Modulate.A < 0)
			QueueFree();
	}
}
