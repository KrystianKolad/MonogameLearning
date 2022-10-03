using Nez;

namespace MonogameLearning.Platformer;

public class MainGame : Core
{
    protected override void Initialize()
    {
        base.Initialize();

        Scene = new Scenes.MainMenuScene();
    }
}
