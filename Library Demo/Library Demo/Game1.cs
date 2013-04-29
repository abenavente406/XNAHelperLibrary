using System;
using Microsoft.Xna.Framework;
using GameHelperLibrary; // <-- Always include the library when dealing with the library
using GameHelperLibrary.States;
using Library_Demo.GameScreens;

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
        ScreenManager screenManager;
        ScreenFactory screenFactory;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
