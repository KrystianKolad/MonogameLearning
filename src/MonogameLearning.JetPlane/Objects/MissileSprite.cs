using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.Objects;
using MonogameLearning.JetPlane.Particles;

namespace MonogameLearning.JetPlane.Objects
{
    public class MissileSprite : BaseGameObject, IGameObjectWithDamage
    {
        private const float StartSpeed = 0.5f;
        private const float Acceleration = 0.15f;
        private const int EmitterOffsetX = 18;
        private const int EmitterOffsetY = -10;
        private float _speed = StartSpeed;
        private int _missileHeight;
        private int _missileWidth;

        private ExhaustEmitter _exhaustEmitter;
        public override Vector2 Position 
        {
            set 
            {
                base.Position = value;
                _exhaustEmitter.Position = new Vector2(_position.X + EmitterOffsetX, _position.Y + _missileHeight + EmitterOffsetY);
            }
        }

        public int Damage => 25;

        public MissileSprite(Texture2D texture, Texture2D exhautTexture)
        {
            _texture = texture;
            _exhaustEmitter = new ExhaustEmitter(exhautTexture, _position);

            var ratio = (float) _texture.Height / (float) _texture.Width;
            _missileWidth = 50;
            _missileHeight = (int)(_missileWidth*ratio);

            // note that the missile is scaled down! so it's bounding box must be scaled down as well
            var bbRatio = (float) _missileWidth / _texture.Width;

            var bbOriginalPositionX = 352;
            var bbOriginalPositionY = 7;
            var bbOriginalWidth = 150;
            var bbOriginalHeight = 500;

            var bbPositionX = bbOriginalPositionX * bbRatio;
            var bbPositionY = bbOriginalPositionY * bbRatio;
            var bbWidth = bbOriginalWidth * bbRatio;
            var bbHeight = bbOriginalHeight * bbRatio; 

            AddBoundingBox(new Engine.Objects.Collisions.BoundingBox(new Vector2(bbPositionX, bbPositionY), bbWidth, bbHeight));
        }

        public void Update(GameTime gameTime)
        {
            _exhaustEmitter.Update(gameTime);

            Position = new Vector2(Position.X, Position.Y - _speed);
            _speed += Acceleration;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var destRectangle = new Rectangle((int)Position.X, (int) Position.Y, _missileWidth, _missileHeight);
            spriteBatch.Draw(_texture, destRectangle, Color.White);

            _exhaustEmitter.Render(spriteBatch);
        }
    }
}