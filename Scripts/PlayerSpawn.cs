using Godot;

public partial class PlayerSpawn : Node2D
{

	public override void _Ready()
	{
		GameManager.instance.player.Position = Position;
		GetParent().CallDeferred("add_child", GameManager.instance.player);
	}
}
