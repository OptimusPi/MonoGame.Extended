using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.TextureAtlases;

namespace Demo.BricksGame.Entities
{
    public abstract class Entity
    {

        protected Entity(TextureRegion2D textureRegion)
        {
            TextureRegion = textureRegion;
        }

        public TextureRegion2D TextureRegion { get; }
        public Vector2 Position { get; set; }
        public int Width => TextureRegion.Width;
        public int Height => TextureRegion.Height;
        public Color Color => Color.White;

        public RectangleF BoundingRectangle => new RectangleF(Position, TextureRegion.Size);

        public Vector2 Center
        {
            get { return Position + TextureRegion.Size * 0.5f; }
            set { Position = value - TextureRegion.Size * 0.5f; }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureRegion, Position, Color);
        }
    }
}