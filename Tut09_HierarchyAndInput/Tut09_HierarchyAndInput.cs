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
        private Transform _cameraAngle;
        private Transform _baseTransform;
        private Transform _bodyTransform;
        private Transform _robotUpperArmTransform;
        private Transform _robotFrontArmTransform;
        private Transform _robotHandArmLeftTransform;
        private Transform _robotHandTopArmTransform;
        private Transform _robotHandArmRightTransform;
        private Boolean _handOpen = false;
        private Boolean _handClosed = false;
        private Boolean _cameraMoved;
        private float _camVelocity;
        


        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _cameraAngle = new Transform
            {
                Translation = new float3(0, 0, 0),
            };

            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new Transform
            {
                Translation = new float3(0, 6, 0),
                Rotation = new float3(0, (float) Math.PI/2, 0)
            };

            _robotUpperArmTransform = new Transform
            {
                Translation = new float3(2, 4, 0),
                Rotation = new float3((float) Math.PI/4, 0, 0)
            };

            _robotFrontArmTransform = new Transform
            {
                Translation = new float3(-2, 4, 0),
                Rotation = new float3((float) Math.PI/3, 0, 0)
            };

            _robotHandArmLeftTransform = new Transform
            {
                Translation = new float3(0, 4, 0),
            };

            _robotHandArmRightTransform = new Transform
            {
                Translation = new float3(0, 4, 0),
            };

            _robotHandTopArmTransform = new Transform
            {
                Translation = new float3(0, 4, 0),
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
                            _cameraAngle
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
                        Name = "Base",
                        Components =
                        {
                            // TRANSFORM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),

                            // MESH COMPONENT
                            new CuboidMesh(new float3(10, 2, 10))
                        },
                        Children =
                        {
                            new SceneNode
                            {
                                Name = "RobotBody",
                                Components = 
                                {
                                    _bodyTransform,
                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.IndianRed),
                                    new CuboidMesh(new float3(2, 10, 2))
                                },
                                Children =
                                {
                                    new SceneNode
                                    {
                                        Name = "RobotUpperArm",
                                        Components = 
                                        {
                                            _robotUpperArmTransform,
                                        },
                                        Children = 
                                        {
                                            new SceneNode
                                            {
                                                Components =
                                                {
                                                    new Transform { Translation = new float3(0, 4, 0)},
                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.ForestGreen),
                                                    new CuboidMesh(new float3(2, 10, 2))
                                                },
                                                Children =
                                                {
                                                    new SceneNode
                                                    {
                                                        Name = "RobotFrontArm",
                                                        Components = 
                                                        {
                                                            _robotFrontArmTransform,
                                                        },
                                                        Children = 
                                                        {
                                                            new SceneNode
                                                            {
                                                                Components =
                                                                {
                                                                    new Transform { Translation = new float3(0, 4, 0)},
                                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.SkyBlue),
                                                                    new CuboidMesh(new float3(2, 10, 2))
                                                                },
                                                                Children =
                                                                {
                                                                    new SceneNode
                                                                    {
                                                                        Name = "HandArmLeft",
                                                                        Components = 
                                                                        {
                                                                            _robotHandArmLeftTransform,
                                                                        },
                                                                        Children = 
                                                                        {
                                                                            new SceneNode
                                                                            {
                                                                                Components =
                                                                                {
                                                                                    new Transform 
                                                                                    {
                                                                                        Translation = new float3(1, 2, 1),
                                                                                        Rotation = new float3(0, (float) Math.PI/4, 0)
                                                                                    },
                                                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),
                                                                                    new CuboidMesh(new float3(1, 3, 1))
                                                                                }
                                                                                
                                                                            },
                                                                        }
                                                                    },
                                                                    new SceneNode
                                                                    {
                                                                        Name = "HandArmRight",
                                                                        Components = 
                                                                        {
                                                                            _robotHandArmRightTransform,
                                                                        },
                                                                        Children =
                                                                        {
                                                                            new SceneNode
                                                                            {
                                                                                Components =
                                                                                {
                                                                                    new Transform 
                                                                                    {
                                                                                        Translation = new float3(-1, 2, 1),
                                                                                        Rotation = new float3(0, (float) Math.PI/4, 0)
                                                                                    },
                                                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),
                                                                                    new CuboidMesh(new float3(1, 3, 1))
                                                                                }
                                                                                
                                                                            }
                                                                        }
                                                                    },
                                                                    new SceneNode
                                                                    {
                                                                        Name = "HandTopArm",
                                                                        Components = 
                                                                        {
                                                                            _robotHandTopArmTransform,
                                                                        },
                                                                        Children =
                                                                        {
                                                                            new SceneNode
                                                                            {
                                                                                Components =
                                                                                {
                                                                                    new Transform { Translation = new float3(0, 2, -1)},
                                                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),
                                                                                    new CuboidMesh(new float3(1, 3, 1))
                                                                                }
                                                                                
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        public override void RenderAFrame()
        {
        
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

       
            if(Mouse.LeftButton) {
                _camVelocity = Mouse.Velocity.x;
                _cameraAngle.Rotation = new float3(0, _cameraAngle.Rotation.y + 0.5f*((float)Math.PI/180) * DeltaTime * _camVelocity, 0);
                _cameraMoved = true;
            }

         
            if(_cameraMoved == true) {
                _camVelocity = _camVelocity/1.1f;
                _cameraAngle.Rotation = new float3(0, _cameraAngle.Rotation.y + 0.5f*((float)Math.PI/180) * DeltaTime * _camVelocity, 0);
                if(_camVelocity > -0.1 & _camVelocity < 0.1) {
                    _cameraMoved = false;
                }
            }

      
            _bodyTransform.Rotation = new float3(0, _bodyTransform.Rotation.y + 90*((float)Math.PI/180) * DeltaTime * Keyboard.LeftRightAxis, 0);
            _robotUpperArmTransform.Rotation = new float3(_robotUpperArmTransform.Rotation.x + 45*((float)Math.PI/180) * DeltaTime * Keyboard.UpDownAxis, 0, 0);
            _robotFrontArmTransform.Rotation = new float3(_robotFrontArmTransform.Rotation.x + 60*((float)Math.PI/180) * DeltaTime * Keyboard.UpDownAxis, 0, 0);

            if(Keyboard.GetKey(KeyCodes.A)) {
                _handOpen = true;
                _handClosed = false;
            }
            if(Keyboard.GetKey(KeyCodes.D)) {
                _handClosed = true;
                _handOpen = false;
            }
            if(_handClosed == true) {
                float rotation = 10*((float)Math.PI/180);
                if(_robotHandArmLeftTransform.Rotation.x <= rotation) {
                    _robotHandArmLeftTransform.Rotation = new float3(_robotHandArmLeftTransform.Rotation.x + rotation * DeltaTime, 0, _robotHandArmLeftTransform.Rotation.z + (-rotation) * DeltaTime);
                    _robotHandArmRightTransform.Rotation = new float3(_robotHandArmRightTransform.Rotation.x + rotation * DeltaTime, 0, _robotHandArmRightTransform.Rotation.z + rotation * DeltaTime);
                    _robotHandTopArmTransform.Rotation = new float3(_robotHandTopArmTransform.Rotation.x + (-rotation) * DeltaTime, 0, 0);
                } else {
                    _handClosed = false;
                }
            }

            if(_handOpen == true) {
                float rotation = 10*((float)Math.PI/180);
                if(_robotHandArmLeftTransform.Rotation.x >= -rotation) {
                    _robotHandArmLeftTransform.Rotation = new float3(_robotHandArmLeftTransform.Rotation.x + (-rotation) * DeltaTime, 0, _robotHandArmLeftTransform.Rotation.z + rotation * DeltaTime);
                    _robotHandArmRightTransform.Rotation = new float3(_robotHandArmRightTransform.Rotation.x + (-rotation) * DeltaTime, 0, _robotHandArmRightTransform.Rotation.z + (-rotation) * DeltaTime);
                    _robotHandTopArmTransform.Rotation = new float3(_robotHandTopArmTransform.Rotation.x + rotation * DeltaTime, 0, 0);
                } else {
                    _handOpen = false;
                }
            }

            

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
    }
}