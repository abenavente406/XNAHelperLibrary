using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameHelperLibrary; // <-- Always include the library when dealing with the library
using GameHelperLibrary.Shapes;

namespace Library_Demo
{

    /// <summary>
    /// This is a demo to showcase usage of the GameHelperLibrary.  The source code for that is also attached.
    /// 
    /// @author: Anthony Benavente
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Showcasing the rpg animation capablilty
        SpriteSheet playerSheet;
        Animation walkingDown, walkingLeft, walkingRight, walkingUp;

        // Lucas is that little guy that's in black and white
        Vector2 lucasPosition = Vector2.Zero;
        float lucasX, lucasY = 300;
        float movementSpeedY = 0.0f, gravity = 0.50f;
        SpriteSheet lucasLeftSheet;
        SpriteSheet lucasRightSheet;
        Texture2D[] lucasRight, lucasLeft, lucasNoMovement, lucasJumpRight, lucasJumpLeft;
        Animation movementRight, movementLeft, noMovement, jumpRight, jumpLeft;
        private bool isJumping = false;

        private DrawableRectangle rect, rect2;
        private DrawableCircle circle, circle2;

        // The direction lucas is facing
        private int lucasDir = 0; // 0: no moving, 1 : right, 2: left

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Initialize the playersheets. See the source code for explanations.
            // XML documentation is also completed.
            playerSheet = new SpriteSheet(this.Content.Load<Texture2D>("EntitySprites"), 32, 32, graphics.GraphicsDevice);
            lucasLeftSheet = new SpriteSheet(this.Content.Load<Texture2D>("lucasleft"), 40, 47, graphics.GraphicsDevice);
            lucasRightSheet = new SpriteSheet(this.Content.Load<Texture2D>("lucasright"), 40, 47, graphics.GraphicsDevice);

            // Different methods to load the texture array
            Texture2D[] walkDownImages = new Texture2D[] { playerSheet.getSubImage(3, 0), playerSheet.getSubImage(4, 0), 
                 playerSheet.getSubImage(5, 0), playerSheet.getSubImage(4, 0) };
            Texture2D[] walkLeftImages = new Texture2D[] { playerSheet.getSubImage(3, 1), playerSheet.getSubImage(4, 1), 
                 playerSheet.getSubImage(5, 1), playerSheet.getSubImage(4, 1) };
            Texture2D[] walkRightImages = new Texture2D[] { playerSheet.getSubImage(3, 2), playerSheet.getSubImage(4, 2), 
                 playerSheet.getSubImage(5, 2), playerSheet.getSubImage(4, 2) };
            Texture2D[] walkUpImages = new Texture2D[] { playerSheet.getSubImage(3, 3), playerSheet.getSubImage(4, 3), 
                 playerSheet.getSubImage(5, 3), playerSheet.getSubImage(4, 3) };

            // Another method to load the texture array
            lucasLeft = new Texture2D[7];
            lucasRight = new Texture2D[8];
            lucasNoMovement = new Texture2D[4];
            lucasJumpRight = new Texture2D[8];
            lucasJumpLeft = new Texture2D[8];

            for (int i = 7; i > 0; i--)
            {
                lucasLeft[7 - i] = lucasLeftSheet.getSubImage(i, 0);
            }
            for (int i = 0; i < 8; i++)
            {
                lucasRight[i] = lucasRightSheet.getSubImage(i, 1);
            }
            for (int i = 0; i < 4; i++)
            {
                lucasNoMovement[i] = lucasRightSheet.getSubImage(i, 0);
            }
            for (int i = 0; i < 8; i++)
            {
                lucasJumpRight[i] = lucasRightSheet.getSubImage(i, 2);
            }
            for (int i = 7; i > -1; i--)
            {
                lucasJumpLeft[7 - i] = lucasLeftSheet.getSubImage(i, 1);
            }

            // Create the animations... these are for lucas
            movementLeft = new Animation(lucasLeft, 75f);
            movementRight = new Animation(lucasRight, 75f);
            noMovement = new Animation(lucasNoMovement, 75f);
            jumpLeft = new Animation(lucasJumpLeft, 81f);
            jumpRight = new Animation(lucasJumpRight, 81f);

            // For the rpg
            walkingDown = new Animation(walkDownImages, 150f);
            walkingLeft = new Animation(walkLeftImages, 150f);
            walkingRight = new Animation(walkRightImages, 150f);
            walkingUp = new Animation(walkUpImages, 150f);

            rect = new DrawableRectangle(GraphicsDevice, new Vector2(100, 155), Color.Red, true);
            rect2 = new DrawableRectangle(GraphicsDevice, new Vector2(100, 155), Color.Red, false);
            circle = new DrawableCircle(GraphicsDevice, 50, Color.Red, false);
            circle2 = new DrawableCircle(GraphicsDevice, 50, Color.White, true);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //
            // Only lucas moves.  The rest of the animations were for showcase only. No movement animations
            //
            int lucasDirX = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                lucasDir = 1;
                lucasDirX++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                lucasDir = 2;
                lucasDirX--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isJumping)
            {
                isJumping = true;
                movementSpeedY = 10.0f;
            }

            if (isJumping)
            {
                performJump();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                lucasDir = 0;
            }

            lucasX += 5f * lucasDirX;
            lucasY -= movementSpeedY;

            // Lucas's vector position
            lucasPosition = new Vector2(lucasX, lucasY);

            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // The spritebatch begins HAS to be like this when dealing with noninteger movement otherwise it gets blurry
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            // Showcase animations.
            walkingDown.Draw(spriteBatch, gameTime, 1, 1);
            walkingLeft.Draw(spriteBatch, gameTime, 100, 1);
            walkingRight.Draw(spriteBatch, gameTime, 1, 100);
            walkingUp.Draw(spriteBatch, gameTime, 100, 100);

            // Animations for lucas... Can you follow along?
            if (!isJumping)
            {

                jumpRight.currentFrame = 0;
                jumpLeft.currentFrame = 0;

                if (lucasDir == 0)
                {
                    noMovement.Draw(spriteBatch, gameTime, lucasPosition);
                }
                else if (lucasDir == 1)
                {
                    movementRight.Draw(spriteBatch, gameTime, lucasPosition);
                }
                else if (lucasDir == 2)
                {
                    movementLeft.Draw(spriteBatch, gameTime, lucasPosition);
                }
            }
            else
            {
                if (lucasDir == 0 || lucasDir == 1)
                {
                    jumpRight.Draw(spriteBatch, gameTime, lucasPosition);
                }
                else if (lucasDir == 2)
                {
                    jumpLeft.Draw(spriteBatch, gameTime, lucasPosition);
                }
            }

            rect.Draw(spriteBatch, lucasPosition * 2.5f);
            rect2.Draw(spriteBatch, lucasPosition * 1.5f);
            circle.Draw(spriteBatch, lucasPosition);
            circle2.Draw(spriteBatch, lucasPosition + new Vector2(100, 100));

            // End the spritebatch.
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // lets lucas jump
        private void performJump()
        {
            if (lucasY <= 300)
            {
                movementSpeedY -= gravity;
            }
            else
            {
                lucasY = 300;
                movementSpeedY = 0.0f;
                isJumping = false;
            }
        }
    }
}
