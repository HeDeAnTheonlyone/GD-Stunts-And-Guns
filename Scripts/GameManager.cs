using Godot;

public partial class GameManager : Node
{
    public string currentLevel = "DebugWorld";
    public RigidBody2D player;
    
    private bool paused = false;

    public static GameManager instance;

    public override void _Ready()
    {
        if (instance == null)
        {
            instance = this;
            SetProcess(false);
            ProcessMode = ProcessModeEnum.Always;
        }
        else
            QueueFree();

        PackedScene playerScene = (PackedScene)GD.Load("res://Objects/Character/Player.tscn");
        player = (RigidBody2D)playerScene.Instantiate();
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause"))
        {
            if (!paused)
            {
                GetTree().Paused = true;
                paused = true;
            }
            else
            {
                GetTree().Paused = false;
                paused = false;
            }
        }

        if (@event.IsActionPressed("Save"))
            Utils.instance.SaveGame();
    }
}
