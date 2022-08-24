using MonogameLearning.Engine.Input;

namespace MonogameLearning.JetPlane.States.Gameplay
{
    public class GameplayInputCommand : BaseInputCommand 
    {
        public class GameExit : GameplayInputCommand { }
        public class GameRestart : GameplayInputCommand { }
        public class PlayerMoveLeft : GameplayInputCommand { }
        public class PlayerMoveRight : GameplayInputCommand { }
        public class PlayerMoveUp : GameplayInputCommand { }
        public class PlayerMoveDown : GameplayInputCommand { }
        public class PlayerStopsMoving : GameplayInputCommand { }
        public class PlayerShoots : GameplayInputCommand { }
    }
}