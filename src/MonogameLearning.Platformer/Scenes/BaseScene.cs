using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace MonogameLearning.Platformer.Scenes
{
    public abstract class BaseScene : Scene
    {
        private VirtualButton _exitButton;
        abstract public Table Table { get; set; }
        public void SetupScene()
        {
            AddRenderer(new DefaultRenderer());
            var UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            Table = UICanvas.Stage.AddElement(new Table());
            Table.SetFillParent(true).Top().PadLeft(10).PadTop(50);
            _exitButton = new VirtualButton();
            _exitButton.AddKeyboardKey(Keys.Escape);
        }

        public override void Update()
        {
            base.Update();
            if (_exitButton.IsPressed)
            {
                Core.Exit();
            }

        }
    }
}