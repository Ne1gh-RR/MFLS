using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;

namespace MFLS
{
     public class LocaleTextDrawerSettings
     { 
          public Vector2 Position { get; set; }
          public Vector2 Scale { get; set; } = Vector2.One;
          public float Rotation { get; set; }
          public Color DefaultColor { get; set; } = Color.Black;
          public float Hue { get; set; } = 0;
          public TextAlign HorizontalAlign { get; set; }
          public TextAlign VerticalAlign { get; set; }
          public bool StartFromHorizontalCenter {  get; set; }
          public bool StartFromVerticalCenter {  get; set; }
          public float XBoundary { get; set; } = float.MaxValue;
          public string Font { get; set; } = "Arial";
          public bool PerLetter { get; set; }
          public BlendState BlendState { get; set; }
          //public SndEffectsMgr Sound { get; set; }
     }
}