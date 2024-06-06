using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Utils;
using System;

namespace MFLS
{
     public class Shaking
     {
          public double ShakingTime { get; set; }
          bool shakingTimeSet;
          public bool IsLerped { get; set; }
          public Vector3 Amount { get; set; }

          public Shaking(Vector3 amount)
          {
               Amount = amount;
          }

          public Vector3 Shake(Vector3 initPosition = default, float speed = 0)
          {
               if (IsLerped)
               {
                    return Vector3.Lerp(Vector3.Zero, initPosition + new Vector3(
                         (Game1.Random.NextSingle() - 0.5f) * 2 * Amount.X,
                         (Game1.Random.NextSingle() - 0.5f) * 2 * Amount.Y,
                         (Game1.Random.NextSingle() - 0.5f) * 2 * Amount.Z),
                         (float)TimeKeeper.Global.Time(speed));
               }

               return new Vector3((Game1.Random.NextSingle() - 0.5f) * 2 * Amount.X,
                    (Game1.Random.NextSingle() - 0.5f) * 2 * Amount.Y,
                    (Game1.Random.NextSingle() - 0.5f) * 2 * Amount.Z);

          }

          public void Reset(Vector3 newAmount, double newShakingTime)
          {
               Amount = newAmount;
               ShakingTime = newShakingTime;
               shakingTimeSet = false;
          }
     }
}
