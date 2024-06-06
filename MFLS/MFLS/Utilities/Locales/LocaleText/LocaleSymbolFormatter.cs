using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using System;

namespace MFLS
{
     public class LocaleSymbolFormatter
     {
          public Action<string> SetNextContainerMethodID;
          public Action<float> SetWaitMsecs;
          public Action<(string GameParameterKey, string GameParameterValue)> SetGameParameter;

          public void FormatSymbol(string formattingCode, ref LocaleSymbol symbol)
          {
               if (formattingCode == "/")
               {
                    symbol = new LocaleSymbol()
                    {
                         Wrap = symbol.Wrap
                    };

                    return;
               }

               #region Managing wraping and milliseconds waiting.
               if (formattingCode == "#")
               {
                    symbol.Wrap = true;
               }
               else if (formattingCode.Contains('%'))
               {
                    if (formattingCode[0] == '/')
                    {
                         symbol.DrawSpeed = 1;
                    }
                    else
                    {
                         symbol.DrawSpeed = float.Parse(formattingCode[1..]);
                    }
               }
               #endregion

               #region Managing shaking.
               else if (formattingCode.Contains('*'))
               {
                    if (formattingCode[0] == '/')
                    {
                         symbol.Shaking = null;
                    }
                    else
                    {
                         var perLetter = formattingCode.Contains('@');
                         var parsedShakingData = formattingCode[(1 + perLetter.ToInt())..].Split(", ");
                         var shaking = new Shaking((Vector2.One * float.Parse(parsedShakingData[0])).ToVector3());

                         shaking.IsLerped = bool.Parse(parsedShakingData[1]);

                         symbol.Shaking = shaking;
                         symbol.ShakingWholeFragment = !perLetter;
                    }
               }
               #endregion

               #region Managing sinusoidal moving.
               else if (formattingCode.Contains('~'))
               {
                    if (formattingCode[0] == '/')
                    {
                         symbol.IsWaved = false;
                    }
                    else
                    {
                         var parsedData = formattingCode[1..].Split(", ");

                         symbol.IsWaved = true;
                         symbol.WaveOnlyForY = bool.Parse(parsedData[0]);
                         symbol.WaveAmplitude = float.Parse(parsedData[1]);
                         symbol.WaveLength = float.Parse(parsedData[2]);

                         try
                         {
                              symbol.IsRotating = bool.Parse(parsedData[3]);
                              symbol.RotationIntensity = float.Parse(parsedData[4]);
                         }
                         catch
                         {
                              symbol.IsRotating = false;
                              symbol.RotationIntensity = 0;
                         }
                    }
               }
               #endregion

               #region Manging color.
               else if (formattingCode.Contains('&'))
               {
                    if (formattingCode[0] == '/')
                    {
                         symbol.Color = null;
                    }
                    else
                    {
                         if (formattingCode.Contains("New"))
                         {
                              string colorValuesInString = formattingCode[5..].Replace(')', '\0');

                              string[] colorValues = colorValuesInString.Split(", ");

                              symbol.Color = new Color(
                                  Convert.ToByte(colorValues[0]),
                                  Convert.ToByte(colorValues[1]),
                                  Convert.ToByte(colorValues[2])) * Convert.ToSingle(colorValues[3]);
                         }
                         else
                         {
                              symbol.Color = ColorExtensions.NameToColor(formattingCode[1..]);
                         }
                    }
               }
               #endregion

               #region Managing units' properties.
               else if (formattingCode[0] == 'N')
               {
                    SetNextContainerMethodID(formattingCode[2..][..^1]);
               }
               else if (formattingCode.Split('(')[0] == "SP")
               {
                    var splittedTuple = formattingCode[3..][..^1].Split(", ");

                    SetGameParameter((splittedTuple[0], splittedTuple[1]));
               }
               else if (formattingCode.Split('(')[0] == "MA")
               {
                    SetWaitMsecs(float.Parse(formattingCode[3..][..^1]));
               }
               #endregion
          }
     }
}