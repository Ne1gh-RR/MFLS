using System;
using System.Collections.Generic;
using System.Linq;

namespace MFLS
{
     public class LocaleFileParser
     {
          LocaleUnitParser localeUnitParser;

          Dictionary<string, LocaleUnitContainer> containers;

          List<LocaleUnit> unitsList;

          LocaleUnit currentUnit;

          public LocaleFileParser()
          {
               localeUnitParser = new LocaleUnitParser(new LocaleTextParser(SetNextContainer, SetWaitMsecs, SetGameParameter));

               containers = new Dictionary<string, LocaleUnitContainer>();

               unitsList = new List<LocaleUnit>();

               currentUnit = new IGT();
          }

          public Locale ParseFile(string[] lines)
          {
               Reset();

               for (int i = 0; i < lines.Length; i++)
               {
                    if (string.IsNullOrEmpty(lines[i]) || lines[i][..2] == "//")
                    {
                         continue;
                    }

                    var splittedLine = lines[i].Split(' ');

                    #region Containers parser.
                    if (splittedLine[0] == "#")
                    {
                         if (containers.Count > 0)
                         {
                              containers.LastOrDefault().Value.Units = unitsList.ToArray();

                              unitsList.Clear();
                         }

                         containers.Add(splittedLine[1], new LocaleUnitContainer());

                         try
                         {
                              containers.Last().Value.Controllable = bool.Parse(splittedLine[2].ToLower());
                         }
                         catch { }
                    }
                    #endregion

                    #region Replicas parsing.
                    else if (splittedLine[0] == "!")
                    {
                         currentUnit = new Replica()
                         {
                              Character = splittedLine[1],
                              CharacterEmotion = (CharsEmotions)Enum.Parse(typeof(CharsEmotions), splittedLine[2])
                         };

                         continue;
                    }
                    else if (currentUnit.GetType().IsAssignableTo(typeof(Replica)))
                    {
                         var currReplica = (Replica)currentUnit;

                         unitsList.Add(
                              new Replica()
                              {
                                   Text = localeUnitParser.ParseUnitText(lines[i..], ref i),
                                   NextContainerID = currentUnit.NextContainerID,
                                   WaitMsecs = currentUnit.WaitMsecs,
                                   GameParameterToSet = currReplica.GameParameterToSet,
                                   Character = currReplica.Character,
                                   CharacterEmotion = currReplica.CharacterEmotion
                              });

                         currentUnit.NextContainerID = null;

                         continue;
                    }
                    #endregion

                    #region Comments parsing.
                    else if (splittedLine[0] == "*")
                    {
                         unitsList.Add(new Comment()
                         {
                              Text = localeUnitParser.ParseUnitText(lines[i..], ref i),
                              NextContainerID = currentUnit.NextContainerID,
                              WaitMsecs = currentUnit.WaitMsecs,
                              GameParameterToSet = currentUnit.GameParameterToSet,
                         });

                         currentUnit.NextContainerID = null;

                         continue;
                    }
                    #endregion

                    #region Choices parsing.
                    else if (splittedLine[0] == ">")
                    {
                         var choiceContainer = new ChoiceContainer()
                         {
                              Text = localeUnitParser.ParseUnitText(lines[i..], ref i),
                              WaitMsecs = currentUnit.WaitMsecs
                         };

                         i++;

                         var choices = new List<Choice>();

                         var leftLines = lines[i..];

                         foreach (var line in lines[i..])
                         {
                              var splittedChoiceLine = line.Split(' ');

                              if (splittedChoiceLine[0] != "^" || choices.Count > 4)
                              {
                                   break;
                              }
                              else
                              {
                                   choices.Add(new Choice()
                                   {
                                        Text = localeUnitParser.ParseUnitText(lines[i..], ref i),
                                        NextContainerID = currentUnit.NextContainerID,
                                        WaitMsecs = currentUnit.WaitMsecs,
                                        GameParameterToSet = currentUnit.GameParameterToSet,
                                   });
                              }
                         }

                         choiceContainer.Choices = choices.ToArray();

                         unitsList.Add(choiceContainer);

                         i += choices.Count - 1;
                    }
                    #endregion

                    #region IGT parsing.
                    else
                    {
                         unitsList.Add(
                              new IGT()
                              {
                                   Text = localeUnitParser.ParseUnitText(lines[i..], ref i),
                                   NextContainerID = currentUnit.NextContainerID,
                                   WaitMsecs = currentUnit.WaitMsecs,
                                   GameParameterToSet = currentUnit.GameParameterToSet
                              });
                    }
                    #endregion

                    if (currentUnit != null)
                    {
                         currentUnit.NextContainerID = null;
                    }
               }

               containers.LastOrDefault().Value.Units = unitsList.ToArray();

               return new Locale(containers);
          }

