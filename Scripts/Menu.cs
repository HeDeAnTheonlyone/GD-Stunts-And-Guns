using Godot;

public partial class Menu : CanvasLayer
{
    private void OnPressNewGame()
    {
        Utils.instance.SwitchLevel("DebugWorld");
    }


    private void OnPressContinue()
    {
        GameManager.instance.PauseGame();
    }


    private void OnPressLoadGame()
    {
        Utils.instance.LoadGame();
        Utils.instance.SwitchLevel($"{GameManager.instance.currentLevel}");
    }

    private void OnPressRestartLevel()
    {
        GameManager.instance.player.QueueFree();
        GameManager.instance.player = (RigidBody2D)GameManager.instance.playerScene.Instantiate();
        OnPressLoadGame();
        GameManager.instance.PauseGame();
    }

    private void OnPressQuitGame()
    {
        if (GameManager.instance.currentLevel != "MainMenu")
            Utils.instance.SaveGame();

        GetTree().Quit();
    }
}
