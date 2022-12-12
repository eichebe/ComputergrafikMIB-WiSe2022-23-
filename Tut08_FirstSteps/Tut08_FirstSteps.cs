using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
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

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);

            //THE CUBE
            var cubeTransform = new Transform{ Translation = new float3(0, 0, 50)};
            var cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue);
            var cubeMesh = new CuboidMesh(new float3(10, 10, 10));

            var cubeNode = new SceneNode();
            cubeNode.Components.Add(cubeTransform);
            cubeNode.Components.Add(cubeEffect);
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

            // Render the scene tree
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

    }
}