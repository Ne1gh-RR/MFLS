using MFLS.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using System;

namespace MFLS
{
     public class LocaleTextDrawer
     {
          public bool Finished { get; private set; }
          public double FinishingTime { get; private set; }
          double createdAt;

          private readonly Vector2 initPosition;
          public Vector2 Position { get; set; }
          public Vector2 Scale { get; set; } = Vector2.One;
          public float Rotation { get; set; }
          public Color DefaultColor { get; set; } = Color.Black;
          public float Hue { get; set; } = 0;
          public TextAlign HorizontalAlign { get; set; }
          public TextAlign VerticalAlign { get; set; }
          public bool StartFromHorizontalCenter {  get; set; }
          public bool StartFromVerticalCenter {  get; set; }
          public float XBoundary { get; set; } = float.MaxValue;
          public int DrawSpeedMultiplier { get; set; } = 1;
          public string Font { get; set; } = "Arial";
          public bool PerLetter { get; set; }
          public BlendState BlendState { get; set; }

          LocaleText localeText;
          int currentSymbol = 0;

          //public SndEffectsMgr Sound { get; set; }

          double spacing = 0;

          SpriteBatch spriteBatch;

          public LocaleTextDrawer(LocaleText _localeText, LocaleTextDrawerSettings localeTextDrawerSettings)
          {
               initPosition = localeTextDrawerSettings.Position;

               Copy(localeTextDrawerSettings);

               localeText = _localeText;

               spriteBatch = new SpriteBatch(GraphicsMgr.Device);

               createdAt = GameMgr.ElapsedTimeTotal;
          }

          public void Update()
          {
               if (Finished)
               {
                    return;
               }

               if (currentSymbol > localeText.Symbols.Length - 2 || !PerLetter)
               {
                    Finish();
               }
               else
               {
                    spacing += TimeKeeper.Global.Time(localeText.Symbols[currentSymbol].DrawSpeed * 25 * DrawSpeedMultiplier);

                    if (spacing > localeText.Symbols[currentSymbol].DrawSpeed)
                    {
                         NextSymbol();

                         spacing = 0;
                    }
               }
          }

          public void DrawTextSymbols()
          {
               GraphicsMgr.VertexBatch.FlushBatch();

               spriteBatch.Begin(
                     SpriteSortMode.Deferred,
                     BlendState,
                     GraphicsMgr.VertexBatch.SamplerState,
                     GraphicsMgr.VertexBatch.DepthStencilState,
                     GraphicsMgr.VertexBatch.RasterizerState,
                     GraphicsMgr.VertexBatch.Effect,
                     GraphicsMgr.VertexBatch.View);

               Vector2 symbolOffset = default;
               Vector2 shakingOffset = default;
               Vector2 sinusoidalOffset = default;

               bool shakedFragment = false;

               float textH = FontsRB.GetFont(Font).MeasureString(localeText.ToString()).Y * Scale.Y;

               var align = new Vector2((float)HorizontalAlign, (float)VerticalAlign) / 2f;

               Vector2 lineSize = FontsRB.GetFont(Font).MeasureString(localeText.ToString()) * Scale;
               Vector2 lineOffset = new Vector2(lineSize.X * align.X, textH * align.Y);

               #region Drawing every symbol in locale text.
               for (int i = 0; i <= currentSymbol; i++)
               {
                    #region Managing align for symbol.
                    if (HorizontalAlign == TextAlign.Center)
                    {
                         
                    }
                    if (VerticalAlign == TextAlign.Center)
                    {

                    }
                    #endregion

                    #region Shaking.
                    if (localeText.Symbols[i].Shaking != null)
                    {
                         if (localeText.Symbols[i].ShakingWholeFragment && !shakedFragment)
                         {
                              shakingOffset = localeText.Symbols[i].Shaking.Shake(initPosition.ToVector3(), 1).ToVector2();
                              shakedFragment = true;
                         }
                         else if (!localeText.Symbols[i].ShakingWholeFragment && !shakedFragment)
                         {
                              shakingOffset = default;
                              shakingOffset = localeText.Symbols[i].Shaking.Shake(initPosition.ToVector3(), 1).ToVector2();
                         }
                    }
                    else
                    {
                         shakingOffset = default;
                    }
                    #endregion

                    #region Sinusiodal moving.

                    Rotation = 0;

                    sinusoidalOffset = default;

                    if (localeText.Symbols[i].IsWaved)
                    {
                         sinusoidalOffset += (Vector2.UnitY + (!localeText.Symbols[i].WaveOnlyForY).ToInt() * Vector2.UnitX / 3) * MathF.Sin((float)GameMgr.ElapsedTimeTotal * localeText.Symbols[i].WaveLength + i / (float)Math.PI) * localeText.Symbols[i].WaveAmplitude;

                         if (localeText.Symbols[i].IsRotating)
                         {
                              Rotation = new Angle() { Radians = sinusoidalOffset.Y / 20 * localeText.Symbols[i].RotationIntensity }.DegreesF;
                         }
                    }

                    #endregion

                    #region Wrapping.
                    var nextWordLength = 0f;

                    if (localeText.Symbols[i].Symbol == ' ' && localeText.Symbols.Length > i + 1)
                    {
                         for (int j = i + 1; localeText.Symbols[j].Symbol != ' ' && localeText.Symbols.Length > j + 1; j++)
                         {
                              nextWordLength += FontsRB.GetFont(Font).MeasureString(localeText.Symbols[j].Symbol.ToString()).X *
                                   Scale.X;
                         }
                    }

                    if (Position.X + symbolOffset.X + nextWordLength >= XBoundary || localeText.Symbols[i].Wrap)
                    {
                         symbolOffset.X = 0;
                         symbolOffset.Y += textH;

                         if (localeText.Symbols[i].Symbol == ' ')
                         {
                              continue;
                         }
                    }
                    #endregion

                    #region Color Management.
                    Color symbolColor = DefaultColor;

                    if (localeText.Symbols[i].Color.HasValue)
                    {
                         symbolColor = localeText.Symbols[i].Color.Value;
                    }
                    #endregion

                    spriteBatch.DrawString(
                         FontsRB.GetFont(Font),
                         localeText.Symbols[i].Symbol.ToString(),
                         Position - lineOffset + symbolOffset + shakingOffset + sinusoidalOffset,
                         symbolColor,
                         new Angle() { Degrees = Rotation }.RadiansF,
                         Vector2.One * 5,
                         Scale,
                         SpriteEffects.None,
                         0);

                    symbolOffset.X += FontsRB.GetFont(Font).MeasureString(localeText.Symbols[i].Symbol.ToString()).X * Scale.X;
               }
               #endregion

               Position = initPosition;

               spriteBatch.End();
          }

