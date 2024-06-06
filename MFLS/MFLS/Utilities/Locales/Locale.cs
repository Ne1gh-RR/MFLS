using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MFLS
{
     public class Locale
     {
          Dictionary<string, LocaleUnitContainer> UnitContainers;

          public Locale(Dictionary<string, LocaleUnitContainer> containers)
          {
               UnitContainers = containers;
          }

          public LocaleUnitContainer this[string key]
          {
               get { return UnitContainers[key]; }
          }
     }
}
