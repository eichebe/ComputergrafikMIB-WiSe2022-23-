using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Engine.Core.Effects;
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
    [FuseeApplication(Name = "Tut09_HierarchyAndInput", Description = "Yet another FUSEE App.")]
    public class Tut09_HierarchyAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _camAngle;
        private Transform _baseTransform;
        private Transform _bodyTransform;
        private Transform _upperRobotArmTransform;
        private Transform _frontRobotArmTransform;
        private Transform _robotHandArmLeftTransform;
        private Transform _robotHandArmRightTransform;
        private float _cameraSpeed;
        private Transform _robotUpperArmTransform;
        private Boolean _cameraMoved;
        private Boolean _robotHandOpen = false;
        private Boolean _robotHandClosed = false;

        private Transform _bodyTransform;

        private Transform _upperArmTransform;


        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _camAngle = new Transform
            {
                Translation = new float3(0, 0, 0),
            };

            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children =
                {
                    new SceneNode
                    {
                        Name = "Camera",
                        Components =
                        {
                            _camAngle
                        },
                        Children =
                        {
                            new SceneNode
                            {
                                Components =
                                {
                                    new Transform
                                    {
                                        Translation = new float3(0, 10, -50),
                                    },
                                    new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4)
                                    {
                                        BackgroundColor =  (float4) ColorUint.Greenery
                                    }
                                }
                            }
                        }
                    },

                    new SceneNode
                    {
                        Name = "Robot",
                        Components =
                        {
                            _baseTransform,
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),
                            new CuboidMesh(new float3(10, 2, 10))
                        }
                    }
                }
            };
        }



        // Init is called on startup. 
        public override void Init()
        {
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            if(Mouse.LeftButton) {
                _cameraSpeed = Mouse.Velocity.x;
                _camAngle.Rotation = new float3(0, _camAngle.Rotation.y + 0.5f*((float)Math.PI/180) * DeltaTime * _cameraSpeed, 0);
                _cameraMoved = true;
            }

            if(_cameraMoved == true) {
                _cameraSpeed = _cameraSpeed/1.1f;
                _camAngle.Rotation = new float3(0, _camAngle.Rotation.y + 0.5f*((float)Math.PI/180) * DeltaTime * _cameraSpeed, 0);
                if(_cameraSpeed > -0.1 & _cameraSpeed < 0.1) {
                    _cameraMoved = false;
                }
            }

            _bodyTransform.Rotation = new float3(0, _bodyTransform.Rotation.y + 90*((float)Math.PI/180) * DeltaTime * Keyboard.LeftRightAxis, 0);
            _upperRobotArmTransform.Rotation = new float3(_upperRobotArmTransform.Rotation.x + 45*((float)Math.PI/180) * DeltaTime * Keyboard.UpDownAxis, 0, 0);
            _frontRobotArmTransform.Rotation = new float3(_frontRobotArmTransform.Rotation.x + 60*((float)Math.PI/180) * DeltaTime * Keyboard.UpDownAxis, 0, 0);

            if(Keyboard.GetKey(KeyCodes.D)) {
                _robotHandOpen = true;
                _robotHandClosed = false;
            }
                if(Keyboard.GetKey(KeyCodes.A)) {
                _robotHandClosed = true;
                _robotHandOpen = false;
            }

            

            if(_robotHandClosed == true) {
                float rotation = 10*((float)Math.PI/180);
                if(_robotHandArmLeftTransform.Rotation.x <= rotation) {
                    _robotHandArmLeftTransform.Rotation = new float3(_robotHandArmLeftTransform.Rotation.x + rotation * DeltaTime, 0, _robotHandArmLeftTransform.Rotation.z + (-rotation) * DeltaTime);
                    _robotHandArmRightTransform.Rotation = new float3(_robotHandArmRightTransform.Rotation.x + rotation * DeltaTime, 0, _robotHandArmRightTransform.Rotation.z + rotation * DeltaTime);
                    _robotUpperArmTransform.Rotation = new float3(_robotUpperArmTransform.Rotation.x + (-rotation) * DeltaTime, 0, 0);
                } else {
                    _robotHandClosed = false;
                }
            }

            if(_robotHandOpen == true) {
                float rotation = 10*((float)Math.PI/180);
                if(_robotHandArmLeftTransform.Rotation.x >= -rotation) {
                    _robotHandArmLeftTransform.Rotation = new float3(_robotHandArmLeftTransform.Rotation.x + (-rotation) * DeltaTime, 0, _robotHandArmLeftTransform.Rotation.z + rotation * DeltaTime);
                    _robotHandArmRightTransform.Rotation = new float3(_robotHandArmRightTransform.Rotation.x + (-rotation) * DeltaTime, 0, _robotHandArmRightTransform.Rotation.z + (-rotation) * DeltaTime);
                    _robotUpperArmTransform.Rotation = new float3(_robotUpperArmTransform.Rotation.x + rotation * DeltaTime, 0, 0);
                } else {
                    _robotHandOpen = false;
                }
            }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
    }
}