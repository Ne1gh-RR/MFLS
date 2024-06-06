namespace MFLS
{
     public abstract class LocaleUnit
     {
          internal LocaleText[] Text { get; set; }
          public string NextContainerID { get; internal set; }
          public float WaitMsecs { get; internal set; } = 500;
          public (string GameParameterKey, string GameParameterValue)? GameParameterToSet { get; internal set; }

          public LocaleText GetLocaleText()
          {
               return Text[Game1.Random.Next(0, Text.Length)];
          }
     }
}