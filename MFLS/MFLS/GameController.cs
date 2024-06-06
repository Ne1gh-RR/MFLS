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

          public static string Language = "EN";

          Layer _guiLayer;

          public GameController() : base(SceneMgr.GetScene("default")["default"])
          {


               GameMgr.MaxGameSpeed = 60;
               GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

               Camera.BackgroundColor = Color.White;

               GameMgr.WindowManager.CanvasSize = Camera.Size;
               GameMgr.WindowManager.Window.AllowUserResizing = false;
               GameMgr.WindowManager.ApplyChanges();
               GameMgr.WindowManager.CenterWindow();
               GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;

               GraphicsMgr.VertexBatch.SamplerState = SamplerState.PointClamp;

               _guiLayer = Scene.CreateLayer("gui");
               _guiLayer.IsGUI = true;

               var cameraController = new CameraController(_guiLayer, Camera);

               var switcher = new SceneSwitcher(_guiLayer, cameraController);
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