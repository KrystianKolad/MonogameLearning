using Microsoft.Xna.Framework;

namespace MonogameLearning.Engine.Objects.Collisions
{
    public class BoundingBox
    {
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            }
        }

        public BoundingBox(Vector2 position, float width, float height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingBox otherBB)
        {
            return (Position.X < otherBB.Position.X + otherBB.Width &&
                Position.X + Width > otherBB.Position.X &&
                Position.Y < otherBB.Position.Y + otherBB.Height &&
                Position.Y + Height > otherBB.Position.Y);
        }
    }
}