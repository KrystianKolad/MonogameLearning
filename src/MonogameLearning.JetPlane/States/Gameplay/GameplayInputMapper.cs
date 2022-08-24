using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.Engine.Input;

namespace MonogameLearning.JetPlane.States.Gameplay
{
    public class GameplayInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<GameplayInputCommand>();
            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }

            if (state.IsKeyDown(Keys.R))
            {
                commands.Add(new GameplayInputCommand.GameRestart());
            }

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveLeft());
            }
            else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }
            else 
            {
                commands.Add(new GameplayInputCommand.PlayerStopsMoving());
            }

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveUp());
            }
            else if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveDown());
            }
            else 
            {
                commands.Add(new GameplayInputCommand.PlayerStopsMoving());
            }

            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new GameplayInputCommand.PlayerShoots());
            }

            return commands;
        }
    }
}