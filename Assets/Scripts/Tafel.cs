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
    public const int TW = 19;
    public const int TH = 23;

    //public const int TW = 15;
    //public const int TH = 10;


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


    /*
     * +1 - einheitlich nördlich
     * +0 - unentscheiden
     * -1 - einheitlich südlich
     * -2 - widersprüchlich
     */
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
                } else if (!tafel[i + 2, j + 1] && !tafel[i + 2, j + 2] && !tafel[i + 2, j + 3] && !tafel[i + 2, j + 4]
                                    && tafel[i + 2, j + 0]
                                    && tafel[i + 1, j + 1] && tafel[i + 3, j + 1]
                                    && tafel[i + 1, j + 4] && tafel[i + 3, j + 4]
                                    && tafel[i + 2, j + 5]
                                   )
                {
                    if (
                        !tafel[i + 1, j + 2] && !tafel[i + 3, j + 2]
                        && tafel[i + 0, j + 2] && tafel[i + 4, j + 2]
                        && tafel[i + 1, j + 3] && tafel[i + 3, j + 3]
                    )
                    {
                        //nordlicher Pfeil
                        dirfound = 1;
                    }
                    else if (
                      !tafel[i + 1, j + 3] && !tafel[i + 3, j + 3]
                      && tafel[i + 1, j + 2] && tafel[i + 3, j + 2]
                      && tafel[i + 0, j + 3] && tafel[i + 4, j + 3]
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

    public static int PrüfRichtungMitHinzufügen(bool[,] ar, out int x, out int y)
    {
        x = 0;
        y = 0;
        int x_orig, y_orig;
        var r_orig = PrüfRichtung(ar, out x_orig, out y_orig);
        //herausfiltern einfache lösungen;;
        if (r_orig==1 || r_orig==-1)
        {
            return -2;
        }
        int x_kollektiv = 0;
        int y_kollektiv = 0;
        int r_kollektiv = 0;

        for (var i = 1; i < TW - 1; i++)
        {
            for (var j = 1; j < TH - 1; j++)
            {
                if (ar[i,j]==true){
                    continue;
                }
                ar[i, j] = true;
                int x_this, y_this;

                int r_this = PrüfRichtung(ar, out x_this, out y_this);
                if (r_this==-2)
                {
                    ar[i, j] = false;
                    continue;
                }

                if (r_this==0){
                    //all ok
                } else if (r_this==r_kollektiv){
                    //all ok
                } else if (r_kollektiv==0)
                {
                    r_kollektiv = r_this;
                    x_kollektiv = x_this;
                    y_kollektiv = y_this;
                }
                else {
                    if (r_this==0){
                        //ok
                    } else
                    {
                        r_kollektiv = -2;
                    }
                }
                ar[i, j] = false;
            }
        }

        if (r_kollektiv==r_orig){
            return r_orig;
        }
        if ( (r_orig==-2 || r_orig==0) && (r_kollektiv==1 || r_kollektiv==-1)){
            x = x_kollektiv;
            y = y_kollektiv;
            return r_kollektiv;
        }

        return -2;
    }


    public static int PrüfRichtungMitHinzuhinzufügen(bool[,] ar, out int x, out int y)
    {
        x = 0;
        y = 0;
        int x_orig, y_orig;
        var r_orig = PrüfRichtung(ar, out x_orig, out y_orig);
        //herausfiltern einfache lösungen;;
        if (r_orig == 1 || r_orig == -1)
        {
            return -2;
        }
        int x_kollektiv = 0;
        int y_kollektiv = 0;
        int r_kollektiv = 0;

        for (var i = 1; i < TW - 1; i++)
        {
            for (var j = 1; j < TH - 1; j++)
            {
                if (ar[i, j] == true)
                {
                    continue;
                }
                ar[i, j] = true;
                //must not be solvable with 1
                int bla, bla2;
                int r_this_vor = PrüfRichtung(ar, out bla, out bla2);
                if (r_this_vor == 1 || r_this_vor==-1){
                    ar[i, j] = false;
                    continue;
                }

                for (var i2 = i+1; i2 < TW - 1; i2++)
                {
                    for (var j2 = j+1; j2 < TH - 1; j2++)
                    {
                        if (ar[i2, j2] == true)
                        {
                            continue;
                        }
                        ar[i2, j2] = true;

                        int x_this, y_this;

                        int r_this = PrüfRichtung(ar, out x_this, out y_this);

                        //must not be solvable with this alone
                        ar[i, j] = false;

                        int x_this2, y_this2;
                        int r_this2 = PrüfRichtung(ar, out x_this2, out y_this2);
                        if (r_this2==1 || r_this2==-1){
                            r_this = -2;
                        }

                        ar[i, j] = true;

                        if (r_this == -2)
                        {
                            ar[i2, j2] = false;
                            continue;
                        }

                        if (r_this == 0)
                        {
                            //all ok
                        }
                        else if (r_this == r_kollektiv)
                        {
                            //all ok
                        }
                        else if (r_kollektiv == 0)
                        {
                            r_kollektiv = r_this;
                            x_kollektiv = x_this;
                            y_kollektiv = y_this;
                        }
                        else
                        {
                            if (r_this == 0)
                            {
                                //ok
                            }
                            else
                            {
                                r_kollektiv = -2;
                            }
                        }
                        ar[i2, j2] = false;
                    }
                }
                ar[i, j] = false;
            }
        }

        if (r_kollektiv == r_orig)
        {
            return r_orig;
        }
        if ((r_orig == -2 || r_orig == 0) && (r_kollektiv == 1 || r_kollektiv == -1))
        {
            x = x_kollektiv;
            y = y_kollektiv;
            return r_kollektiv;
        }

        return -2;
    }

    public static void invert(bool[,] tafel)
    {

        for (var i = 1; i < TW-1; i++)
        {
            for (var j = 1; j < TH-1; j++)
            {
                tafel[i, j] = !tafel[i, j];
            }
        }
    }

    public static bool[,] FromString(string s)
    {
        var lines = s.Trim().Split('\n');
        var ar = new bool[TW, TH];
        for (int i = 1; i < Tafel.TW - 1; i++)
        {
            for (int j = 1; j < Tafel.TH - 1; j++)
            {        
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

