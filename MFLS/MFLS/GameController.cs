using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;

namespace MFLS
{
     public class GameController : Entity
     {
          public Camera2D Camera = new Camera2D(800, 600);

          public static string Language = "EN"; //Так похуй.

          public static Layer GUILayer; //Так похуй.

          public GameController() : base(SceneMgr.GetScene("default")["default"])
          {
               GameMgr.MaxGameSpeed = 144;
               GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

               Camera.BackgroundColor = Color.White;

               GameMgr.WindowManager.CanvasSize = Camera.Size;
               GameMgr.WindowManager.Window.AllowUserResizing = false;
               GameMgr.WindowManager.ApplyChanges();
               GameMgr.WindowManager.CenterWindow();
               GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;

               GraphicsMgr.VertexBatch.SamplerState = SamplerState.PointClamp;

               GUILayer = Scene.CreateLayer("gui");
               GUILayer.IsGUI = true;

               var cameraController = new CameraController(GUILayer, Camera);

               var switcher = new SceneSwitcher(GUILayer, cameraController);
               switcher.CurrentFactory.CreateScene();

               Text.CurrentFont = ResourceHub.GetResource<IFont>("Arial");
          }

          public override void Update()
          {
               base.Update();
          }


          public override void Draw()
          {
               base.Draw();
          }

     }
}