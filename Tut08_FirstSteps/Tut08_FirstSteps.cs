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

        private Transform[] _cubeSnowflakeTransform;

        private SurfaceEffect[] _cubeSnowflakeEffect;

        private Transform _snowflakesTransform;

        private SurfaceEffect _snowflakesEffect;

        private float _cubeAngle = 0;

        private Transform _bigSnowflakeTransform;

        private Transform[] _BigSnowflakeCubeTransformArr;

        private SurfaceEffect[] _bigSnowflakeCubeEffect;

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
                BackgroundColor = (float4)ColorUint.DarkBlue
            };

            var cameraNode = new SceneNode();
            cameraNode.Components.Add(_camera);
            _cameraTransform = new Transform { Translation = new float3(0, 0, -50) };
            cameraNode.Components.Add(_cameraTransform);
            cameraNode.Components.Add(_camera);
            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);

            //THE CUBE
            //Original Cube
            _cubeTransform = new Transform { Rotation = new float3(0, 45, 0) };
            _cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue);
            _cubeMesh = new CuboidMesh(new float3(10, 10, 10));
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeEffect);
            cubeNode.Components.Add(_cubeMesh);

            //Cube Rotated to create a star           
            var cubeMesh2 = new CuboidMesh(new float3(10, 10, 10));
            var cubeNode2 = new SceneNode();
            _cubeTransform2 = new Transform { Translation = new float3(100, 100, 0) };
            _cubeTransform2 = new Transform { Rotation = new float3(0, 10, 0) };
            var cubeEffect2 = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Yellow);
            cubeNode.Components.Add(_cubeTransform2);
            cubeNode.Components.Add(cubeEffect2);
            cubeNode.Components.Add(cubeMesh2);

            //Up down long Cube
            _longCubeTransform = new Transform { Translation = new float3(30, 1, 20) };
            CuboidMesh longCube = new CuboidMesh(new float3(10, 40, 10));
            var longCubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Gold);
            var longCubeNode = new SceneNode();
            longCubeNode.Components.Add(_longCubeTransform);
            longCubeNode.Components.Add(longCubeEffect);
            longCubeNode.Components.Add(longCube);

            //Moving Big Snowflake Cubes
            _bigSnowflakeTransform = new Transform {Translation = new float3(-20,20,20),
                                                    Rotation = new float3(0,0,-10)};
            
            CuboidMesh bigSnowflake = new CuboidMesh(new float3(5,15,2));
            var BigSnowflakeCubeNode = new SceneNode();
            
            BigSnowflakeCubeNode.Components.Add(_cubeEffect);
            BigSnowflakeCubeNode.Components.Add(_bigSnowflakeTransform);
            BigSnowflakeCubeNode.Components.Add(_cubeMesh);
            
            var bigSnowflakeParts = 3;
            
            SceneNode[] bigSnowflakeNode = new SceneNode[bigSnowflakeParts];
            _BigSnowflakeCubeTransformArr = new Transform[bigSnowflakeParts];
            _bigSnowflakeCubeEffect = new SurfaceEffect[bigSnowflakeParts];

            for(var j = 0; j < bigSnowflakeParts; j++){
                
                bigSnowflakeNode[j] = new SceneNode();
                
                if(j==0){
                    _BigSnowflakeCubeTransformArr[j] = new Transform()
                    {
                        Translation = new float3(-23.5f,10,20),
                        Rotation = new float3(0,0,-10),
                    };
                }else if(j==1){
                    _BigSnowflakeCubeTransformArr[j] = new Transform()
                    {
                        Translation = new float3(-17,20,20),
                        Rotation = new float3(0,0,10),
                    };
                }else{
                    _BigSnowflakeCubeTransformArr[j] = new Transform()
                    {
                        Translation = new float3(-23.5f,10,20),
                        Rotation = new float3(0,0,M.Pi/2),
                    };
                }
                bigSnowflakeNode[j].Components.Add(_BigSnowflakeCubeTransformArr[j]);
                bigSnowflakeNode[j].Components.Add(_cubeEffect);
                bigSnowflakeNode[j].Components.Add(bigSnowflake);
            }

            
            //Snowflake Cubes
            _snowflakesTransform = new Transform { Translation = new float3(30, 1, 20), Scale = new float3(4f, 5f, 0) };
            _snowflakesEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Snow);
            CuboidMesh snowflake = new CuboidMesh(new float3(1,1,1));
            var cubeSnowflake = new SceneNode();
            
            cubeSnowflake.Components.Add(_snowflakesEffect);
            cubeSnowflake.Components.Add(_snowflakesTransform);
            cubeSnowflake.Components.Add(_cubeMesh);
            var amountSnowflakes = 100;

            SceneNode[] snowflakesNode = new SceneNode[amountSnowflakes];
            _cubeSnowflakeTransform = new Transform[amountSnowflakes];
            _cubeSnowflakeEffect = new SurfaceEffect[amountSnowflakes];
            
          

            for(var i = 0; i < amountSnowflakes; i++)
            {
                
                
                snowflakesNode[i] = new SceneNode();
                _cubeSnowflakeTransform[i] = new Transform()
                {
                    Rotation = new float3(_cubeTransform.Rotation.y + 0.05f, _cubeTransform.Rotation.y + 0.05f, 0),
                    Translation = new float3(10,20,0),
                };


                //_cubeSnowflakeEffect[i] = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue);

                snowflakesNode[i].Components.Add(_cubeSnowflakeTransform[i]);
                snowflakesNode[i].Components.Add(_snowflakesEffect);
                snowflakesNode[i].Components.Add(snowflake);
            }


            //THE SCENE
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(longCubeNode);
            for (int i = 0; i < amountSnowflakes; i++){
                _scene.Children.Add(snowflakesNode[i]);
            }

            for (int j = 0; j < bigSnowflakeParts; j++) {
                _scene.Children.Add(bigSnowflakeNode[j]);
            }




            //THE SCENE RENDERER
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            _cubeAngle = _cubeAngle + 90.0f * M.Pi / 180.0f * DeltaTime;
           
            _cubeTransform.Rotation = new float3(_cubeTransform.Rotation.y + 0.05f, _cubeTransform.Rotation.y + 0.05f, 0);

            _cubeTransform.Translation = new float3(5 * M.Sin(3 * TimeSinceStart), 5 * M.Cos(3 * TimeSinceStart), 0);

            _longCubeTransform.Translation = new float3(30, 20 * M.Cos(6 * TimeSinceStart), 20);

            

            Random rd  = new Random();
            var randomOne = rd.Next(-30,30);
            var randomTwo = rd.Next(-30,30);
            
           
            for (int i = 0; i < _cubeSnowflakeTransform.Length; i++){

                _cubeSnowflakeTransform[i].Rotation = new float3(0, 0, _cubeAngle);
                _cubeSnowflakeTransform[i].Translation = new float3(randomOne, randomTwo,0);
               
            }

            for (int j = 0; j < _BigSnowflakeCubeTransformArr.Length; j++) {
                
                _BigSnowflakeCubeTransformArr[j].Translation = new float3(-23.5f,10, -20 * M.Cos(3 * TimeSinceStart));
            }

            //change color per second
            _cubeEffect.SurfaceInput.Albedo = new float4(M.Sin(3 * Time.TimeSinceStart) * 0.5f + 0.5f, //Rotkanal 
                                                         M.Sin(7 * Time.TimeSinceStart) * 0.5f + 0.5f, //Grünkanal
                                                         M.Sin(6 * Time.TimeSinceStart) * 0.5f + 0.5f, //Blaukanal
                                                         1);    //Alfawert helligkeit

            // Render the scene tree
            _sceneRenderer.Render(RC);

            //Gradwert/360 * 2* pi (Grad in Radiant umrechenen)

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

    }
}