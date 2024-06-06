using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Resources;
using System.IO;

namespace MFLS.Resources
{
     public class LocalesRB : ResourceBox<Locale>
     {
          static ContentManager content;

          public LocalesRB() : base("Locales")
          {
               content = new ContentManager(GameMgr.Game.Services);
               content.RootDirectory = ResourceInfoMgr.ContentDir + "/Locales";
          }

          public override void Load()
          {
               if (Loaded)
               {
                    return;
               }
               Loaded = true;

               var filesPaths = Directory.GetFiles(@$"{ResourceInfoMgr.ContentDir}\Locales", "*.*", SearchOption.AllDirectories);

               var localeParser = new LocaleFileParser();

               foreach (var file in filesPaths)
               {
                    var localeName = file[(ResourceInfoMgr.ContentDir.Length + 9)..][..^4];

                    AddResource(localeName, localeParser.ParseFile(File.ReadAllLines(file)));
               }
          }

          public override void Unload()
          {
               Loaded = false;
               content.Unload();
          }

          public static Locale GetLocale(string locale)
          {
               locale.Replace("/", "\\");

               return ResourceHub.GetResource<Locale>(@$"{GameController.Language}\{locale}");
          }
     }
}
