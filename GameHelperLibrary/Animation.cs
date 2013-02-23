using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameHelperLibrary {
    public class Animation {

        // The images used in the animation
        public Texture2D[] images;

        private float timer { get; set; }
        public float interval { get; set; }

        // Number of frames in the animation
        private int frames = 0;
        // The current frame to render
        public int currentFrame { get; set; }

        // The position to draw the animation on the screen
        private Vector2 position;

        /// <summary>
        /// Creates an animation based on an array of images
        /// </summary>
        /// <param name="images">The array of images the animation is made from</param>
        /// <param name="interval">The amount of time between each frame (default: 100f)</param>
        public Animation(Texture2D[] images, float interval) {
            this.interval = interval;
            frames = images.Length;

            this.images = new Texture2D[frames];

            for (int i = 0; i < frames; i++) {
                this.images[i] = images[i];
            }

        }

        /// <summary>
        /// Draws the animation to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="x">X location to draw the animation</param>
        /// <param name="y">Y location to draw the animation</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float x, float y) {

            position = new Vector2(x, y);

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            //Check the timer is more than the chosen interval
            if (timer > interval) {
                 //Show the next frame
                    currentFrame++;
                 //Reset the timer
                    timer = 0f;
            }

            // If we are on the last frame, reset back to the one before the first frame
            if (currentFrame == frames) {
                  currentFrame = 0;
            }

            spriteBatch.Draw(images[currentFrame], position, Color.White);
        }

        /// <summary>
        /// Draws the animation to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="position">The position to draw the animation</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position) {
            Draw(spriteBatch, gameTime, position.X, position.Y);
        }

    }
}
