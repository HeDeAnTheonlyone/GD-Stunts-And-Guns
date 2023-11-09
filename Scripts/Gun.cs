using Godot;

public partial class Gun : Node2D
{
    [ExportCategory("Motion settings")]
    [Export] private float maxRotVelocity = 10f;
    [Export] private float recoilStrenth = 1.5f;

    [ExportCategory("Gun stats")]
    private int bullets = 7;
    [Export] public int Bullets
    {
        get
        {
            return bullets;
        }
        private set
        {
            bullets = value;
            bulletDisp.Value = bullets;
        }
    }
    private int magazins = 5;
    [Export] public int Magazins
    {
        get
        {
            return magazins;
        }
        private set
        {
            magazins = value;
            magazinDisp.Text = magazins.ToString();
        }
    }

    private RigidBody2D player;
    private RayCast2D trajectory;
    private PackedScene shootLine;
    private PackedScene dummyMagazin;
    private AnimatedSprite2D anim;
    private VSlider bulletDisp;
    private Label magazinDisp;
    private AnimationPlayer magazinAnim;
    private AudioStreamPlayer sfxShoot;
    private AudioStreamPlayer sfxReload;
    private AudioStreamPlayer sfxEmptyMagazinShooting;

    private Vector2 gunDir;
    private float mouseDistance;
    private const int maxGunDistance = 35;
    private const float yVelocityCompensation = 1.7f;
    private const float trajectoryLength = 1200f;
    private const float rotationalDamper = 0.2f;
    private bool reloading = false;
    


    public override void _Ready()
    {
        player = GetNode<RigidBody2D>("../../../Player");
        trajectory = GetNode<RayCast2D>("Trajectory");
        shootLine = GD.Load<PackedScene>("res://Objects/VFX/ShootEffect.tscn");
        dummyMagazin = GD.Load<PackedScene>("res://Objects/Items/DummyMagazin.tscn");
        anim = GetNode<AnimatedSprite2D>("Anim");
        bulletDisp = GetNode<VSlider>("../../HUD/Bullets");
        magazinDisp = GetNode<Label>("../../HUD/MagazinsCounter");
        magazinAnim = magazinDisp.GetNode<AnimationPlayer>("MagazinAnim");
        sfxShoot = GetNode<AudioStreamPlayer>("SFX/Shoot");
        sfxReload = GetNode<AudioStreamPlayer>("SFX/Reload");
        sfxEmptyMagazinShooting = GetNode<AudioStreamPlayer>("SFX/EmptyMagazinShooting");

        magazinAnim.AnimationFinished += OnFinishReloading;

        Bullets += 0;
        Magazins += 0;
    }



    public void LoadSaveData(int _bullets, int _magazins)
    {
        Bullets = _bullets;
        Magazins = _magazins;
    }



    // Handel Weapon Positioning
    public override void _Process(double delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        gunDir = (mousePos - player.Position).Normalized();
        mouseDistance = player.Position.DistanceTo(mousePos);
        Vector2 gunPos = (mouseDistance <= maxGunDistance ? gunDir * mouseDistance : gunDir * maxGunDistance);
        float gunRot = new Vector2(0, -1).AngleTo(mousePos - player.Position);

        MoveGun(gunPos, gunRot);
        MoveTrajectory();
    }

    private void MoveGun(Vector2 gunPos, float gunRot)
    {
        Position = gunPos + player.Position;
        Rotation = gunRot;

        if (gunDir.X > 0)
            anim.FlipH = true;
        else
            anim.FlipH = false;
    }
    private void MoveTrajectory()
    {
        trajectory.Position = Position;
        trajectory.Rotation = Rotation;
    }



    // Handel Player Input
    public override void _Input(InputEvent @event)
    {        
        if (@event.IsActionPressed("Attack"))
        {
            if (Bullets > 0)
                AttackAction();
            else
                sfxEmptyMagazinShooting.Play();
        }

        if (@event.IsActionPressed("Reload"))
        {
            if (Magazins > 0 && !reloading)
                ReloadAction();
            else
                sfxEmptyMagazinShooting.Play();
        }
    }

    private void AttackAction()
    {
        sfxShoot.Play();
        anim.Play("Shoot");

        ApplyMotionToPlayer();

        Bullets--;

        ManageTrajectory();
    }

    private void ApplyMotionToPlayer()
    {
        Vector2 playerImpuls = -gunDir * mouseDistance * recoilStrenth;
        playerImpuls.Y *= yVelocityCompensation;
        player.ApplyCentralImpulse(playerImpuls);
        player.AngularVelocity += -rotationalDamper * mouseDistance * gunDir.X;

        if (player.AngularVelocity < -maxRotVelocity)
            player.AngularVelocity = -maxRotVelocity;
        else if (player.AngularVelocity > maxRotVelocity)
            player.AngularVelocity = maxRotVelocity;
    }

    private void ManageTrajectory()
    {
        trajectory.ForceRaycastUpdate();

        Node2D hit = (Node2D)trajectory.GetCollider();
        Line2D shootLineInstance = (Line2D)shootLine.Instantiate();
        if (hit != null)
        {
            shootLineInstance.Points = new Vector2[] { Position, trajectory.GetCollisionPoint() };
            if (hit is Enemy)
                hit.Call("Death");
        }
        else
            shootLineInstance.Points = new Vector2[] { Position, Position + gunDir * trajectoryLength };

        CallDeferred("AddShootEffectToScene", shootLineInstance);
    }

    private void AddShootEffectToScene(Line2D instance) => GetNode("../../../").AddChild(instance);


    private void ReloadAction()
    {
        reloading = true;
        magazinAnim.Play("Reload");
        sfxReload.Play();
        Magazins--;
        Bullets = 7;

        DropMagazin();
    }

    private void OnFinishReloading(StringName animName) => reloading = false;



    private void DropMagazin()
    {
        RigidBody2D dummyMagazinInstance = (RigidBody2D)dummyMagazin.Instantiate();
        dummyMagazinInstance.Position = Position;
        dummyMagazinInstance.GetNode<Sprite2D>("Texture").FlipH = anim.FlipH;
        CallDeferred("AddDummyMagazinToScene", dummyMagazinInstance);
    }

    private void AddDummyMagazinToScene(RigidBody2D instance) => GetNode("../../../").AddChild(instance);
}
