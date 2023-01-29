using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.Effects;
using Fusee.Math.Core;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut11_AssetsPicking", Description = "Yet another FUSEE App.")]
    public class Tut11_AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _camTransform;
        private Transform _rightRearTransform;
        private Transform _currentPickTransform;
        private float4 _oldColor;
        private RayCastResult _currentPick;
        private SceneRayCaster _sceneRayCaster;

        // Init is called on startup. 
        public override void Init()
        {
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);
        }

        SceneContainer CreateScene()
        {
            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNode>
                {
                    new SceneNode
                    {
                        Components = new List<SceneComponent>
                        {
                            // TRANSFROM COMPONENT
                            new Transform(),

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),

                            // MESH COMPONENT
                            new CuboidMesh(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }


        public override async Task InitAsync()
        {
            _scene = AssetStorage.Get<SceneContainer>("Forklift.fus");

            //_scene.Children.FindNodes(nodes => nodes.Name == "LeftFrontWheel")?.FirstOrDefault()?.GetComponent<Transform>();
            
            _camTransform = new Transform
            {
                Translation = new float3(0, 5, -40),
            };
            SceneNode cam = new SceneNode
            {
                Name = "Camera",
                Components =
                {
                    _camTransform,
                    new Camera(ProjectionMethod.Perspective, 5, 500, M.PiOver4)
                    {
                        BackgroundColor =  (float4) ColorUint.Greenery,
                    }
                },
            };

            _scene.Children.Add(cam);

            _rightRearTransform = _scene.Children.FindNodes(node => node.Name == "RightRearWheel")?.FirstOrDefault()?.GetTransform();
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
            _sceneRayCaster = new SceneRayCaster(_scene);

            await base.InitAsync();
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

        _camTransform.RotateAround(float3.Zero, new float3(0, Keyboard.LeftRightAxis * DeltaTime, 0));
        _sceneRayCaster = new SceneRayCaster(_scene);
        
         if (Mouse.LeftButton)
         {
             float2 pickPos = Mouse.Position;

             RayCastResult newPick = _sceneRayCaster.RayPick(RC, pickPos).OrderBy(rr => rr.DistanceFromOrigin).FirstOrDefault();

             if (newPick?.Node != _currentPick?.Node)
             {
                 if (_currentPick != null)
                 {  
                     var ef = _currentPick.Node.GetComponent<SurfaceEffect>();
                     ef.SurfaceInput.Albedo = _oldColor;
                 }
                 if (newPick != null)
                 {
                    var ef = newPick.Node.GetComponent<SurfaceEffect>();
                    _oldColor = ef.SurfaceInput.Albedo;
                    ef.SurfaceInput.Albedo = (float4) ColorUint.Violet;
                 }
                 _currentPick = newPick;
             }
         }

         if(_currentPick != null){
            _currentPickTransform = _currentPick.Node.GetTransform();

            if(_currentPick.Node.Name == "Axis"){
                if(_currentPickTransform.Rotation.y + 2f * Keyboard.ADAxis * DeltaTime <= Math.PI/10 && _currentPickTransform.Rotation.y + 2f * Keyboard.ADAxis * DeltaTime >= -Math.PI/10){
                 _currentPickTransform.Rotation = new float3
                        (
                            (float) _currentPickTransform.Rotation.x + 2f * Keyboard.WSAxis * DeltaTime,
                            (float) _currentPickTransform.Rotation.y + 2f * Keyboard.ADAxis * DeltaTime,
                            0
                        );
                }
            }else if(_currentPick.Node.Name == "BalckWheel"){
                _currentPickTransform.Rotation = new float3(
                    (float) _currentPickTransform.Rotation.x + 2f * Keyboard.WSAxis * DeltaTime,
                            0,
                            0
                );
            }else if(_currentPick.Node.Name == "Fork"){
                _currentPickTransform.Translation = new float3(
                    (float) _currentPickTransform.Translation.x + 0,
                    (float)_currentPickTransform.Translation.y + 2f * Keyboard.WSAxis * DeltaTime,
                    (float) _currentPickTransform.Translation.z + 0

                );
            }else if(_currentPick.Node.Name == "Chassis"){
                _currentPickTransform.Rotation = new float3(
                    _currentPickTransform.Rotation.x + 0,
                    (float) _currentPickTransform.Rotation.y + 2f * Keyboard.ADAxis * DeltaTime,
                    _currentPickTransform.Rotation.z + 0

                );
                
            }else{
                _currentPickTransform.Rotation = new float3(
                    (float) _currentPickTransform.Rotation.x + 2f * Keyboard.WSAxis * DeltaTime,
                    _currentPickTransform.Rotation.y + 0,
                    _currentPickTransform.Rotation.z + 0
                );
            }
            
         }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }
    }
}