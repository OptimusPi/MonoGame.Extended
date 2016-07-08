using Microsoft.Xna.Framework;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.TextureAtlases;

namespace Demo.BricksGame.Entities
{
    public class Ball : Entity
    {
        public Ball(TextureRegion2D textureRegion)
            : base(textureRegion)
        {
            Velocity = new Vector2(200, 100);
        }

        public Vector2 Velocity { get; set; }
        public CircleF BoundingCircle => new CircleF(Center, TextureRegion.Width * 0.5f);
    }
}