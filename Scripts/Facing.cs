using System.Collections.Generic;

public enum Facing
{
    Up = -90,
    Down = 90,
    Right = 0,
    Left = 180,
    UpRight = -45,
    UpLeft = -135,
    DownRight = 45,
    DownLeft = 135
}

public static class FacingHelper
{
    public static Dictionary<int, Facing> facingNames = new Dictionary<int, Facing>
    {
        { -90, Facing.Up },
        { 90, Facing.Down },
        { 0, Facing.Right },
        { 180, Facing.Left },
        { -45, Facing.UpRight },
        { -135, Facing.UpLeft },
        { 45, Facing.DownRight },
        { 135, Facing.DownLeft }
    };

    public static Facing GetFacingName(int value) => facingNames[value];
}
