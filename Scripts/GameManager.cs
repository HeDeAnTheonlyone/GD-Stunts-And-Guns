using Godot;

public partial class GameManager : Node
{
    public PackedScene playerScene = (PackedScene)GD.Load("res://Objects/Character/Player.tscn");

    public string currentLevel = "DebugWorld";
    public RigidBody2D player;

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

        player = (RigidBody2D)playerScene.Instantiate();
    }



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Pause") && GetNode("../MainMenu") == null)
            PauseGame();

        if (@event.IsActionPressed("Save"))
            Utils.instance.SaveGame();
    }



    public void PauseGame()
    {
        if (GetTree().Paused)
        {
            GetTree().Paused = false;
            player.GetNode<CanvasLayer>("PauseMenu").Visible = false;
        }
        else
        {
            GetTree().Paused = true;
            player.GetNode<CanvasLayer>("PauseMenu").Visible = true;
        }
    }
}
