using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace MonogameLearning.Platformer.Scenes
{
    public class MainMenuScene : BaseScene
    {
        private VirtualButton _nextInput;
        public override Table Table { get; set; }
        public override void Initialize()
        {
            SetupScene();
            
            Table.Add(new Label("Main Menu").SetFontScale(5));
            
            Table.Row().SetPadTop(20);
            
            Table.Add(new Label("This is our main menu for our game!").SetFontScale(2));

            Table.Row().SetPadTop(40);

            var playButton = Table.Add(new TextButton("Start!", Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();

            playButton.OnClicked += PlayButton_onClicked;

            _nextInput = new VirtualButton();
            _nextInput.AddKeyboardKey(Keys.Enter);

        }

        private void PlayButton_onClicked(Button obj)
        {
            PlayNextScene();
        }

        public override void Update()
        {
            base.Update();
            if (_nextInput.IsPressed)
            {
                PlayNextScene();
            }
        }

        private void PlayNextScene()
        {
            Core.StartSceneTransition(new TextureWipeTransition(() => new GameplayScene())
            {
                TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink")
            });
        }
    }
}