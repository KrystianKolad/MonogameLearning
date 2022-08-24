using Microsoft.Xna.Framework;
using MonogameLearning.Engine.Input;
using MonogameLearning.Engine.Particles;
using MonogameLearning.Engine.States;
using MonogameLearning.JetPlane.Objects;
using MonogameLearning.JetPlane.Particles;

namespace MonogameLearning.JetPlane.States.Dev
{
    public class DevState : BaseGameState
    {
        private const string Texture = "Particles/Explosion";
        private Emitter _emitter;

        public override void LoadContent()
        {
            var position = new Vector2(_viewportWidth / 2, _viewportHeight / 2);
            _emitter = new ExplosionEmitter(LoadTexture(Texture), position);
            AddGameObject(_emitter);
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is DevInputCommand.DevQuit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime) 
        {
            _emitter.Update(gameTime);
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new DevInputMapper());
        }
    }
}