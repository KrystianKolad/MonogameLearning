using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.Objects;

namespace MonogameLearning.JetPlane.Objects
{
    public class TerrainBackground : BaseGameObject
    {
        private float ScrollingSpeed;
        public TerrainBackground(Texture2D texture, float scrollingSpeed)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
            ScrollingSpeed = scrollingSpeed;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;

            var sourceRectangle = new Rectangle(0,0, _texture.Width, _texture.Height);

            for (int nbVertical = -1; nbVertical < viewport.Height / _texture.Height +1; nbVertical++)
            {
                var y = (int)_position.Y + nbVertical*_texture.Height;
                for (int nbHorizontal = 0; nbHorizontal < viewport.Width / _texture.Width +1 ; nbHorizontal++)
                {
                    var x = (int) _position.X + nbHorizontal * _texture.Width;
                    var destRectangle = new Rectangle(x,y,_texture.Width, _texture.Height);
                    spriteBatch.Draw(_texture, destRectangle, sourceRectangle, Color.White);
                }
            }
            _position.Y = (int)(_position.Y + ScrollingSpeed) % _texture.Height;
        }
    }
}