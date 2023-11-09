using Godot;

public partial class Player : RigidBody2D
{
    Node2D gun;
    PackedScene dummyGun;
    PackedScene deathEffect;
    Sprite2D texture;

    public override void _Ready()
    {
        gun = GetNode<Node2D>("Node/Gun");
        dummyGun = (PackedScene)GD.Load("res://Objects/Items/DummyGun.tscn");
        deathEffect = (PackedScene)GD.Load("res://Objects/VFX/DeathEffect.tscn");
        texture = GetNode<Sprite2D>("Texture");

        GameManager.instance.player = this;
    }



    public void Death()
    {
        ProcessMode = ProcessModeEnum.Disabled;
        SetDeferred("freeze", true);
        texture.Modulate = texture.Modulate with { A = 0 };
        gun.Modulate = gun.Modulate with { A = 0 };

        SpawnDeathEffect();

        DropGun();
    }

    private void AddDummyGunToScene(RigidBody2D instance) => GetNode("../").AddChild(instance);
    private void AddDeathEffectToScene(Node2D instance) => GetNode("../").AddChild(instance);



   private void DropGun()
    {
        RigidBody2D dummyGunInstance = (RigidBody2D)dummyGun.Instantiate();
        dummyGunInstance.Position = Position;
        dummyGunInstance.Rotation = gun.GlobalRotation;
        dummyGunInstance.LinearVelocity = LinearVelocity;
        CallDeferred("AddDummyGunToScene", dummyGunInstance);

    }



    private void SpawnDeathEffect()
    {
        Node2D deathEffectInstance = (Node2D)deathEffect.Instantiate();
        deathEffectInstance.Position = Position;
        CallDeferred("AddDeathEffectToScene", deathEffectInstance);
    }
}
