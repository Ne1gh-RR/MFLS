using System;
using System.Collections.Generic;
using System.Text;

namespace MFLS

{
     public class LocaleTextParser
     {
          List<LocaleSymbol> symbols;

          LocaleSymbol currentSymbol;

          LocaleSymbolFormatter formatter;

          int currentChar = 0;

          public LocaleTextParser(Action<string> setNextContainerID, Action<float> setWaitMsecs, Action<(string, string)> setGameParameter)
          {
               symbols = new List<LocaleSymbol>();

               currentSymbol = new LocaleSymbol();

               formatter = new LocaleSymbolFormatter()
               {
                    SetNextContainerMethodID = setNextContainerID,
                    SetWaitMsecs = setWaitMsecs,
                    SetGameParameter = setGameParameter
               };
          }

          public LocaleText ParseLine(string text)
          {
               Reset();

               for (; currentChar < text.Length; currentChar++)
               {
                    try
                    {
                         while (text[currentChar] == '<')
                         {
                              var formattingCode = GetFormattingCode(text[(currentChar + 1)..]);

                              formatter.FormatSymbol(formattingCode, ref currentSymbol);

                              currentChar += formattingCode.Length + 2;
                         }

                         symbols.Add(
                              new LocaleSymbol()
                              {
                                   Symbol = text[currentChar],
                                   Color = currentSymbol.Color,
                                   ShakingWholeFragment = currentSymbol.ShakingWholeFragment,
                                   Shaking = currentSymbol.Shaking,
                                   Wrap = currentSymbol.Wrap,
                                   DrawSpeed = currentSymbol.DrawSpeed,
                                   IsWaved = currentSymbol.IsWaved,
                                   WaveAmplitude = currentSymbol.WaveAmplitude,
                                   WaveLength = currentSymbol.WaveLength,
                                   WaveOnlyForY = currentSymbol.WaveOnlyForY,
                                   IsRotating = currentSymbol.IsRotating,
                                   RotationIntensity = currentSymbol.RotationIntensity,
                              });

                         currentSymbol.Wrap = false;
                    }
                    catch
                    {
                         currentChar = 0;
                         break;
                    }
               }

               return new LocaleText() { Symbols = symbols.ToArray() };
          }

          public string GetFormattingCode(string text)
          {
               StringBuilder formattingCode = new();

               foreach (var _char in text)
               {
                    if (_char == '>')
                    {
                         break;
                    }

                    formattingCode.Append(_char);
               }

               return formattingCode.ToString();
          }

          void Reset()
          {
               symbols = new List<LocaleSymbol>();

               currentSymbol = new LocaleSymbol();

               currentChar = 0;
          }
     }
}
