using Godot;

public partial class DeathEffect : Node2D
{
    AudioStreamPlayer sfxSquish;
    GpuParticles2D vfxBlood;

    public override void _Ready()
    {
        sfxSquish = GetNode<AudioStreamPlayer>("Squish");
        vfxBlood = GetNode<GpuParticles2D>("Blood");

        sfxSquish.Play();
        vfxBlood.Emitting = true;
    }

    private void OnTimerRunsOut() => QueueFree();
}
