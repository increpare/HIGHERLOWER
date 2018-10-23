using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MeshGen : MonoBehaviour {

    public int depth = 5;
    public float corridorwidth = 0.1f;
    public float ceilheight = 1.0f;

    public static int IntPow(int x, uint pow)
    {
        int ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }

    public int CorridorLen(int d)
    {
        d++;
        if (d==0){
            return 1;
        }
        var h = d / 2;
        var parity = d - 2 * h;
        return (2 + parity) * IntPow(2, (uint)h) - 1;
        //https://oeis.org/search?q=1%2C2%2C3%2C5%2C7%2C11%2C15%2C23%2C31&sort=&language=german&go=Suche
    }

    void GenMeshRecursive(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> tris, Vector3 origin, int depth, Vector3 forward, bool first)
    {

        var length = CorridorLen(depth);
        var length_next = CorridorLen(depth - 1);

        var target = origin + forward * length;
        Debug.DrawLine(origin, target, Color.yellow);
        var left = Vector3.Cross(forward, Vector3.up);
        var right = -left;

        var baseIndex = vertices.Count;

        Vector3 tl = origin + (corridorwidth) * left + (length + corridorwidth) * forward;
        Vector3 tr = origin + (corridorwidth) * right + (length + corridorwidth) * forward;
        Vector3 br = origin + (corridorwidth) * right + corridorwidth * forward;
        Vector3 bl = origin + (corridorwidth) * left + corridorwidth * forward;


        Vector3 tl_ceil = tl + Vector3.up * ceilheight;
        Vector3 tr_ceil = tr + Vector3.up * ceilheight;
        Vector3 br_ceil = br + Vector3.up * ceilheight;
        Vector3 bl_ceil = bl + Vector3.up * ceilheight;


        Vector2 kreuzung_orig = new Vector2(0.5f, 0.5f);
        float k_s = 0.4f;

        Vector2 uv_ground_tl = kreuzung_orig + k_s * (Vector2.left + Vector2.up);
        Vector2 uv_ground_tr = kreuzung_orig + k_s * (Vector2.right + Vector2.up);
        Vector2 uv_ground_br = kreuzung_orig + k_s * (Vector2.right + Vector2.down);
        Vector2 uv_ground_bl = kreuzung_orig + k_s * (Vector2.left + Vector2.down);


        Vector2 kreuzung_ceil_orig = new Vector2(0.5f, 0.5f);

        Vector3 uv_ceil_tl = kreuzung_ceil_orig + k_s * (Vector2.left + Vector2.up);
        Vector2 uv_ceil_tr = kreuzung_ceil_orig + k_s * (Vector2.right + Vector2.up);
        Vector2 uv_ceil_br = kreuzung_ceil_orig + k_s * (Vector2.right + Vector2.down);
        Vector2 uv_ceil_bl = kreuzung_ceil_orig + k_s * (Vector2.left + Vector2.down);


        vertices.Add(tl);
        vertices.Add(tr);
        vertices.Add(br);
        vertices.Add(bl);

        vertices.Add(tl_ceil);
        vertices.Add(tr_ceil);
        vertices.Add(br_ceil);
        vertices.Add(bl_ceil);


        uvs.Add(uv_ground_tl);
        uvs.Add(uv_ground_tr);
        uvs.Add(uv_ground_br);
        uvs.Add(uv_ground_bl);

        uvs.Add(uv_ceil_tl);
        uvs.Add(uv_ceil_tr);
        uvs.Add(uv_ceil_br);
        uvs.Add(uv_ceil_bl);


        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        normals.Add(Vector3.down);
        normals.Add(Vector3.down);
        normals.Add(Vector3.down);
        normals.Add(Vector3.down);


        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 1);
        tris.Add(baseIndex + 2);

        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 2);
        tris.Add(baseIndex + 3);


        tris.Add(baseIndex + 5);
        tris.Add(baseIndex + 4);
        tris.Add(baseIndex + 6);

        tris.Add(baseIndex + 6);
        tris.Add(baseIndex + 4);
        tris.Add(baseIndex + 7);

        //forward wall

        baseIndex = vertices.Count;

        float fcorsize = length_next;
        if (depth > 1)
        {
            fcorsize -= corridorwidth;
        }
        else if (depth==1)
        {
            fcorsize += corridorwidth;
        }
        else 
        {
            fcorsize = corridorwidth;
        }

        Vector3 wall_l = origin + (fcorsize) * left + (length + corridorwidth) * forward;
        Vector3 wall_r = origin + (fcorsize) * right + (length + corridorwidth) * forward;

        Vector3 wall_l_ceil = wall_l + Vector3.up * ceilheight;
        Vector3 wall_r_ceil = wall_r + Vector3.up * ceilheight;

        vertices.Add(wall_l);
        vertices.Add(wall_r);
        vertices.Add(wall_r_ceil);
        vertices.Add(wall_l_ceil);

        Vector2 wall_orig = new Vector2(0.5f, 0.5f);

        Vector3 uv_wall_tl = wall_orig + k_s * (Vector2.left + Vector2.up);
        Vector2 uv_wall_tr = wall_orig + k_s * (Vector2.right + Vector2.up);
        Vector2 uv_wall_br = wall_orig + k_s * (Vector2.right + Vector2.down);
        Vector2 uv_wall_bl = wall_orig + k_s * (Vector2.left + Vector2.down);

        uvs.Add(uv_wall_tl);
        uvs.Add(uv_wall_tr);
        uvs.Add(uv_wall_br);
        uvs.Add(uv_wall_bl);

        normals.Add(-forward);
        normals.Add(-forward);
        normals.Add(-forward);
        normals.Add(-forward);

        tris.Add(baseIndex + 1);
        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 2);

        tris.Add(baseIndex + 2);
        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 3);

        if (depth>0){
            //two walls on back side

            var f = length_next - corridorwidth;
            if (depth==1){
                f += 2 * corridorwidth;
            }

            Vector3 bwall_l_far = origin + (f) * left + (length - corridorwidth) * forward;
            Vector3 bwall_l_near = origin + (corridorwidth) * left + (length - corridorwidth) * forward;

            Vector3 bwall_r_far = origin + (f) * right + (length - corridorwidth) * forward;
            Vector3 bwall_r_near = origin + (corridorwidth) * right + (length - corridorwidth) * forward;

            Vector3 bwall_l_far_ceil = bwall_l_far + Vector3.up * ceilheight;
            Vector3 bwall_l_near_ceil = bwall_l_near + Vector3.up * ceilheight;

            Vector3 bwall_r_far_ceil = bwall_r_far + Vector3.up * ceilheight;
            Vector3 bwall_r_near_ceil = bwall_r_near + Vector3.up * ceilheight;

            baseIndex = vertices.Count;

            vertices.Add(bwall_l_far);
            vertices.Add(bwall_l_near);
            vertices.Add(bwall_l_near_ceil);
            vertices.Add(bwall_l_far_ceil);

            vertices.Add(bwall_r_far);
            vertices.Add(bwall_r_near);
            vertices.Add(bwall_r_near_ceil);
            vertices.Add(bwall_r_far_ceil);


            uvs.Add(uv_wall_tl);
            uvs.Add(uv_wall_tr);
            uvs.Add(uv_wall_br);
            uvs.Add(uv_wall_bl);

            uvs.Add(uv_wall_tl);
            uvs.Add(uv_wall_tr);
            uvs.Add(uv_wall_br);
            uvs.Add(uv_wall_bl);


            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);

            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);



            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 2);

            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 2);
            tris.Add(baseIndex + 3);

            tris.Add(baseIndex + 5);
            tris.Add(baseIndex + 4);
            tris.Add(baseIndex + 6);

            tris.Add(baseIndex + 6);
            tris.Add(baseIndex + 4);
            tris.Add(baseIndex + 7);
        }

        if (first){

            baseIndex = vertices.Count;

            Vector3 wallbase_lt = origin + (corridorwidth) * left + (length - corridorwidth) * forward;
            Vector3 wallbase_rt = origin + (corridorwidth) * right + (length - corridorwidth) * forward;
            Vector3 wallbase_rb = origin + (corridorwidth) * right + corridorwidth * forward;
            Vector3 wallbase_lb = origin + (corridorwidth) * left + corridorwidth * forward;

            Vector3 wallbase_lt_ceil = wallbase_lt + ceilheight*Vector3.up;
            Vector3 wallbase_rt_ceil = wallbase_rt + ceilheight * Vector3.up;
            Vector3 wallbase_rb_ceil = wallbase_rb + ceilheight * Vector3.up;
            Vector3 wallbase_lb_ceil = wallbase_lb + ceilheight * Vector3.up;



            vertices.Add(wallbase_lt);
            vertices.Add(wallbase_rt);
            vertices.Add(wallbase_rb);
            vertices.Add(wallbase_lb);

            vertices.Add(wallbase_lt_ceil);
            vertices.Add(wallbase_rt_ceil);
            vertices.Add(wallbase_rb_ceil);
            vertices.Add(wallbase_lb_ceil);

            vertices.Add(wallbase_rb);
            vertices.Add(wallbase_lb);
            vertices.Add(wallbase_rb_ceil);
            vertices.Add(wallbase_lb_ceil);


            uvs.Add(uv_wall_tl);
            uvs.Add(uv_wall_tr);
            uvs.Add(uv_wall_br);
            uvs.Add(uv_wall_bl);

            uvs.Add(uv_wall_tl);
            uvs.Add(uv_wall_tr);
            uvs.Add(uv_wall_br);
            uvs.Add(uv_wall_bl);

            uvs.Add(uv_wall_tl);
            uvs.Add(uv_wall_tr);
            uvs.Add(uv_wall_br);
            uvs.Add(uv_wall_bl);


            normals.Add(right);
            normals.Add(left);
            normals.Add(left);
            normals.Add(right);

            normals.Add(right);
            normals.Add(left);
            normals.Add(left);
            normals.Add(right);

            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);
            normals.Add(forward);

            tris.Add(baseIndex + 2);
            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 5);

            tris.Add(baseIndex + 6);
            tris.Add(baseIndex + 2);
            tris.Add(baseIndex + 5);


            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 3);
            tris.Add(baseIndex + 7);

            tris.Add(baseIndex + 4);
            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 7);


            tris.Add(baseIndex + 9);
            tris.Add(baseIndex + 8);
            tris.Add(baseIndex + 10);

            tris.Add(baseIndex + 11);
            tris.Add(baseIndex + 9);
            tris.Add(baseIndex + 10);

            /*
            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 5);
            tris.Add(baseIndex + 7);



            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 3);
            tris.Add(baseIndex + 6);

            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 6);
            tris.Add(baseIndex + 8);*/
        }

        if (depth > 0)
        {
            GenMeshRecursive(vertices, normals, uvs, tris, target, depth-1, left, false);
            GenMeshRecursive(vertices, normals, uvs, tris, target, depth-1, right, false);
        }
    }

    void GenMesh(Vector3 origin, int depth, Vector3 forward)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        GenMeshRecursive(vertices, normals, uvs,  tris, origin, depth, forward, true);

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        //mesh.uv = uvs.ToArray();
        //mesh.normals = normals.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();


        //MeshUtility.Optimize(mesh);

        MeshUtility.SetMeshCompression(mesh, ModelImporterMeshCompression.High);
        AssetDatabase.CreateAsset(mesh, "Assets/CompressedMesh.asset");
        AssetDatabase.SaveAssets();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private int lastdepth = -1;

	// Use this for initialization
	void Start () {
    }
	
    void DrawHGrid(Vector3 origin,int depth, Vector3 forward){
        var length = CorridorLen(depth);
        var target = origin + forward * length;
        Debug.DrawLine(origin,target,Color.yellow);
        if (depth>0)
        {
            var left = Vector3.Cross(forward, Vector3.up);
            var right = -left;
            DrawHGrid(target, depth - 1, left);
            DrawHGrid(target, depth - 1, right);
        }
    }

    public bool regen = false;
	// Update is called once per frame
	void Update () {
        if (regen){
            regen = false;
            GenMesh(transform.position, depth, Vector3.forward);
        }
    /*    if (lastdepth!=depth){
            lastdepth = depth;
            GenMesh(transform.position, depth, Vector3.forward);
        }*/
        //DrawHGrid(transform.position,depth,Vector3.forward);
	}
}
