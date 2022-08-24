using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.Objects;

namespace MonogameLearning.JetPlane.Objects.Text
{
    public class LivesText : BaseTextObject
    {
        private int _nbLives = -1;

        public int NbLives {
            get
            {
                return _nbLives;
            }
            set
            {
                _nbLives = value;
                Text = $"Lives: {_nbLives}";
            }
        }
        public LivesText(SpriteFont font) : base(font)
        {
        }
    }
}