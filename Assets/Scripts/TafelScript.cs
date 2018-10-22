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
        tafel.Apply();
        //StartCoroutine(Gen());
        var ar = Tafel.FromString(testString);
        Debug.Log(ar);
        ZeigTafel(ar);
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
    void ZeigTafel(bool[,] ar,int dir=-2, int x=-1, int y=-1)
    {
        for (var i = 1; i < Tafel.TW - 1; i++)
        {
            for (var j = 1; j < Tafel.TH - 1; j++)
            {
                var px = i - 1;
                var py = j - 1;
                py = Tafel.TH - 3 - py;
                if (ar[i, j])
                {
                    tafel.SetPixel(px,py, col);
                }
                else
                {
                    tafel.SetPixel(px,py, Color.clear);
                }
            }
        }
        tafel.Apply();
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.N)){
            int i = 0;
            while (true){
                i++;
                if (i>1000000){
                    Debug.Log(":(");
                    break;
                }
                float density = UnityEngine.Random.Range(0.1f, 0.9f);
                bool[,] ar = Tafel.Random(UnityEngine.Random.Range(0,1000000), density);
                int x, y;
           
                var pr = Tafel.PrüfRichtungMitHinzufügen(ar, out x, out y);
                if (pr == 1 || pr == -1)
                {
                    if (UnityEngine.Random.Range(0, 2) == 1)
                    {
                        if (x>1 && x<(Tafel.TW-Tafel.CW-1) && y > 1 && y < (Tafel.TH - Tafel.CH - 1)){
                            Tafel.invert(ar);
                            Debug.Log("invert");
                        } else {
                            continue;
                        }
                    }
                    Debug.Log(pr+"\t("+x+","+ y+")");
                    ZeigTafel(ar, pr, x, y);
                    break;
                }

            }
        }
	}
}
