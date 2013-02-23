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

namespace GameHelperLibrary {
    public class SpriteSheet {

        private Texture2D sourceImage { get; set; }

        private int spriteWidth { get; set; }
        private int spriteHeight { get; set; }

        private int imageWidth { get; set; }
        private int imageHeight { get; set; }

        private GraphicsDevice graphics { get; set; }

        /// <summary>
        /// Create a sprite sheet from a Texture2D
        /// </summary>
        /// <param name="sourceImage">The sprite sheet image</param>
        /// <param name="spriteWidth">The width of each sprite</param>
        /// <param name="spriteHeight">The height of each sprite</param>
        /// <param name="graphics">The graphics device used by the game</param>
        public SpriteSheet(Texture2D sourceImage, int spriteWidth, int spriteHeight, GraphicsDevice graphics) {
            this.sourceImage = sourceImage;
            this.imageWidth = sourceImage.Width;
            this.imageHeight = sourceImage.Height;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.graphics = graphics;
        }

        /// <summary>
        /// Gets a sub image from the sprite sheet
        /// </summary>
        /// <param name="x">The x tile of the image</param>
        /// <param name="y">The y tile of the image</param>
        /// <returns>A texture2D that is the sub image</returns>
        public Texture2D getSubImage(int x, int y) {
            Rectangle sourceRect = new Rectangle(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight);

            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            sourceImage.GetData<Color>(0, sourceRect, data, 0, data.Length);

            var newTexture = new Texture2D(graphics, sourceRect.Width, sourceRect.Height);
            newTexture.SetData<Color>(data);

            return newTexture;
        }
    }
}
