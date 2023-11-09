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
            playerGun.CallDeferred("LoadSaveData", (int)data["Bullets"], (int)data["Magazins"]);
        }
    }



    public void SwitchLevel(string level)
    {
        if (GetNode<RigidBody2D>($"../{level}/Player") != null)
            GetNode<Node2D>($"../{level}").RemoveChild(GameManager.instance.player);

        SceneTree tree = GetTree();
        tree.ChangeSceneToFile($"res://Level/{level}.tscn");
        GameManager.instance.currentLevel = level;
    }
}
