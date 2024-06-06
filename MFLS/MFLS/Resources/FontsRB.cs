using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;

namespace MFLS.Resources
{
     public class FontsRB : ResourceBox<IFont>
     {
          private static ContentManager _content;

          public FontsRB() : base("Fonts")
          {
               _content = new ContentManager(GameMgr.Game.Services);
               _content.RootDirectory = ResourceInfoMgr.ContentDir + "/Fonts";
          }

          public override void Load()
          {
               if (Loaded)
               {
                    return;
               }
               Loaded = true;

               // Add your resources here. 
               // Example:
               // AddResource("Test", _content.Load<SpriteFont>("test"));
               // You can access the resources via ResourceHub.GetResource<>();
               AddResource("Arial", new Font(_content.Load<SpriteFont>("Arial")));

          }

          public override void Unload()
          {
               if (!Loaded)
               {
                    return;
               }
               Loaded = false;
               _content.Unload();
          }

          public static SpriteFont GetFont(string fontName)
          {
               return _content.Load<SpriteFont>(fontName);
          }
     }
}
