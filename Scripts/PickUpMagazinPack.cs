using Godot;

public partial class PickUpMagazinPack : Node2D
{
	private const float tweenTime = 0.5f; 

    private void OnPickUp(Node2D body)
	{
		body.GetNode<Gun>("Node/Gun").AddMagazins(5);

        GetNode<AudioStreamPlayer2D>("Collect").Play();

        Tween tween = CreateTween();
		
		tween.TweenProperty(this, "scale", new Vector2(2.0f, 2.0f), tweenTime);
		tween.Parallel().TweenProperty(GetNode<Sprite2D>("Texture"), "modulate:a", 0, tweenTime);
		tween.Parallel().TweenProperty(GetNode<GpuParticles2D>("Particles"), "modulate:a", 0, tweenTime);

		tween.TweenCallback(Callable.From(QueueFree));
    }


}
