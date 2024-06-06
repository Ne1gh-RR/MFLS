using MFLS.Resources;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using System.Numerics;

namespace MFLS
{
     public class Example : Entity
	{
		LocaleTextMgr localeTextMgr;

		public Example(Layer layer) : base(layer)
		{
			var container = LocalesRB.GetLocale("Test")["TestContainer"];

			var textDrawerSettings = new LocaleTextDrawerSettings()
			{
				Position = Vector2.One * 300,
				PerLetter = true,
				Scale = Vector2.One * 5
			};

			localeTextMgr = new LocaleTextMgr(GameController.GUILayer, container, textDrawerSettings);
		}

		public override void Update()
		{
			base.Update();
		}
	}
}
