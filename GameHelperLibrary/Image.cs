using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameHelperLibrary {
    public class Image {

        private Texture2D image { get; set; }

        public int width {
            get {
                return image.Width;
            }
        }

        public int height {
            get {
                return image.Height;
            }
        }

        /// <summary>
        /// Creates an image that can be easily drawn to the screen
        /// </summary>
        /// <param name="content">Content Manager of game</param>
        /// <param name="assetName">The name of the asset with no extensions</param>
        public Image(ContentManager content, string assetName) {
            image = content.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Creates an image that can easily be drawn to the screen.
        /// </summary>
        /// <param name="image"></param>
        public Image(Texture2D image) {
            this.image = image;
        }

        public void Draw(SpriteBatch spriteBatch, float x, float y) {
            spriteBatch.Draw(image, new Vector2(x, y), Color.White);
        }

    }
}
