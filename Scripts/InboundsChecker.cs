using Godot;

public partial class InboundsChecker : Area2D
{
    public void OnOutOfBounds(Node2D body)
    {
        if (body.IsInGroup("KillOnOutOfBounds") && body.ProcessMode != ProcessModeEnum.Disabled)
            body.Call("Death");
    }
}
