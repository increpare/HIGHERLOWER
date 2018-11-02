using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MeshGen : MonoBehaviour {

    public GameObject urn;
    public GameObject dong;

    public int depth = 5;
    public float corridorwidth = 0.1f;
    public float ceilheight = 1.0f;

    public float portraitGroße = 0.1f;
    public float portraitRaum = 0.01f;

    public MeshFilter wände;
    public MeshFilter schilder;
    public MeshFilter steine;

    public TextAsset lösungtext;
    
    public static int IntPow(int x, int pow)
    {
        int ret = 1;
        while (pow > 0)
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
        return (2 + parity) * IntPow(2, h) - 1;
        //https://oeis.org/search?q=1%2C2%2C3%2C5%2C7%2C11%2C15%2C23%2C31&sort=&language=german&go=Suche
    }

    private string lösung;

    public int[] fehlendersteinZahl;

    void GenMeshRecursive(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> tris, List<Vector3> vertices_sch, List<Vector3> normals_sch, List<Vector2> uvs_sch, List<int> tris_sch, List<Vector3> vertices_steine, List<Vector3> normals_steine, List<Vector2> uvs_steine, List<int> tris_steine, Vector3 origin, int depth, Vector3 forward, bool first, int maxdepth, int rowstartindex, int index, bool allright, bool allwrong)
    {        
        var length = CorridorLen(depth);
        var length_next = CorridorLen(depth - 1);

        var target = origin + forward * length;
        Debug.DrawLine(origin, target, Color.yellow);
        var left = Vector3.Cross(forward, Vector3.up);
        var right = -left;

        var baseIndex = vertices.Count;

        // Vector3 tl = origin + (corridorwidth) * left + (length + corridorwidth) * forward;
        // Vector3 tr = origin + (corridorwidth) * right + (length + corridorwidth) * forward;
        // Vector3 br = origin + (corridorwidth) * right + corridorwidth * forward;
        // Vector3 bl = origin + (corridorwidth) * left + corridorwidth * forward;


        // Vector3 tl_ceil = tl + Vector3.up * ceilheight;
        // Vector3 tr_ceil = tr + Vector3.up * ceilheight;
        // Vector3 br_ceil = br + Vector3.up * ceilheight;
        // Vector3 bl_ceil = bl + Vector3.up * ceilheight;


        // Vector2 kreuzung_orig = new Vector2(0.5f, 0.5f);
        float k_s = 0.4f;

        // Vector2 uv_ground_tl = kreuzung_orig + k_s * (Vector2.left + Vector2.up);
        // Vector2 uv_ground_tr = kreuzung_orig + k_s * (Vector2.right + Vector2.up);
        // Vector2 uv_ground_br = kreuzung_orig + k_s * (Vector2.right + Vector2.down);
        // Vector2 uv_ground_bl = kreuzung_orig + k_s * (Vector2.left + Vector2.down);


        // Vector2 kreuzung_ceil_orig = new Vector2(0.5f, 0.5f);

        // Vector3 uv_ceil_tl = kreuzung_ceil_orig + k_s * (Vector2.left + Vector2.up);
        // Vector2 uv_ceil_tr = kreuzung_ceil_orig + k_s * (Vector2.right + Vector2.up);
        // Vector2 uv_ceil_br = kreuzung_ceil_orig + k_s * (Vector2.right + Vector2.down);
        // Vector2 uv_ceil_bl = kreuzung_ceil_orig + k_s * (Vector2.left + Vector2.down);

        /*
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
*/
        //forward wall

        int ilevel =maxdepth-depth;
        baseIndex = vertices.Count;
        int baseIndex_sch = vertices_sch.Count;

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

        Vector3 t_c = (wall_l+wall_r+wall_l_ceil+wall_r_ceil)/4;
        Vector3 dx = (wall_r-wall_l).normalized;
        Vector3 dy = (wall_l_ceil-wall_l).normalized;
        var dz = Vector3.Cross(dx,dy);
        
        var relindex=index % TafelScript.imagekopien;
        var globalindex = TafelScript.imagekopien*ilevel+relindex;
    
        float portrait_wh  = portraitGroße*Tafel.TH/2;
        float portrait_hh  = portraitGroße*Tafel.TW/2;

        Vector3 t_lt = t_c-portrait_wh*dx-portrait_hh*dy-portraitRaum*dz;
        Vector3 t_rt = t_c+portrait_wh*dx-portrait_hh*dy-portraitRaum*dz;
        Vector3 t_lb = t_c-portrait_wh*dx+portrait_hh*dy-portraitRaum*dz;
        Vector3 t_rb = t_c+portrait_wh*dx+portrait_hh*dy-portraitRaum*dz;
              
        if(depth>0){                

  
        int steine = fehlendersteinZahl[ilevel];
       // steine=2;
        Vector3 lastpos = Vector3.one*(-666);

            while (steine>0){
                steine--;

                
                Vector3 s_bl =  t_lb-dz/10;
                Vector3 s_br = t_rb-dz/10;
                
                float steinGröße = (t_rt-t_lt).magnitude/Tafel.TH;
                
                Vector3 s_bl_f = t_lb-dz/6;
                Vector3 s_br_f = t_rb-dz/6;

                Vector3 s_o = s_bl + Vector3.up*0.01f;   

                Vector3 s_dx = s_br-s_bl;
                Vector3 s_dy = s_br_f-s_br;

                s_o = s_o+Random.Range(0.0f,1.0f)*s_dx+Random.Range(0.0f,1.0f)*s_dy;         
                s_o.y=0.01f;
                
                s_dx = s_dx.normalized;
                s_dy = s_dy.normalized;
                
                if (Vector3.Distance(lastpos,s_o)<steinGröße){
                    s_o=lastpos+s_dx*steinGröße;
                }

                Vector3 s_lt = s_o - s_dx*steinGröße/2 - s_dy*steinGröße/2;
                Vector3 s_rt = s_o + s_dx*steinGröße/2 - s_dy*steinGröße/2;
                Vector3 s_rb = s_o + s_dx*steinGröße/2 + s_dy*steinGröße/2;
                Vector3 s_lb = s_o - s_dx*steinGröße/2 + s_dy*steinGröße/2;

                int baseIndex_stein = vertices_steine.Count;

                vertices_steine.Add(s_lb);
                vertices_steine.Add(s_rb);
                vertices_steine.Add(s_rt);
                vertices_steine.Add(s_lt);

                tris_steine.Add(baseIndex_stein + 1);
                tris_steine.Add(baseIndex_stein + 0);
                tris_steine.Add(baseIndex_stein + 2);

                tris_steine.Add(baseIndex_stein + 2);
                tris_steine.Add(baseIndex_stein + 0);
                tris_steine.Add(baseIndex_stein + 3);

                lastpos=s_o;
            }


            vertices_sch.Add(t_lt);
            vertices_sch.Add(t_rt);
            vertices_sch.Add(t_rb);
            vertices_sch.Add(t_lb);


            //muss die größe den Tafeln ausrechnen
            float pixel_größe = 1.0f/TafelScript.imageKopieGröße;
            
            Vector2 d2x = Vector2.right*pixel_größe;
            Vector2 d2y = Vector2.up*pixel_größe;
            
            Vector2 ex = 0.1f*d2x;
            Vector2 ey = 0.1f*d2y;

            Vector2 o = Vector2.zero;        

        
            int curx = 0;
            int cury = 0;
            for (var i=0;i<globalindex;i++){ 
                curx += Tafel.TW;
                if (curx+Tafel.TW>=TafelScript.imageKopieGröße){
                    curx = 0;
                    cury += Tafel.TH;
                }            
            }
            o += curx*d2x+cury*d2y;

            uvs_sch.Add(o+d2x*Tafel.TW-ex+ey);
            uvs_sch.Add(o+d2x*Tafel.TW+d2y*Tafel.TH-ex-ey);
            uvs_sch.Add(o+d2y*Tafel.TH+ex-ey);
            uvs_sch.Add(o+ex+ey);

            normals_sch.Add(-forward);
            normals_sch.Add(-forward);
            normals_sch.Add(-forward);
            normals_sch.Add(-forward);

            tris_sch.Add(baseIndex_sch + 1);
            tris_sch.Add(baseIndex_sch + 0);
            tris_sch.Add(baseIndex_sch + 2);

            tris_sch.Add(baseIndex_sch + 2);
            tris_sch.Add(baseIndex_sch + 0);
            tris_sch.Add(baseIndex_sch + 3);

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
        
        if (depth==0) {
            Vector3 t_b = (wall_l+wall_r)/2-dz/6;
            if (allwrong){///actually all right
                urn.transform.position = t_b;
                urn.transform.forward = dz;
            } else if (allright){
                dong.transform.position = t_b;
                dong.transform.forward = dz;
            }
        }

        if (first){

            baseIndex = vertices.Count;

            Vector3 wallbase_lt = origin + (corridorwidth) * left + (length - corridorwidth) * forward;
            Vector3 wallbase_rt = origin + (corridorwidth) * right + (length - corridorwidth) * forward;
            Vector3 wallbase_rb = origin + (corridorwidth) * right - corridorwidth * forward;
            Vector3 wallbase_lb = origin + (corridorwidth) * left - corridorwidth * forward;

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
            int newstartindex = rowstartindex+IntPow(2,ilevel);            
            int leftpointindex =  newstartindex + 2*(index-rowstartindex);  
            bool left_is_correct =  lösung[globalindex]=='0';
            var allright_left = allright && left_is_correct;
            var allwrong_left = allwrong && (!left_is_correct);

            var allright_right = allright && (!left_is_correct);
            var allwrong_right = allwrong && (left_is_correct);

            GenMeshRecursive(vertices, normals, uvs, tris,vertices_sch,normals_sch,uvs_sch,tris_sch, vertices_steine, normals_steine, uvs_steine,  tris_steine, target, depth-1, left, false,maxdepth,newstartindex,leftpointindex, allright_left, allwrong_left);
            GenMeshRecursive(vertices, normals, uvs, tris,vertices_sch,normals_sch,uvs_sch,tris_sch, vertices_steine, normals_steine, uvs_steine,  tris_steine, target, depth-1, right, false,maxdepth,newstartindex,leftpointindex+1, allright_right, allwrong_right);
        }
    }

    void GenMesh(Vector3 origin, int depth, Vector3 forward)
    {
        lösung = lösungtext.text;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        
        List<Vector3> vertices_sch = new List<Vector3>();
        List<Vector3> normals_sch = new List<Vector3>();
        List<Vector2> uvs_sch = new List<Vector2>();
        List<int> tris_sch = new List<int>();

        List<Vector3> vertices_steine = new List<Vector3>();
        List<Vector3> normals_steine = new List<Vector3>();
        List<Vector2> uvs_steine = new List<Vector2>();
        List<int> tris_steine = new List<int>();

        GenMeshRecursive(vertices, normals, uvs,  tris, vertices_sch, normals_sch, uvs_sch,  tris_sch, vertices_steine, normals_steine, uvs_steine,  tris_steine, origin, depth, forward, true,depth,0,0,  true, true);

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        //mesh.uv = uvs.ToArray();
        //mesh.normals = normals.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();

        Mesh mesh_sch = new Mesh();
        mesh_sch.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh_sch.vertices = vertices_sch.ToArray();
        mesh_sch.uv = uvs_sch.ToArray();
        //mesh.normals = normals.ToArray();
        mesh_sch.triangles = tris_sch.ToArray();
        mesh_sch.RecalculateNormals();


        Mesh mesh_steine = new Mesh();
        mesh_steine.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh_steine.vertices = vertices_steine.ToArray();
        //mesh_steine.uv = uvs_sch.ToArray();
        //mesh.normals = normals.ToArray();
        mesh_steine.triangles = tris_steine.ToArray();
        mesh_steine.RecalculateNormals();

        //MeshUtility.Optimize(mesh);

#if UNITY_EDITOR
        MeshUtility.SetMeshCompression(mesh, ModelImporterMeshCompression.Low);
        AssetDatabase.CreateAsset(mesh, "Assets/CompressedMesh.asset");
        AssetDatabase.SaveAssets();
#endif
        wände.GetComponent<MeshFilter>().mesh = mesh;
        wände.GetComponent<MeshCollider>().sharedMesh = mesh;

        schilder.sharedMesh = mesh_sch;
        steine.sharedMesh = mesh_steine;
    }

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