          void Reset()
          {
               localeUnitParser = new LocaleUnitParser(new LocaleTextParser(SetNextContainer, SetWaitMsecs, SetGameParameter));

               containers = new Dictionary<string, LocaleUnitContainer>();

               unitsList = new List<LocaleUnit>();

               currentUnit = new IGT() { NextContainerID = null };
          }

          public void SetNextContainer(string containerID)
          {
               currentUnit.NextContainerID = containerID;
          }

          public void SetWaitMsecs(float msecs)
          {
               currentUnit.WaitMsecs = msecs;
          }

          public void SetGameParameter((string GameParameterKey, string GameParameterValue) parameterTuple)
          {
               currentUnit.GameParameterToSet = parameterTuple;
          }
     }
}

#region legacy shit
//string lastId = "";
//bool lastControllableProperty = false;

//for (int i = 0; i < lines.Length; i++)
//{
//     if (string.IsNullOrEmpty(lines[i]))
//     {
//          continue;
//     }

//     var splittedLine = lines[i].Split(' ');

//     if (splittedLine[0] == "#")
//     {
//          lastId = splittedLine[1];
//          lastControllableProperty = bool.Parse(splittedLine[2]);

//          containers.Add(lastId, new LocaleUnitContainer()
//          {
//               Units = unitsList.ToArray(),
//               Controllable = lastControllableProperty
//          });

//          continue;
//     }
//     else if (splittedLine[0] == "!")
//     {
//          currentUnit = new Replica()
//          {
//               Character = splittedLine[1],
//               CharacterEmotion = (CharsEmotions)Enum.Parse(typeof(CharsEmotions), splittedLine[2])
//          };

//          continue;
//     }
//     else if (splittedLine[0] == "*")
//     {
//          currentUnit = new Comment()
//          {
//               Text = localeTextParser.ParseLine(string.Join("", splittedLine[1..])),
//          };

//          continue;
//     }
//     else if (splittedLine[0] == ">")
//     {
//          List<Choice> choices = new List<Choice>();

//          foreach (var str in lines[(i + 1)..])
//          {
//               var splittedStr = str.Split(' ');

//               if (splittedStr[0] == "^")
//               {
//                    choices.Add(new Choice() { Text = localeTextParser.ParseLine(string.Join("", splittedStr[1..])) });
//               }
//               else
//               {
//                    break;
//               }

//               if (choices.Count > 4)
//               {
//                    throw new Exception("Only 4 choices units can be in the container at the same time.");
//               }
//          }

//          unitsList.Add(new ChoiceContainer()
//          {
//               Choices = choices.ToArray(),
//               Text = localeTextParser.ParseLine(splittedLine[1])
//          });

//          i += choices.Count + 1;

//          continue;
//     }

//     if (currentUnit != null)
//     {
//          switch (currentUnit.GetType().Name)
//          {
//               case "IGT":
//                    {
//                         unitsList.Add(new IGT
//                         {
//                              Text = localeTextParser.ParseLine(lines[i])
//                         });
//                         break;
//                    }
//               case "Replica":
//                    {
//                         unitsList.Add(new Replica
//                         {
//                              Text = localeTextParser.ParseLine(lines[i]),
//                              Character = ((Replica)currentUnit).Character,
//                              CharacterEmotion = ((Replica)currentUnit).CharacterEmotion,
//                         });
//                         break;
//                    }
//               case "Comment":
//                    {
//                         unitsList.Add(new Comment
//                         {
//                              Text = localeTextParser.ParseLine(lines[i])
//                         });
//                         break;
//                    }
//          }

//          if (currentUnit.GetType().Name == "IGT")
//          {
//               unitsList.Add(new IGT
//               {
//                    Text = localeTextParser.ParseLine(lines[i])
//               });
//          }
//          else
//          {


//               currentUnit.Text = localeTextParser.ParseLine(lines[i]);
//          }

//          unitsList.Add(currentUnit);
//     }
//}

//if (containers.Count > 1)
//{
//     containers[lastId].Units = unitsList.ToArray();
//     containers[lastId].Controllable = lastControllableProperty;
//}

//return new Locale()
//{
//     UnitContainers = containers
//};
#endregion