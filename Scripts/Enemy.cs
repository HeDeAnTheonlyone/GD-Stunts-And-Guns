using Godot;

public partial class Enemy : RigidBody2D
{
	PackedScene deathEffect;

    public override void _Ready()
    {
        deathEffect = (PackedScene)GD.Load("res://Objects/VFX/DeathEffect.tscn");
    }

    public void Death()
	{
        Node2D deathEffectInstance = (Node2D)deathEffect.Instantiate();
        deathEffectInstance.Position = Position;
        CallDeferred("AddDeathEffectToScene", deathEffectInstance);

        QueueFree();

        
    }

    private void AddDeathEffectToScene(Node2D instance) => GetNode("../").AddChild(instance);



    private void OnContact(Node2D body)
	{
		if (body.Name == "Player")
		{
			body.Call("Death");
		}
	}
}
