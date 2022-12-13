using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using Fusee.Engine.Core.Effects;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.Gui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;

        private Camera _camera;

        private Transform _cubeTransform;
        private Transform _cameraTransform; 

        private SurfaceEffect _cubeEffect;


        // Init is called on startup. 
        public override void Init()
        {
            // Create a scene tree containing the camera :
            // _scene---+
            //          |
            //          +---cameraNode-----_camera

            // THE CAMERA
            // A node containing one Camera component.
            _camera = new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4)
            {
                BackgroundColor = new float4(0.5f, 0.5f, 1.0f, 1.0f) //(float4)ColorUint.Greenery
            };

            var cameraNode = new SceneNode();
            cameraNode.Components.Add(_camera);
            _cameraTransform = new Transform{ Translation = new float3(0, 0, -50)};

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);

            //THE CUBE
            
            _cubeTransform= new Transform{ Translation = new float3(0, 0, 50), Rotation = new float3(0,45,0)};
            
            _cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue);
            var cubeMesh = new CuboidMesh(new float3(10, 10, 10));

            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeEffect);
            cubeNode.Components.Add(cubeMesh);

            //THE SCENE
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);
            _scene.Children.Add(cubeNode);
            
            
            //THE SCENE RENDERER
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            _cubeTransform.Rotation = new float3(0, _cubeTransform.Rotation.y +0.01f, 0);

            _cubeTransform.Translation = new float3(5 * M.Sin(3 * TimeSinceStart),0,0);

            //change color per second
            _cubeEffect.SurfaceInput.Albedo = new float4(M.Sin(3 * Time.TimeSinceStart) *0.5f + 0.5f, //Rotkanal 
                                                         M.Sin(7 * Time.TimeSinceStart) *0.5f + 0.5f, //Grünkanal
                                                         M.Sin(6 * Time.TimeSinceStart) *0.5f + 0.5f, //Blaukanal
                                                         1);    //Alfawert helligkeit
            
            // Render the scene tree
            _sceneRenderer.Render(RC);

            //Gradwert/360 * 2* pi (Grad in Radiant umrechenen)

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

    }
}