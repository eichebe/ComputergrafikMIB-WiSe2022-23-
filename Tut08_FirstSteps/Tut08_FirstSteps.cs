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

        private CuboidMesh _cubeMesh;

        private Transform _cubeTransform2;

        private Transform _longCubeTransform;

        private Transform _randomCubeTransform;


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
                BackgroundColor = (float4)ColorUint.DarkGray
            };

            var cameraNode = new SceneNode();
            cameraNode.Components.Add(_camera);
            _cameraTransform = new Transform{ Translation = new float3(0, 0, -50)};
            cameraNode.Components.Add(_cameraTransform);
            cameraNode.Components.Add(_camera);
            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);

            //THE CUBE
            //Original Cube
            _cubeTransform = new Transform{Rotation = new float3(0,45,0)};
            _cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue);
            _cubeMesh = new CuboidMesh(new float3(10, 10, 10));
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeEffect);
            cubeNode.Components.Add(_cubeMesh);

            //Cube Rotated to create a star           
            var cubeMesh2 = new CuboidMesh(new float3(10, 10, 10));
            var cubeNode2 = new SceneNode();
            _cubeTransform2 = new Transform{Translation = new float3(100,100,0)};
            _cubeTransform2 = new Transform{Rotation = new float3(0,10,0)};
            var cubeEffect2 = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Red);
            
            
            cubeNode.Components.Add(_cubeTransform2);
            cubeNode.Components.Add(cubeEffect2);
            cubeNode.Components.Add(cubeMesh2);

            //Up down long Cube
            _longCubeTransform = new Transform{Translation = new float3(30,1,20)}; 
            CuboidMesh longCube = new CuboidMesh(new float3(10,40,10));
            var longCubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Gold);
            var longCubeNode = new SceneNode();
            longCubeNode.Components.Add(_longCubeTransform);
            longCubeNode.Components.Add(longCubeEffect);
            longCubeNode.Components.Add(longCube);

            //Random Snowflake Cubes
            
            for(int i = 0; i<60; i++){
            Random rd = new Random();
            var randomCubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Snow);
            _randomCubeTransform = new Transform{Translation = new float3(rd.Next(0,60),rd.Next(0,60),0)};
            CuboidMesh randomCube = new CuboidMesh(new float3(1,1,1));
            longCubeNode.Components.Add(_randomCubeTransform);
            longCubeNode.Components.Add(randomCubeEffect);
            longCubeNode.Components.Add(randomCube);
            }
            

            //THE SCENE
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(longCubeNode);
           
            
            
            
            //THE SCENE RENDERER
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            
            _cubeTransform.Rotation = new float3( _cubeTransform.Rotation.y +0.05f,_cubeTransform.Rotation.y +0.05f,0);

            _cubeTransform.Translation = new float3(5 * M.Sin(3 * TimeSinceStart),5 * M.Cos(3 * TimeSinceStart),0);
            
            _longCubeTransform.Translation = new float3(30,20*M.Cos(6 * TimeSinceStart), 20);
            Random rd = new Random();
            for(int i = 0; i<60; i++){
            _randomCubeTransform = new Transform{Translation = new float3(rd.Next(0,60),rd.Next(0,60),0)};
            }

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