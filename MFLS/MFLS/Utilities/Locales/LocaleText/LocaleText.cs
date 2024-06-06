using System.Text;

namespace MFLS
{
     public class LocaleText
     {
          public LocaleSymbol[] Symbols { get; set; }

          public string GetTextByIndex(int index)
          {
               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < index && i < Symbols.Length; i++)
               {
                    sb.Append(Symbols[i]);
               }

               return sb.ToString();
          }

          public override string ToString()
          {
               StringBuilder sb = new StringBuilder();

               foreach (var item in Symbols)
               {
                    sb.Append(item.Symbol);
               }

               return sb.ToString();
          }
     }

     
}
