using MonogameLearning.Engine.Input;

namespace MonogameLearning.JetPlane.States.Dev
{
    public class DevInputCommand : BaseInputCommand
    {
        public class DevQuit : DevInputCommand { }
        public class DevShoot : DevInputCommand { }
    }
}