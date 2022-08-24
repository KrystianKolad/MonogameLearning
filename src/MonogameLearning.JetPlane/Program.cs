using System;
using MonogameLearning.JetPlane.States.Splash;

using var game = new MonogameLearning.Engine.MainGame(1280, 720, new SplashState());
game.IsFixedTimeStep = true;
game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
game.Run();