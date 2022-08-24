using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.Engine.Input;
using MonogameLearning.Engine.States;
using MonogameLearning.JetPlane.Objects;
using MonogameLearning.JetPlane.States.Gameplay;

namespace MonogameLearning.JetPlane.States.Splash
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splash")));
            
            var track1 = LoadSound("Sounds/Music/FutureAmbient_3").CreateInstance();
            var track2 = LoadSound("Sounds/Music/FutureAmbient_4").CreateInstance();
            _soundManager.SetSoundtrack(new List<SoundEffectInstance>{track1, track2});
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchState(new GameplayState());
            }
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }

        public override void UpdateGameState(GameTime gameTime)
        {
        }
    }
}