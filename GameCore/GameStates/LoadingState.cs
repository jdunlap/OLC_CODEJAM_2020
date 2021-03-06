﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PandaMonogame;
using PandaMonogame.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class LoadingState : GameState
    {
        protected PUIMenu _menu = new PUIMenu();
        protected Sprite _menuBG = null;
        protected bool _goToPlay = false;

        public override void Load(ContentManager Content, GraphicsDevice graphics)
        {
            _menu.Load(graphics, "LoadingMenuDefinition", "UITemplates");

            _menuBG = new Sprite(ModManager.Instance.AssetManager.LoadTexture2D(graphics, "MenuBG"));
            _menuBG.Position = Vector2.Zero;
            _menuBG.Center = Vector2.Zero; // otherwise scaling is weird
            _menuBG.Scale = ((float)graphics.PresentationParameters.BackBufferWidth / (float)_menuBG.Texture.Width);
        }

        public override int Update(GameTime gameTime)
        {
            if (_goToPlay)
                return (int)GameStateType.Play;

            _goToPlay = true;
            return (int)GameStateType.None;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _menuBG.Draw(spriteBatch);
            _menu.Draw(spriteBatch);
            spriteBatch.End();
        }
    } // LoadingState
}
