using Godot;

public partial class MainMenu : Node2D
{
    private void OnPressNewGame()
    {
        Utils.instance.SwitchLevel("DebugWorld");
    }



    private void OnPressLoadGame()
    {
        Utils.instance.LoadGame();
        Utils.instance.SwitchLevel($"{GameManager.instance.currentLevel}");
    }



    private void OnPressQuit() => GetTree().Quit();
}
