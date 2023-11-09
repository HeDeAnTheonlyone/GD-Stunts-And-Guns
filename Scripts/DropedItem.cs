using Godot;

public partial class DropedItem : RigidBody2D
{
    RandomNumberGenerator rng;

    public override void _Ready()
    {
        rng = new RandomNumberGenerator();
        AngularVelocity = rng.RandiRange(-20, 20);
        if (Name != "DummyGun")
            LinearVelocity = LinearVelocity with { Y = LinearVelocity.Y - 250 };
    }

    private void OnTimerRunOutDelete() => QueueFree();
}
