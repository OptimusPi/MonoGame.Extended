using System.Collections.Generic;
using Demo.BricksGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;

namespace Demo.BricksGame
{
    public class Game1 : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private List<Brick> _bricks;
        private Paddle _paddle;
        private Ball _ball;
        private ViewportAdapter _viewportAdapter;

        public Game1()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            _camera = new Camera2D(_viewportAdapter);

            var brickTexture = Content.Load<Texture2D>("white-brick");
            var ballTexture = Content.Load<Texture2D>("white-ball");
            var paddleTexture = Content.Load<Texture2D>("paddle");

            _paddle = new Paddle(new TextureRegion2D(paddleTexture)) {Position = new Vector2(0, 440)};
            _ball = new Ball(new TextureRegion2D(ballTexture)) {Position = new Vector2(400, 240)};

            // bricks
            _bricks = new List<Brick>();

            var offset = new Vector2(15, 15);

            for (var y = 0; y < 9; y++)
            {
                for (var x = 0; x < 12; x++)
                {
                    var brickRegion = new TextureRegion2D(brickTexture);
                    var brick = new Brick(brickRegion);
                    brick.Position = offset + new Vector2(x*brick.Width, y*brick.Height);
                    _bricks.Add(brick);
                }
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _paddle.Center = new Vector2(mouseState.X, _paddle.Center.Y);

            if (_paddle.Position.X < 0)
                _paddle.Position = new Vector2(0, _paddle.Position.Y);

            if (_paddle.Position.X > _viewportAdapter.VirtualWidth - _paddle.Width)
                _paddle.Position = new Vector2(_viewportAdapter.VirtualWidth - _paddle.Width, _paddle.Position.Y);

            _ball.Position += _ball.Velocity * deltaTime;

            if (_ball.BoundingRectangle.Intersects(_paddle.BoundingRectangle))
                _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);

            if (_ball.Position.X < 0 || _ball.Position.X + _ball.Width > _viewportAdapter.VirtualWidth)
                _ball.Velocity = new Vector2(-_ball.Velocity.X, _ball.Velocity.Y);

            if (_ball.Position.Y < 0)
                _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);

            var hitABrick = false;

            for (var i = _bricks.Count - 1; i >= 0; i--)
            {
                var brick = _bricks[i];

                if (brick.BoundingRectangle.Intersects(_ball.BoundingRectangle))
                {
                    _bricks.Remove(brick);
                    hitABrick = true;
                }
            }

            if(hitABrick)
                _ball.Velocity = -_ball.Velocity;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            foreach (var brick in _bricks)
                brick.Draw(_spriteBatch);

            _paddle.Draw(_spriteBatch);
            _ball.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}