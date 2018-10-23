using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TafelScript : MonoBehaviour {



    public Texture2D tafel;
   
    private static string testString =
@"..O.....X.....O......
 .........O...........
.....O...X...........
..O..........O......O
..O........OO....O.O.
O......O........O...O
...O....O....OOO.....
....O...O.........O..
.O.......O.....O.....
.............O.......
O....................
...O.....O.O.........
......O.........O....
..............O.O....
O...O................
O.......O....O..O....
.................O...";

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
        //StartCoroutine(Gen());
        //var ar = Tafel.FromString(testString);
        //Debug.Log(ar);
        //ZeigTafel(ar);
	}

    //IEnumerator Gen(){
    //    bool[,] ar = null;
    //    for (var i = 0; i < 1000; i++)
    //    {
    //        float density = Random.Range(0.1f, 0.5f);
    //        ar = Tafel.Random();
    //        int x, y;
    //        var pr = Tafel.PrüfRichtung(ar,out x, out y);
    //        Debug.Log(pr);
    //        if (pr == 1)
    //        {
    //            ZeigTafel(ar);
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
    public Color hinweis_voll;
    public Color hinweis_leer;

    void ZeigTafel(bool[,] ar,int dir=-2, int x=-1, int y=-1)
    {
        x++;
        y++;


        Color32 resetColor = new Color32(255, 255, 255, 0);

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

    private bool[,] _ar=null;
    private int _ax;
    private int _ay;
    private int _apr;

    public float density = 0.5f;


    bool Verbesser(bool[,] ar, bool fancy, ref int x, ref int y)
    {
        var replacementsmade = false;
        x = 0;
        y = 0;
        int ax=0;
        int ay=0;

        var orig_pr = Tafel.PrüfRichtung(ar, out x, out y);


        for (var i = 1; i < Tafel.TW - 5; i++)
        {
            for (var j = 1; j < Tafel.TH-1; j++)
            {
                var v0 = ar[i + 0, j];
                var v1 = ar[i + 1, j];
                var v2 = ar[i + 2, j];
                var v3 = ar[i + 3, j];
                if ((v0 == !v1) && (v0 == !v2) && (v0 == v3))
                {
                    var r = UnityEngine.Random.Range(0, 4);
                    ar[i + r, j] = !ar[i + r, j];
                    var new_pr = fancy ? Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) : Tafel.PrüfRichtung(ar, out ax, out ay);
                    if (new_pr != orig_pr)
                    {
                        ar[i + r, j] = !ar[i + r, j];
                    }
                    else
                    {
                        x = ax;
                        y = ay;
                        replacementsmade = true;
                    }
                }
            }
        }


        for (var i = 1; i < Tafel.TW-1; i++)
        {
            for (var j = 1; j < Tafel.TH-4; j++)
            {
                var v0 = ar[i, j];
                var v1 = ar[i, j+1];
                var v2 = ar[i, j+2];
                var v3 = ar[i, j+3];
                if ((v0 == !v1) && (v0 == !v2) && (v0 == v3))
                {
                    var r = UnityEngine.Random.Range(0, 4);
                    ar[i, j+r] = !ar[i, j+r];
                    var new_pr = fancy ? Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) : Tafel.PrüfRichtung(ar, out ax, out ay);
                    if (new_pr != orig_pr)
                    {
                        ar[i, j + r] = !ar[i, j + r];
                    }
                    else
                    {
                        x = ax;
                        y = ay;
                        replacementsmade = true;
                    }
                }
            }
        }


        for (var i = 1; i < Tafel.TW-6; i++)
        {
            for (var j = 1; j < Tafel.TH-1; j++)
            {
                var v0 = ar[i+0, j];
                var v1 = ar[i+1, j];
                var v2 = ar[i+2, j];
                var v3 = ar[i+3, j];
                var v4 = ar[i+4, j];
                if ( (v0 == v1) && (v0 == v2) && (v0 == v3) && (v0 == v4) ){
                    var r = UnityEngine.Random.Range(0, 5);
                    ar[i + r, j] = !ar[i + r, j];
                    var new_pr = fancy ? Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) : Tafel.PrüfRichtung(ar, out ax, out ay);
                    if (new_pr != orig_pr) {
                        ar[i + r, j] = !ar[i + r, j];
                    }
                    else
                    {
                        x = ax;
                        y = ay;
                        replacementsmade = true;
                    }
                }
            }
        }

        for (var i = 1; i < Tafel.TW-1; i++)
        {
            for (var j = 1; j < Tafel.TH-6; j++)
            {
                var v0 = ar[i, j+0];
                var v1 = ar[i, j+1];
                var v2 = ar[i, j+2];
                var v3 = ar[i, j+3];
                var v4 = ar[i, j+4];
                if ((v0 == v1) && (v0 == v2) && (v0 == v3) && (v0 == v4))
                {
                    var r = UnityEngine.Random.Range(0, 5);
                    ar[i, j+r] = !ar[i, j+r];
                    var new_pr = fancy ? Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) : Tafel.PrüfRichtung(ar, out ax, out ay);
                    if (new_pr != orig_pr)
                    {
                        ar[i, j+r] = !ar[i, j+r];
                    } else
                    {
                        x = ax;
                        y = ay;
                        replacementsmade = true;
                    }
                }
            }
        }

        return replacementsmade;
    }


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



    // Update is called once per frame
    void Update ()
    {

        if (l1)
        {
            l1 = false;
        }
        if (l2)
        {
            l2 = false;
        }
        if (l3)
        {
            l3 = false;
        }
        if (l4)
        {
            l4 = false;
        }
        if (l5)
        {
            l5 = false;
        }
        if (l6)
        {
            l6 = false;
        }
        if (l7)
        {
            l7 = false;
        }
        if (l8)
        {
            l8 = false;
        }
        if (l9)
        {
            l9 = false;
        }
        if (l10)
        {
            l10 = false;
        }
        if (l11)
        {
            l11 = false;
        }
        if (l12)
        {
            l12 = false;
        }
        if (l13)
        {
            l13 = false;
        }
        if (l14)
        {
            l14 = false;
        }
        if (l15)
        {
            l15 = false;
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
            int i = 0;
            while (true){
                i++;
                if (i>1000000){
                    Debug.Log(":(");
                    break;
                }
                bool[,] ar = Tafel.Random(UnityEngine.Random.Range(0,1000000), density);
                int x, y;
           
                var pr = generate==1 ? Tafel.PrüfRichtung(ar, out x, out y)
                                            : ( generate==2?Tafel.PrüfRichtungMitHinzufügen(ar, out x, out y)
                                               : Tafel.PrüfRichtungMitHinzuhinzufügen(ar,out x, out y));

                if (pr == 1 || pr == -1)
                {
                    Debug.Log(pr+"\t("+x+","+ y+")");
                    _ar = ar;
                    _ax = x;
                    _ay = y;
                    _apr = pr;
                    break;
                }

            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                int c = 0;
                while (Verbesser(_ar, generate == 2, ref _ax, ref _ay) && c < 1)
                {
                    c++;
                }
            }
        }


        if (_ar != null)
        {

            ZeigTafel(_ar, _apr, _ax, _ay);
        }
	}
}
