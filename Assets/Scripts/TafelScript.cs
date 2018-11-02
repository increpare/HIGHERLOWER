using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class TafelScript : MonoBehaviour {



    public Texture2D tafel;
   

[Multiline]
    public String startstring="";

    // Use this for initialization
    void Start () {
        //tafel.Resize(Tafel.CW, Tafel.CH, TextureFormat.RGBA32, false);

        Color32 resetColor = new Color32(255, 255, 255, 0);
        Color32[] resetColorArray = tafel.GetPixels32();

        for (int i = 0; i < resetColorArray.Length; i++)
        {
            resetColorArray[i] = resetColor;
        }

        tafel.SetPixels32(resetColorArray);


        tafel.Apply();

    }

    //IEnumerator Gen(){
    //    _ar = null;
    //    for (var i = 0; i < 1000; i++)
    //    {
    //        float density = Random.Range(0.1f, 0.5f);
    //        ar = Tafel.Random();
    //        int x, y;
    //        var pr = Tafel.PrüfRichtung(ar,out x, out y);
    //        Debug.Log(pr);
    //        if (pr == 1)
    //        {
    //            ZeigTafel(_ar);
    //            yield return new WaitForSeconds(5.0f);
    //        }
    //    }
    //    yield return 0;
    //}

    /*
     * 1 upwards
     * 0 no direction
     * -1 downwards
     * -2 fehler
     */


    public Color col;
    public Color col_bg;
    public Color hinweis_voll;
    public Color hinweis_leer;

    public Texture2D tex;
    public int[] tex_ergebnisse;
    public bool regentex;

    void ZeigTafel(bool[,] ar,int dir=-2, int x=-1, int y=-1)
    {
        x++;
        y++;
        
        var hint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        for (var i = 0; i < Tafel.TW ; i++)
        {
            for (var j = 0; j < Tafel.TH ; j++)
            {

                bool Kreuzpunkt = (i == x + 1) && (j == y || j == y + 1 || j == y + 2 || j == y + 3);
                if (dir == 0 || dir == -2)
                {
                    Kreuzpunkt = false;
                }
                else if (dir == 1)
                {
                    Kreuzpunkt = Kreuzpunkt || (i == x && j == y + 1) || (i == x + 2 && j == y + 1);
                }
                else if (dir == -1)
                {
                    Kreuzpunkt = Kreuzpunkt || (i == x && j == y + 2) || (i == x + 2 && j == y + 2);
                }

                var px = i ;
                var py = j ;
                py = Tafel.TH - 1 - py;
                if (ar[i, j])
                {
                    if (hint && Kreuzpunkt)
                    {
                        tafel.SetPixel(px, py, hinweis_voll);
                    }
                    else
                    {
                        tafel.SetPixel(px, py, col);
                    }
                }
                else
                {
                    if (hint && Kreuzpunkt)
                    {
                        tafel.SetPixel(px, py, hinweis_leer);
                    }
                    else
                    {
                        tafel.SetPixel(px, py, Color.clear);
                    }
                }


            }
        }
        tafel.Apply();
    }


    public float density = 0.5f;





    public bool l1 = false;
    public bool l2 = false;
    public bool l3 = false;
    public bool l4 = false;
    public bool l5 = false;
    public bool l6 = false;
    public bool l7 = false;
    public bool l8 = false;
    public bool l9 = false;
    public bool l10 = false;
    public bool l11 = false;
    public bool l12 = false;
    public bool l13 = false;
    public bool l14 = false;
    public bool l15 = false;

    public static int imageKopieGröße = 512;

    public static int imagekopien = 10;//war 38

    IEnumerator GenTex(){
        tex = new Texture2D(imageKopieGröße,imageKopieGröße);
        tex_ergebnisse = new int[15 * imagekopien];

        int curx = 0;
        int cury = 0;
        for (var i = 1;i <= 15;i++){
            for (var kopie = 0; kopie < imagekopien;kopie++){
                Debug.Log(i+"_"+(kopie+1));
                int x,y,d;
                var tafel = Tafel.mitNiveau(i,out d, out x, out y);
                int tei = (i-1) * imagekopien + kopie;
               // Debug.Log("te "+tei+" = " + d);
                tex_ergebnisse[tei] = d;

                for (int ix = 0; ix < Tafel.TW; ix++)
                {
                    for (int iy = 0; iy < Tafel.TH; iy++)
                    {
                        if (tafel[ix, iy])
                        {
                            tex.SetPixel(curx + ix, cury + iy, col);
                        } else
                        {
                            tex.SetPixel(curx + ix, cury + iy, col_bg);
                        }
                    }
                }

                curx += Tafel.TW;
                if (curx+Tafel.TW>=imageKopieGröße){
                    curx = 0;
                    cury += Tafel.TH;
                }
                yield return 0;
            }
        }
        tex.Apply();


        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes("Assets/Resources/SavedScreen.png", bytes);

        string s = "";
        for (var i = 0; i < tex_ergebnisse.Length;i++){
            if (tex_ergebnisse[i]==-1){
                s += "0";
            } else {
                s += "1";
            }
        }
        File.WriteAllText("Assets/Resources/SavedScreenErgebnisse.txt", s);
        yield break;
    }

    private bool[,] _ar=null;
    private int _d;
    private int _x;
    private int _y;

    // Update is called once per frame
    void Update ()
    {

        if (startstring!=""){
            _ar = Tafel.FromString(startstring);
            startstring="";
        }

        if (regentex){
            regentex = false;
            StartCoroutine("GenTex");
        }
        if (l1)
        {
            l1 = false;
            
            _ar = Tafel.mitNiveau(1,out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l2)
        {
            l2 = false;
            
            _ar = Tafel.mitNiveau(2, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l3)
        {
            l3 = false;
            
            _ar = Tafel.mitNiveau(3, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l4)
        {
            l4 = false;
            
            _ar = Tafel.mitNiveau(4, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l5)
        {
            l5 = false;
            
            _ar = Tafel.mitNiveau(5, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l6)
        {
            l6 = false;
            
            _ar = Tafel.mitNiveau(6, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l7)
        {
            l7 = false;
            
            _ar = Tafel.mitNiveau(7, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l8)
        {
            l8 = false;
            
            _ar = Tafel.mitNiveau(8, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l9)
        {
            l9 = false;
            
            _ar = Tafel.mitNiveau(9, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l10)
        {
            l10 = false;
            
            _ar = Tafel.mitNiveau(10, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l11)
        {
            l11 = false;
            
            _ar = Tafel.mitNiveau(11, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l12)
        {
            l12 = false;
            
            _ar = Tafel.mitNiveau(12, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l13)
        {
            l13 = false;
            
            _ar = Tafel.mitNiveau(13, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
            
        }
        if (l14)
        {
            l14 = false;
            
            _ar = Tafel.mitNiveau(14, out _d, out _x, out _y);
            Debug.Log(Tafel.ToString(_ar));
        }
        if (l15)
        {
            l15 = false;
            
            _ar = Tafel.mitNiveau(15, out _d, out _x, out _y);   
            Debug.Log(Tafel.ToString(_ar));         
        }

        if (Input.GetKeyDown(KeyCode.P) && _ar!=null){
            _d = Tafel.PrüfRichtungMitHinzuhinzufügen(_ar, out _x, out _y);
            Debug.Log("PrüfRichtungMitHinzuhinzufügen "+_d);
        }

        var generate = -1;
        if (Input.GetKeyDown(KeyCode.N)){
            generate = 1;
        } else if (Input.GetKeyDown(KeyCode.M)){
            generate = 2;
        } else if (Input.GetKeyDown(KeyCode.Comma)){
            generate = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            density = 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            density = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            density = 0.3f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            density = 0.4f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            density = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            density = 0.6f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            density = 0.7f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            density = 0.8f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            density = 0.9f;
        }

        if (generate>0){
            
            Tafel.GenerateWithDensity(generate,density,out _d, out _x, out _y,false);
        }
        if (_ar!=null){
            ZeigTafel(_ar,_d,_x,_y);
        }
	}
}
