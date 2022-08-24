using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.States;

namespace MonogameLearning.Engine;

public class MainGame : Game
{
    private BaseGameState _currentGameState;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private RenderTarget2D _renderTarget;
    private Rectangle _renderScaleRectangle;

    private int _designedResolutionWidth;
    private int _designedResolutionHeight;
    private float _designedResolutionAspectRatio;

    private BaseGameState _firstGameState;
    private bool _debug;
    public MainGame(int designedResolutionWidth, int designedResolutionHeight, BaseGameState firstGameState, bool debug = false)
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        
        _designedResolutionWidth = designedResolutionWidth;
        _designedResolutionHeight = designedResolutionHeight;
        _designedResolutionAspectRatio = _designedResolutionWidth / (float) _designedResolutionHeight;
        _firstGameState = firstGameState;

        _debug = debug;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _designedResolutionWidth;
        _graphics.PreferredBackBufferHeight = _designedResolutionHeight;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        _renderTarget = new RenderTarget2D(
            _graphics.GraphicsDevice, 
            _designedResolutionWidth, 
            _designedResolutionHeight, 
            false,
            SurfaceFormat.Color, 
            DepthFormat.None, 
            0, 
            RenderTargetUsage.DiscardContents);

        _renderScaleRectangle = GetScaleRectangle();
        base.Initialize();
    }

    /// <summary>
    /// Uses the current window size compared to the design resolution
    /// </summary>
    /// <returns>Scaled Rectangle</returns>
    private Rectangle GetScaleRectangle()
    {
        var variance = 0.5;
        var actualAspectRatio = Window.ClientBounds.Width / (float) Window.ClientBounds.Height;

        Rectangle scaleRectangle;

        if (actualAspectRatio <= _designedResolutionAspectRatio)
        {
            var presentHeight = (int) (Window.ClientBounds.Width / _designedResolutionAspectRatio + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int) (Window.ClientBounds.Height * _designedResolutionAspectRatio + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

            scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }

        return scaleRectangle;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        SwitchGameState(_firstGameState);
    }

    private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
    {
        SwitchGameState(e);
    }
    private void SwitchGameState(BaseGameState gameState)
    {
        if (_currentGameState != null)
        {
            _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification -= _currentGameState_OnEventNotification;
            _currentGameState.UnloadContent();
        }
        _currentGameState = gameState;
        _currentGameState.Initialize(Content, _designedResolutionWidth, _designedResolutionHeight, _debug);
        _currentGameState.LoadContent();
        _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;
    }

    private void _currentGameState_OnEventNotification(object sender, BaseGameStateEvent e)
    {
        switch (e)
        {
            case BaseGameStateEvent.GameQuit:
                Exit();
                break;
        }
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
        _currentGameState?.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _currentGameState.HandleInput(gameTime);
        _currentGameState.Update(gameTime);

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Render to the Render Target
        GraphicsDevice.SetRenderTarget(_renderTarget);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _currentGameState.Render(_spriteBatch);

        _spriteBatch.End();

        // Now render the scaled content
        _graphics.GraphicsDevice.SetRenderTarget(null);

        _graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

        _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}