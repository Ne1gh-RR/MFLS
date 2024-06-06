using MFLS.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;
using Monofoxe.Resources;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;
using System;

namespace MFLS
{
     public class Game1 : Game
     {
          public static Random Random;

          public Game1()
          {
               Content.RootDirectory = ResourceInfoMgr.ContentDir;
               GameMgr.Init(this);

               Random = new Random();
          }

          protected override void Initialize()
          {
               base.Initialize();
               TiledEntityFactoryPool.InitFactoryPool();

               new GameController();
          }

          protected override void LoadContent()
          {
               GraphicsMgr.Init(GraphicsDevice);

               new SpriteGroupResourceBox("DefaultSprites", "Graphics/Default");
               new DirectoryResourceBox<Effect>("Effects", "Effects");
               new DirectoryResourceBox<TiledMap>("Maps", "Maps");

               new LocalesRB();

               new FontsRB();
          }

          protected override void UnloadContent()
          {
               ResourceHub.UnloadAll();
          }

          protected override void Update(GameTime gameTime)
          {
               GameMgr.Update(gameTime);

               base.Update(gameTime);
          }

          protected override void Draw(GameTime gameTime)
          {
               GameMgr.Draw(gameTime);

               base.Draw(gameTime);
          }
     }
}
