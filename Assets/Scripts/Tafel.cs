using System;
using System.Collections;
using System.Collections.Generic;

public class IntPair {
    public int x;
    public int y;
    public IntPair(int x, int y){
        this.x = x;
        this.y = y;
    }
}

public class Tafel
{
    public const int TW = 23;
    public const int TH = 19;

    public const int CW = 5;
    public const int CH = 6;

    public static bool[,] Random(int seed, float density){
        var tafel = new bool[TW, TH];
        var r = new System.Random(seed);


        for (var i = 1; i < TW - 1; i++)
        {
            for (var j = 1; j < TH - 1; j++)
            {
                if ((float)r.NextDouble() < density)
                {
                    tafel[i, j] = true;
                }
            }
        }

        //tafel[3, 5] = true;
        //tafel[3, 6] = true;
        //tafel[2, 6] = true;
        //tafel[4, 6] = true;
        //tafel[3, 7] = true;
        //tafel[3, 8] = true;
        return tafel;
    }

    public static int PrüfRichtung(bool[,] tafel, out int x, out int y)
    {
        x = 0;
        y = 0;
        int richtung = 0;
        for (var i = 0; i < TW - CW; i++)
        {
            for (var j = 0; j < TH - CH; j++)
            {
                int dirfound = 0;

                if (tafel[i + 2, j + 1] && tafel[i + 2, j + 2] && tafel[i + 2, j + 3] && tafel[i + 2, j + 4]
                    && !tafel[i + 2, j + 0]
                    && !tafel[i + 1, j + 1] && !tafel[i + 3, j + 1]
                    && !tafel[i + 1, j + 4] && !tafel[i + 3, j + 4]
                    && !tafel[i + 2, j + 5]
                   )
                {
                    if (
                        tafel[i + 1, j + 2] && tafel[i + 3, j + 2]
                        && !tafel[i + 0, j + 2] && !tafel[i + 4, j + 2]
                        && !tafel[i + 1, j + 3] && !tafel[i + 3, j + 3]
                    )
                    {
                        //nordlicher Pfeil
                        dirfound = 1;
                    }
                    else if (
                      tafel[i + 1, j + 3] && tafel[i + 3, j + 3]
                      && !tafel[i + 1, j + 2] && !tafel[i + 3, j + 2]
                      && !tafel[i + 0, j + 3] && !tafel[i + 4, j + 3]
                    )
                    {
                        dirfound = -1;

                    }
                }

                if (dirfound==1){
                    if (richtung == 0)
                    {
                        richtung = 1;
                        x = i;
                        y = j;
                    }
                    else if (richtung == -1)
                    {
                        return -2;
                    }
                } else if (dirfound==-1){
                    //südlicher Pfeil
                    if (richtung == 0)
                    {
                        richtung = -1;
                        x = i;
                        y = j;
                    }
                    else if (richtung == 1)
                    {
                        return -2;
                    }
                }
            }
        }


        return richtung;
    }


    public static void invert(bool[,] tafel)
    {

        for (var i = 0; i < TW; i++)
        {
            for (var j = 0; j < TH; j++)
            {
                tafel[i, j] = !tafel[i, j];
            }
        }
    }

    public static bool[,] FromString(string s)
    {
        var lines = s.Trim().Split('\n');
        var ar = new bool[TW, TH];
        Console.WriteLine("AR " + lines[0].Length + "," + lines.Length);
        for (int i = 1; i < Tafel.TW - 1; i++)
        {
            for (int j = 1; j < Tafel.TH - 1; j++)
            {
                Console.WriteLine("C " + i + "," + j);

                var c = lines[j - 1][i - 1];
                if (c != '.')
                {
                    ar[i, j] = true;
                }
            }
        };
        return ar;
    }

    public Tafel()
    {
    }
}

