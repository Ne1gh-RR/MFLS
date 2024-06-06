using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using System;
using System.Diagnostics;

namespace MFLS
{
     public class LocaleTextMgr : Entity
     {
          public Func<string, LocaleUnitContainer> GetUnitByKey {  get; set; }

          LocaleUnitContainer unitContainer;
          public LocaleTextDrawer TextDrawer { get; set; }
          public bool Finished { get; private set; }

          Stopwatch stopwatch;

          int currentUnit = 0;

          public bool MomentarySkip { get; set; }
          public int SkipIndex { get; set; } = 0;
          int skipsCount = 0;
          public int SkipRestrictions { get; set; } = 1;

          public LocaleTextMgr(Layer layer, LocaleUnitContainer _unitContainer, LocaleTextDrawerSettings textDrawerSettings) : base(layer)
          {
               unitContainer = _unitContainer;

               TextDrawer = new LocaleTextDrawer(unitContainer.Units[0].GetLocaleText(), textDrawerSettings);

               stopwatch = new Stopwatch();
          }

          public override void Update()
          {
               base.Update();

               if (!stopwatch.IsRunning && !unitContainer.Controllable && TextDrawer.Finished)
               {
                    stopwatch = Stopwatch.StartNew();
               }

               TextDrawer.Update();

               if (TextDrawer.Finished && unitContainer.Controllable && CustomInput.InteractButton() ||
                    !unitContainer.Controllable && stopwatch.ElapsedMilliseconds > unitContainer.Units[currentUnit].WaitMsecs &&
                    TextDrawer.Finished)
               {
                    NextUnit();
               }

               if (CustomInput.SkipButton() && MomentarySkip)
               {
                    TextDrawer.Finish();
               }
               else if (unitContainer.Controllable && CustomInput.SkipButton() && SkipIndex > 0 && skipsCount < SkipRestrictions)
               {
                    TextDrawer.DrawSpeedMultiplier += SkipIndex;

                    skipsCount++;
               }
          }

          public override void Draw()
          {
               base.Draw();

               TextDrawer.DrawTextSymbols();
          }

          public virtual void NextUnit()
          {
               #region Managing game parameters.
               if (unitContainer.Units[currentUnit].GameParameterToSet != null)
               {
                    var tuple = unitContainer.Units[currentUnit].GameParameterToSet.Value;

                    //if (GameState.GameParameters.ContainsKey(tuple.GameParameterKey))
                    //{
                    //     GameState.GameParameters[tuple.GameParameterKey] = tuple.GameParameterValue;
                    //}
                    //else
                    //{
                    //     GameState.GameParameters.Add(tuple.GameParameterKey, tuple.GameParameterValue);
                    //}
               }
               #endregion

               if (!unitContainer.Controllable)
               {
                    stopwatch.Reset();
               }

               skipsCount = 0;

               currentUnit++;

               if (currentUnit == unitContainer.Units.Length)
               {
                    #region Managing containers.
                    if (unitContainer.Units[currentUnit - 1].NextContainerID != null &&
                         GetUnitByKey != null)
                    {
                         unitContainer = GetUnitByKey(unitContainer.Units[currentUnit - 1].NextContainerID);

                         currentUnit = 0;

                         TextDrawer.Reset(unitContainer.Units[currentUnit].GetLocaleText());
                         TextDrawer.PerLetter = true;

                         return;
                    }
                    #endregion;

                    Destroy();
                    Finished = true;

                    return;
               }

               TextDrawer.Reset(unitContainer.Units[currentUnit].GetLocaleText());
          }

          public override void Destroy()
          {
               base.Destroy();

               Enabled = false;
               Visible = false;
               TextDrawer = null;
          }
     }
}
