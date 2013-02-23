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
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        #region Shape definitions
        DrawableRectangle rectangleFilled;
        DrawableRectangle rectangleFramed;
        DrawableCircle circleFilled;
        DrawableCircle circleFramed;
        #endregion

        #region SpriteSheet definitions
        SpriteSheet lucasSpriteSheetLeft;    // Holds the images of lucas facing to the left
        SpriteSheet lucasSpriteSheetRight;   // Holds the images of lucas facing to the right
        #endregion

        #region Animation definitions
        Animation lucasAnimationRight;
        Animation lucasAnimationLeft;
        Animation lucasAnimationNoMovement;
        #endregion
        
        #region Lucas definitions
        Vector2 lucasPosition;
        int    lucasDirection;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /**
             * Here's how you initialize the shapes...
             * DrawableRectangle(GraphicsDevice, A Vector2 representing size, Color of the shape, bool isFilled)
             * DrawableCircle(GraphicsDevice, int radius, Color of the shape, bool isFilled)
             * */
            rectangleFilled = new DrawableRectangle(GraphicsDevice, new Vector2(160, 80), Color.White, true);
            rectangleFramed = new DrawableRectangle(GraphicsDevice, new Vector2(160, 80), Color.White, false);
            circleFilled    = new DrawableCircle(GraphicsDevice, 80, Color.White, true);
            circleFramed    = new DrawableCircle(GraphicsDevice, 80, Color.White, false);

            Texture2D spriteSheetLeft  = Content.Load<Texture2D>("lucasleft");
            Texture2D spriteSheetRight = Content.Load<Texture2D>("lucasright");

            // I define the size of the sprites
            int spriteWidth  = 40;
            int spriteHeight = 47;

            /**
             * Here's how you initialize a sprite sheet . . .
             * SpriteSheet(Texture2D of the image file for sprites, width of sprites, height of sprites, GraphicsDevice);
             * */
            lucasSpriteSheetLeft  = new SpriteSheet(spriteSheetLeft, spriteWidth, spriteHeight, GraphicsDevice);
            lucasSpriteSheetRight = new SpriteSheet(spriteSheetRight, spriteWidth, spriteHeight, GraphicsDevice);

            // Okay so up there ^^ I initialized the spritesheets.  With that, I can make the animations

            // BUT WAIT!  I need an array of pictures that will hold all the frames of the animation... 
            // Let's do that now

            Texture2D[] framesLeft  = new Texture2D[] {  lucasSpriteSheetLeft.GetSubImage(8, 0),  lucasSpriteSheetLeft.GetSubImage(7, 0),
                lucasSpriteSheetLeft.GetSubImage(6, 0), lucasSpriteSheetLeft.GetSubImage(5, 0),   lucasSpriteSheetLeft.GetSubImage(4, 0),
                lucasSpriteSheetLeft.GetSubImage(3, 0), lucasSpriteSheetLeft.GetSubImage(2, 0),   lucasSpriteSheetLeft.GetSubImage(1, 0) };   
            // I could use a loop but this is for readability
            Texture2D[] framesRight = new Texture2D[] {  lucasSpriteSheetRight.GetSubImage(0, 1), lucasSpriteSheetRight.GetSubImage(1, 1), 
                lucasSpriteSheetRight.GetSubImage(2, 1), lucasSpriteSheetRight.GetSubImage(3, 1), lucasSpriteSheetRight.GetSubImage(4, 1),
                lucasSpriteSheetRight.GetSubImage(5, 1), lucasSpriteSheetRight.GetSubImage(6, 1), lucasSpriteSheetRight.GetSubImage(7, 1) };
            Texture2D[] framesNoMove = new Texture2D[] { lucasSpriteSheetRight.GetSubImage(0, 0), lucasSpriteSheetRight.GetSubImage(1, 0),
                lucasSpriteSheetRight.GetSubImage(2, 0), lucasSpriteSheetRight.GetSubImage(3, 0) };

            //Now that the array's are loaded because I used the "GetSubImage" function from the library, I can make animations
            lucasAnimationLeft       = new Animation(framesLeft,   100f);
            lucasAnimationRight      = new Animation(framesRight,  100f);
            lucasAnimationNoMovement = new Animation(framesNoMove, 100f);
            // Done loading animations!
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            lucasDirection = 0;

            if (keyState.IsKeyDown(Keys.Right))
                lucasDirection++;
            else if (keyState.IsKeyDown(Keys.Left))
                lucasDirection--;

            if (keyState.IsKeyUp(Keys.Right) && keyState.IsKeyUp(Keys.Left))
                lucasDirection = 0;

            lucasPosition.X += 5f * lucasDirection;

            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Starting the spritebatch like this is necessary because "SamplerState.PointClamp" prevents it from blurring
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            {
                /**
                 * You can draw shapes now with the classes "DrawableRectangle" and 
                 * "DrawableCircle".
                 * */
                rectangleFilled.Draw(spriteBatch, new Vector2(10, 200));
                rectangleFramed.Draw(spriteBatch, new Vector2(rectangleFilled.Position.X + rectangleFilled.Width + 10,
                    rectangleFilled.Position.Y));
                circleFramed.Draw(spriteBatch, new Vector2(rectangleFilled.Position.X,
                    rectangleFilled.Position.Y + rectangleFilled.Height + 10));
                circleFilled.Draw(spriteBatch, new Vector2(circleFramed.Position.X + (2 * circleFramed.Radius) + 10,
                    circleFramed.Position.Y));

                /**
                 * You can draw animations directly to the screen with no complications!
                 * */
                if (lucasDirection == 0) // lucas isn't moving
                    lucasAnimationNoMovement.Draw(spriteBatch, gameTime, lucasPosition);
                else if (lucasDirection == 1) // lucas is moving to the right
                    lucasAnimationRight.Draw(spriteBatch, gameTime, lucasPosition);
                else if (lucasDirection == -1) // lucas is moving to the left
                    lucasAnimationLeft.Draw(spriteBatch, gameTime, lucasPosition);

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
