using Godot;
using Godot.Collections;


public partial class Utils : Node
{
    public static Utils instance;

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
    }



    public void SaveGame()
    {
        RigidBody2D player = GameManager.instance.player;
        Gun playerGun = (Gun)player.GetNode("Node/Gun");

        string dataString;

        using (FileAccess file = FileAccess.Open("user://PlayerData.bin", FileAccess.ModeFlags.Write))
        {
            Dictionary data = new Dictionary
            {
                { "Level", GameManager.instance.currentLevel },
                { "Bullets", playerGun.Bullets},
                { "Magazins", playerGun.Magazins }
            };

            dataString = Json.Stringify(data);

            file.StoreString(dataString);
        }
    }

    

    public void LoadGame()
    {
        RigidBody2D player = GameManager.instance.player;
        Gun playerGun = (Gun)player.GetNode("Node/Gun");

        using (FileAccess file = FileAccess.Open("user://PlayerData.bin", FileAccess.ModeFlags.Read))
        {
            Dictionary data = (Dictionary)Json.ParseString(file.GetLine());

            GameManager.instance.currentLevel = (string)data["Level"];
            playerGun.LoadSaveData((int)data["Bullets"], (int)data["Magazins"]);
        }
    }



    public void SwitchLevel(string level)
    {
        if (GameManager.instance.player.GetParentOrNull<Node2D>() != null)
            GameManager.instance.player.GetParent().RemoveChild(GameManager.instance.player);

        SceneTree tree = GetTree();
        tree.ChangeSceneToFile($"res://Level/{level}.tscn");
        GameManager.instance.currentLevel = level;
    }



    public static Vector2 AngleToVector(float angle, bool useDegreesInsteadOfRadiance)
    {
        if(useDegreesInsteadOfRadiance)
            angle = Mathf.DegToRad(angle);

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