          void NextSymbol()
          {
               currentSymbol += 1;

               PlayOrNotSound();

               spacing = 0;
          }

          void PlayOrNotSound()
          {
               //if (Sound != null && localeText.Symbols[currentSymbol].Symbol != ' '
               //               && localeText.Symbols[currentSymbol].Symbol != ','
               //               && localeText.Symbols[currentSymbol].Symbol != '.'
               //               && localeText.Symbols[currentSymbol].Symbol != '!'
               //               && localeText.Symbols[currentSymbol].Symbol != '?'
               //               && localeText.Symbols[currentSymbol].Symbol != '"'
               //               && localeText.Symbols[currentSymbol].Symbol != ':'
               //               && localeText.Symbols[currentSymbol].Symbol != ';')
               //{
               //     Sound.Play(false);
               //}
          }

          public void Finish()
          {
               currentSymbol = localeText.Symbols.Length - 1;

               DrawSpeedMultiplier = 1;
               Finished = true;
               FinishingTime = GameMgr.ElapsedTimeTotal - createdAt;
          }

          public void Reset(LocaleText localeText, string _sound = null)
          {
               this.localeText = localeText;

               if (!string.IsNullOrEmpty(_sound))
               {
                    //Sound = new SndEffectsMgr(_sound);
               }

               currentSymbol = 0;
               DrawSpeedMultiplier = 1;

               Finished = false;

               Position = initPosition;
          }

          private void Copy(LocaleTextDrawerSettings settings)
          {
               Position = settings.Position;
               Scale = settings.Scale;
               Rotation = settings.Rotation;
               DefaultColor = settings.DefaultColor;
               Hue = settings.Hue;
               HorizontalAlign = settings.HorizontalAlign;
               VerticalAlign = settings.VerticalAlign;
               StartFromHorizontalCenter = settings.StartFromHorizontalCenter;
               StartFromVerticalCenter = settings.StartFromVerticalCenter;
               XBoundary = settings.XBoundary;
               Font = settings.Font;
               PerLetter = settings.PerLetter;
               BlendState = settings.BlendState;
               //Sound = settings.Sound;
          }
     }
}