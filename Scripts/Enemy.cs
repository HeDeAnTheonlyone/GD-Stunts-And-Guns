using Godot;

public partial class Enemy : RigidBody2D
{
    [ExportCategory("Settings")]
    [Export] private bool followPlayer = false;
    [Export] private float speed = 100.0f;

	PackedScene deathEffect;

    private bool following = false;

    public override void _Ready()
    {
        deathEffect = (PackedScene)GD.Load("res://Objects/VFX/DeathEffect.tscn");
    }


    public override void _PhysicsProcess(double delta)
    {
        if (following)
        {
            Vector2 dir = (GameManager.instance.player.Position - Position).Normalized();
            ApplyTorqueImpulse(dir.X * speed);
        }
    }


    public void Death()
	{
        Node2D deathEffectInstance = (Node2D)deathEffect.Instantiate();
        deathEffectInstance.Position = Position;
        CallDeferred("AddDeathEffectToScene", deathEffectInstance);

        QueueFree();
    }

    private void AddDeathEffectToScene(Node2D instance) => GetNode("../").AddChild(instance);



    private void OnFollowAreaEnter(Node2D body)
    {
        following = true;
    }

    private void OnFollowAreaExit(Node2D body) => following = false;

    private void OnContact(Node2D body)
	{
		if (body is Player)
			body.Call("Death");
	}
}
