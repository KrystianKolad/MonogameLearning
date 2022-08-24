using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.Input;
using MonogameLearning.Engine.Objects;
using MonogameLearning.Engine.Sound;

namespace MonogameLearning.Engine.States
{
    public abstract class BaseGameState
    {
        private const string FallbackTexture = "Engine/emptyTexture";
        private const string FallbackSound = "Engine/emptySound";
        private const string FallbackFont = "Engine/emptyFont";
        private bool _debug = false;
        private ContentManager _contentManager;
        protected int _viewportHeight;
        protected int _viewportWidth;
        protected InputManager InputManager { get; set; }
        protected SoundManager _soundManager = new SoundManager();

        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

        public void Initialize(ContentManager contentManager, int viewportWIdth, int viewportHeight, bool debug = false)
        {
            _contentManager = contentManager;
            _viewportWidth = viewportWIdth;
            _viewportHeight = viewportHeight;

            _debug = debug;

            SetInputManager();
        }

        protected abstract void SetInputManager();
        public abstract void HandleInput(GameTime gameTime);
        public abstract void UpdateGameState(GameTime gameTime);
        public abstract void LoadContent();
        public void UnloadContent()
        {
            _contentManager.Unload();
        }
        public void Update(GameTime gameTime) 
        {
            UpdateGameState(gameTime);
            _soundManager.PlaySoundtrack();
        }
        public event EventHandler<BaseGameState> OnStateSwitched;
        protected void SwitchState(BaseGameState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }

        public event EventHandler<BaseGameStateEvent> OnEventNotification;
        protected void NotifyEvent(BaseGameStateEvent eventType)
        {
            OnEventNotification?.Invoke(this, eventType);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(eventType);
            }

            _soundManager.OnNotify(eventType);
        }

        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        protected void RemoveGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
                if (_debug)
                {
                    gameObject.RenderBoundingBoxes(spriteBatch);
                }
            }
        }

        protected Texture2D LoadTexture(string textureName)
        {
            try
            {
                return  _contentManager.Load<Texture2D>(textureName);
            }
            catch (ContentLoadException)
            {
                return _contentManager.Load<Texture2D>(FallbackTexture);
            }
        }

        protected SoundEffect LoadSound(string soundName)
        {
            try
            {
                return  _contentManager.Load<SoundEffect>(soundName);
            }
            catch (ContentLoadException)
            {
                return _contentManager.Load<SoundEffect>(FallbackSound);
            }
        }

        protected SpriteFont LoadFont(string fontName)
        {
            try
            {
                return  _contentManager.Load<SpriteFont>(fontName);
            }
            catch (ContentLoadException)
            {
                return _contentManager.Load<SpriteFont>(FallbackFont);
            }
        }
    }
}