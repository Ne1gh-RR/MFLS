using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monofoxe.Engine;
using System;
using Buttons = Monofoxe.Engine.Buttons;

namespace MFLS
{
     public static class CustomInput
     {
          public static bool InteractButton(bool checkIfPressed = true)
          {
               if (checkIfPressed)
               {
                    if (Input.CheckButtonPress(Buttons.Z) ||
                         Input.CheckButtonPress(Buttons.Enter) ||
                         Input.CheckButtonPress(Buttons.GamepadA)
                         )
                    {
                         return true;
                    }

                    return false;
               }
               else
               {
                    if (Input.CheckButton(Buttons.Z) ||
                         Input.CheckButton(Buttons.Enter) ||
                         Input.CheckButton(Buttons.GamepadA)
                         )
                    {
                         return true;
                    }

                    return false;
               }
          }

          public static bool SkipButton(bool checkIfPressed = true)
          {
               if (checkIfPressed)
               {
                    if (Input.CheckButtonPress(Buttons.X) ||
                         Input.CheckButtonPress(Buttons.LeftShift) ||
                         Input.CheckButtonPress(Buttons.RightShift) ||
                         Input.CheckButtonPress(Buttons.GamepadB)
                         )
                    {
                         return true;
                    }
                    return false;
               }
               else
               {
                    if (Input.CheckButton(Buttons.X) ||
                         Input.CheckButton(Buttons.LeftShift) ||
                         Input.CheckButton(Buttons.RightShift) ||
                         Input.CheckButton(Buttons.GamepadB)
                         )
                    {
                         return true;
                    }
                    return false;
               }
          }

          public static bool InventoryButton(bool checkIfPressed = true)
          {
               if (checkIfPressed)
               {
                    if (Input.CheckButtonPress(Buttons.C) ||
                         Input.CheckButtonPress(Buttons.LeftControl) ||
                         Input.CheckButtonPress(Buttons.RightControl) ||
                         Input.CheckButtonPress(Buttons.GamepadX)
                         )
                    {
                         return true;
                    }
                    return false;
               }
               else
               {
                    if (Input.CheckButton(Buttons.C) ||
                         Input.CheckButton(Buttons.LeftControl) ||
                         Input.CheckButton(Buttons.RightControl) ||
                         Input.CheckButton(Buttons.GamepadX)
                         )
                    {
                         return true;
                    }
                    return false;
               }
          }

          public static Vector2 GetMoveDirection()
          {
               var gamepadInput = new Vector2(Input.GamepadGetLeftStick(0).X, -Input.GamepadGetLeftStick(0).Y);

               var keyboardInput = new Vector2(
                    Input.CheckButton(Buttons.Right).ToInt() - Input.CheckButton(Buttons.Left).ToInt(),
                    Input.CheckButton(Buttons.Down).ToInt() - Input.CheckButton(Buttons.Up).ToInt()
               );

               if (gamepadInput != Vector2.Zero)
               {
                    return gamepadInput;
               }
               else
               {
                    return keyboardInput;
               }
          }

          public static Vector2 GetSelectionPressingDirection()
          {
               var gamepadInput = new Vector2(Input.GamepadGetRightStick(0).X, -Input.GamepadGetRightStick(0).Y);

               var keyboardInput = new Vector2(
                    -Input.CheckButtonPress(Buttons.Left).ToInt() + Input.CheckButtonPress(Buttons.Right).ToInt(),
                    -Input.CheckButtonPress(Buttons.Up).ToInt() + Input.CheckButtonPress(Buttons.Down).ToInt()
                    );

               if (gamepadInput != Vector2.Zero)
               {
                    gamepadInput = new Vector2((float)Math.Round(gamepadInput.X), (float)Math.Round(gamepadInput.Y));
                    return gamepadInput;
               }
               else
               {
                    keyboardInput = new Vector2((float)Math.Round(keyboardInput.X), (float)Math.Round(keyboardInput.Y));
                    return keyboardInput;
               }
          }
     }
}
