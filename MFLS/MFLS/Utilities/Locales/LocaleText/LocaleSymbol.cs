using Microsoft.Xna.Framework;

namespace MFLS
{
     public class LocaleSymbol
     {
          public char Symbol { get; set; } = ' ';
          public Color? Color { get; set; } = null;
          public Shaking Shaking { get; set; }
          public bool ShakingWholeFragment { get; set; }
          public bool IsWaved { get; set; } = false;
          public bool WaveOnlyForY { get; set; }
          public float WaveAmplitude { get; set; }
          public float WaveLength { get; set; }
          public bool IsRotating { get; set; }
          public float RotationIntensity { get; set; }
          public bool Wrap { get; set; }
          public float DrawSpeed { get; set; } = 1;
     }
}
