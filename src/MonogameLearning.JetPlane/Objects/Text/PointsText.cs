using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Engine.Objects;

namespace MonogameLearning.JetPlane.Objects.Text
{
    public class PointsText : BaseTextObject
    {
        private int _nbPoints = -1;

        public int NbPoints {
            get
            {
                return _nbPoints;
            }
            set
            {
                _nbPoints = value;
                Text = $"Points: {_nbPoints}";
            }
        }
        public PointsText(SpriteFont font) : base(font)
        {
        }
    }
}