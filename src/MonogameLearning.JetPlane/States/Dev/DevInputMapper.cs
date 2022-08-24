using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.Engine.Input;

namespace MonogameLearning.JetPlane.States.Dev
{
    public class DevInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<DevInputCommand>();

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new DevInputCommand.DevQuit());
            }
            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new DevInputCommand.DevShoot());
            }
            return commands;
        }
    }
}