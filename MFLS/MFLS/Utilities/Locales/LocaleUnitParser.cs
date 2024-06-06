using System.Collections.Generic;

namespace MFLS
{
     public class LocaleUnitParser
     {
          LocaleTextParser localeTextParser;

          public LocaleUnitParser(LocaleTextParser textParser)
          {
               localeTextParser = textParser;
          }

          public LocaleText[] ParseUnitText(string[] leftLines, ref int i)
          {
               List<LocaleText> text = new List<LocaleText>();

               if (leftLines[0][0] != ':')
               {
                    return new LocaleText[] { localeTextParser.ParseLine(leftLines[0]) };
               }
               else
               {
                    foreach (string line in leftLines)
                    {
                         if (string.IsNullOrEmpty(line))
                         {
                              break;
                         }

                         if (line[0] == ':')
                         {
                              text.Add(localeTextParser.ParseLine(line[2..]));

                              i++; 
                         }
                         else
                         {
                              break;
                         }
                    }

                    if (text.Count > 0)
                    {
                         i--;
                    }

                    return text.ToArray();
               }
          }
     }
}