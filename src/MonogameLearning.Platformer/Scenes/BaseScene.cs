using Nez;
using Nez.UI;

namespace MonogameLearning.Platformer.Scenes
{
    public abstract class BaseScene : Scene
    {
        abstract public Table Table { get; set; }
        public void SetupScene()
        {
            AddRenderer(new DefaultRenderer());
            var UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            Table = UICanvas.Stage.AddElement(new Table());
            Table.SetFillParent(true).Top().PadLeft(10).PadTop(50);
        }
    }
}