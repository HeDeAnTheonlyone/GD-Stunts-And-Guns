using Godot;
using System.Collections.Generic;

[Tool]
public partial class LaunchPad : Node2D
{
	[ExportCategory("Settings")]
	private Vector2 dir = new Vector2(1, 0);
	private float facing = 0;
	[Export] private Facing Facing
	{
		get => FacingHelper.GetFacingName((int)facing);
		set
		{
			facing = (float)value;
            RotationDegrees = facing;
			dir = Utils.AngleToVector(facing, true);
        }
    }
	[Export(PropertyHint.Range, "0, 100, or_greater")] private float LaunchStrength { get; set; } = 1;
	[Export] private ForceType forceType = ForceType.Constant;

    private const float baseLaunchMultiplier = 100.0f;
    private List<RigidBody2D> bodiesInsideLaunchPad = new List<RigidBody2D>();



	public override void _PhysicsProcess(double delta)
    {
		foreach (RigidBody2D body in bodiesInsideLaunchPad)
		{
			switch (forceType)
			{
				case ForceType.Constant:
                    body.ApplyCentralForce(dir * baseLaunchMultiplier * LaunchStrength);
                    break;

                case ForceType.Impuls:
                    body.ApplyCentralImpulse(dir * baseLaunchMultiplier * LaunchStrength);
                    break;
            }
		}
    }



    private void OnEntityEnter(Node2D body)
	{
		if (body is RigidBody2D)
			bodiesInsideLaunchPad.Add((RigidBody2D)body);
	}

	private void OnEntityExit(Node2D body)
	{
		bodiesInsideLaunchPad.Remove((RigidBody2D)body);
	}
}
