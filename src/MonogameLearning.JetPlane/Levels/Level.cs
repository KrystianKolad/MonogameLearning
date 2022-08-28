using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameLearning.Engine.States;

namespace MonogameLearning.JetPlane.Levels
{
    public class Level
    {
        private LevelReader _levelReader;
        private List<List<BaseGameStateEvent>> _currentLevel;
        private int _currentLevelNumber;
        private int _currentLevelRow;

        private TimeSpan _startGameTime;
        private readonly TimeSpan TickTimeSpan = new TimeSpan(0, 0, 2);

        public int CurrentLevel => _currentLevelNumber;
        public bool LevelExists => _currentLevel.Count > 0;

        public event EventHandler<LevelEvents.GenerateEnemies> OnGenerateEnemies;
        public event EventHandler<LevelEvents.GenerateTurret> OnGenerateTurret;
        public event EventHandler<LevelEvents.StartLevel> OnLevelStart;
        public event EventHandler<LevelEvents.EndLevel> OnLevelEnd;
        public event EventHandler<LevelEvents.NoRowEvent> OnLevelNoRowEvent;
        public Level(LevelReader reader)
        {
            _levelReader = reader;
            _currentLevelNumber = 1;
            _currentLevelRow = 0;

            _currentLevel = _levelReader.LoadLevel(_currentLevelNumber);
        }

        public void LoadNextLevel()
        {
            _currentLevelNumber++;
            _currentLevelRow = 0;
            _currentLevel = _levelReader.LoadLevel(_currentLevelNumber);
        }

        public void Reset(bool fullReset = false)
        {
            _currentLevelRow = 0;
            if (fullReset)
            {
                _currentLevelNumber = 1;
                _currentLevel = _levelReader.LoadLevel(_currentLevelNumber);   
            }
        }

        public void GenerateLevelEvents(GameTime gameTime)
        {
            // nothing to do until tick time
            if (gameTime.TotalGameTime - _startGameTime < TickTimeSpan || !LevelExists)
            {
                return;
            }
            _startGameTime = gameTime.TotalGameTime;

            foreach (var e in _currentLevel[_currentLevelRow])
            {
                switch (e) 
                {
                    case LevelEvents.GenerateEnemies g:
                        OnGenerateEnemies?.Invoke(this, g);
                        break;

                    case LevelEvents.GenerateTurret g:
                        OnGenerateTurret?.Invoke(this, g);
                        break;

                    case LevelEvents.StartLevel s:
                        OnLevelStart?.Invoke(this, s);
                        break;

                    case LevelEvents.EndLevel s:
                        OnLevelEnd?.Invoke(this, s);
                        break;

                    case LevelEvents.NoRowEvent n:
                        OnLevelNoRowEvent?.Invoke(this, n);
                        break;
                }
            }

            _currentLevelRow++;
            if (_currentLevelRow >= _currentLevel.Count)
            {
                this.LoadNextLevel();
            }
        }
    }
}