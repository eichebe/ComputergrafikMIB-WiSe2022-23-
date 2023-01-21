using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;

namespace FuseeApp
{
    public class CuboidMesh : Mesh
    {
        public CuboidMesh(float3 size)
        {
            Vertices = new MeshAttributes<float3>(new float3[]
            {
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = +0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z},
                new float3 {x = +0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = +0.5f * size.z},
                new float3 {x = -0.5f * size.x, y = -0.5f * size.y, z = -0.5f * size.z}
            });

            Triangles = new MeshAttributes<uint>(new uint[]
            {
                // front face
                0, 2, 1, 0, 3, 2,
                // right face
                4, 6, 5, 4, 7, 6,
                // back face
                8, 10, 9, 8, 11, 10,
                // left face
                12, 14, 13, 12, 15, 14,
                // top face
                16, 18, 17, 16, 19, 18,
                // bottom face
                20, 22, 21, 20, 23, 22
            });

            Normals = new MeshAttributes<float3>(new float3[]
            {
                new float3(0, 0, 1),
                new float3(0, 0, 1),
                new float3(0, 0, 1),
                new float3(0, 0, 1),
                new float3(1, 0, 0),
                new float3(1, 0, 0),
                new float3(1, 0, 0),
                new float3(1, 0, 0),
                new float3(0, 0, -1),
                new float3(0, 0, -1),
                new float3(0, 0, -1),
                new float3(0, 0, -1),
                new float3(-1, 0, 0),
                new float3(-1, 0, 0),
                new float3(-1, 0, 0),
                new float3(-1, 0, 0),
                new float3(0, 1, 0),
                new float3(0, 1, 0),
                new float3(0, 1, 0),
                new float3(0, 1, 0),
                new float3(0, -1, 0),
                new float3(0, -1, 0),
                new float3(0, -1, 0),
                new float3(0, -1, 0)
            });

            UVs = new MeshAttributes<float2>(new float2[]
            {
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0),
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0),
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0),
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0),
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0),
                new float2(1, 0),
                new float2(1, 1),
                new float2(0, 1),
                new float2(0, 0)
            });
        }
    }

public class CylinderMesh : Mesh
{
    public CylinderMesh(float radius, float height, int segments) {
        
        // calculate the total number of vertices needed for the cylinder 
        // 4 vertices for each segment (top, bottom, 2 sides) + 2 for the top and bottom center
        float3[] verts = new float3[4*segments+2];
        float3[] norms = new float3[4*segments+2];
        uint[] tris = new uint[4*segments*3];

        // calculate the angle change between each segment
        float delta = 2 * M.Pi / segments;

        // create the top of the cylinder
        // top vertex 
        verts[0] = new float3(radius, 0.5f * height, 0);
        norms[0] = new float3(0, 1, 0);
        // top center
        verts[4*segments+1] = new float3(0, 0.5f * height, 0);
        norms[4*segments+1] = new float3(0, 1, 0);

        // create the bottom of the cylinder
        // bottom vertex
        verts[segments] = new float3(radius, -0.5f * height, 0);
        norms[segments] = new float3(0, -1, 0);
        // bottom center
        verts[4*segments] = new float3(0, -0.5f * height, 0);
        norms[4*segments] = new float3(0, -1, 0);

        //Top Side
        verts[2*segments] = new float3(radius, 0.5f * height, 0);
        norms[2*segments] = new float3(1, 0, 0);

        //Bottom Side
        verts[3*segments] = new float3(radius, -0.5f * height, 0);
        norms[3*segments] = new float3(1, 0, 0);

        //create the sides
        for (int i = 1; i < segments; i++)
        {
            // calculate the x and y positions of each vertex
            float x = radius * M.Cos(i*delta);
            float y = radius * M.Sin(i*delta);

            // calculate the normals for the side of the cylinder
            float x_normale = M.Cos(i*delta);
            float y_normale = M.Sin(i*delta);

            // top vertex
            verts[i] = new float3(x, 0.5f * height,y);

            norms[i] = new float3(0, 1, 0);

            // creating the triangles for the top of the cylinder
            int tri_index = ((i-1)*3);
            tris[tri_index + 0] = (uint) i-1;
            tris[tri_index + 1] = (uint) i;
            tris[tri_index + 2] = (uint) (4*segments+1);
            tri_index = 0;
            
            // side vertices
            verts[2*segments+i] = new float3(x, 0.5f * height,y);
            verts[3*segments+i] = new float3(x, -0.5f * height,y);

            norms[2*segments+i] = new float3(x_normale, 0, y_normale);
            norms[3*segments+i] = new float3(x_normale, 0, y_normale);

            // creating the triangles for the sides of the cylinder
            tri_index = (2*segments+i-1)*3;
            tris[tri_index + 0] = (uint) (2*segments+i-1);
            tris[tri_index + 1] = (uint) (3*segments+i-1);
            tris[tri_index + 2] = (uint) (2*segments+i);
            tri_index = 0;

            tri_index = (3*segments+i-1)*3;
            tris[tri_index + 0] = (uint) (3*segments+i-1);
            tris[tri_index + 1] = (uint) (3*segments+i);
            tris[tri_index + 2] = (uint) (2*segments+i);
            tri_index = 0;
            // bottom vertex
            verts[segments+i] = new float3(x, -0.5f * height,y);
            norms[segments+i] = new float3(0, -1, 0);

            // creating the triangles for the bottom of the cylinder
            tri_index = (segments+i-1)*3;
            tris[tri_index + 0] = (uint) (segments+i-1);
            tris[tri_index + 1] = (uint) (segments+i);
            tris[tri_index + 2] = (uint) (4*segments);
            tri_index = 0;

            
        }
            //Top
            tris[3*segments-3] = (uint) segments-1;
            tris[3*segments-2] = 0;
            tris[3*segments-1] = (uint) (4*segments+1);

            //Bottom
            tris[2*3*segments-3] = (uint) (2*segments-1);
            tris[2*3*segments-2] = (uint) segments;
            tris[2*3*segments-1] = (uint) (4*segments);

            //Side
            tris[3*3*segments-3] = (uint) (4*segments-1);
            tris[3*3*segments-2] = (uint) (2*segments);
            tris[3*3*segments-1] = (uint) (3*segments-1);

            tris[4*3*segments-3] = (uint) (3*segments);
            tris[4*3*segments-2] = (uint) (2*segments);
            tris[4*3*segments-1] = (uint) (4*segments-1);
            
            Vertices = new MeshAttributes<float3>(verts);
            Normals = new MeshAttributes<float3>(norms);
            Triangles = new MeshAttributes<uint>(tris);
    }
}

       

      
    

        




        
    public class ConeMesh : ConeFrustumMesh
    {
        public ConeMesh(float radius, float height, int segments) : base(radius, 0.0f, height, segments) { }
    }

    public class ConeFrustumMesh : Mesh
    {
        public ConeFrustumMesh(float radiuslower, float radiusupper, float height, int segments)
        {
            throw new NotImplementedException();
        }
    }

    public class PyramidMesh : Mesh
    {
        public PyramidMesh(float baselen, float height)
        {
            throw new NotImplementedException();
        }
    }

    public class TetrahedronMesh : Mesh
    {
        public TetrahedronMesh(float edgelen)
        {
            throw new NotImplementedException();
        }
    }

    public class TorusMesh : Mesh
    {
        public TorusMesh(float mainradius, float segradius, int segments, int slices)
        {
            throw new NotImplementedException();
        }
    }
}
