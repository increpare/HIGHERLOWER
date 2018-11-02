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


    public static bool Verbesser(bool[,] ar, int generate, ref int x, ref int y)
    {
        var replacementsmade = false;
        x = 0;
        y = 0;
        int ax = 0;
        int ay = 0;

        var orig_pr = Tafel.PrüfRichtung(ar, out x, out y);


        for (var i = 1; i < Tafel.TW - 5; i++)
        {
            for (var j = 1; j < Tafel.TH - 1; j++)
            {
                var v0 = ar[i + 0, j];
                var v1 = ar[i + 1, j];
                var v2 = ar[i + 2, j];
                var v3 = ar[i + 3, j];
                if ((v0 == !v1) && (v0 == !v2) && (v0 == v3))
                {
                    var r = rand.Next(0, 4);
                    ar[i + r, j] = !ar[i + r, j];
                    
                    var new_pr = (generate==2) 
                                    ?    Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) 
                                    :  (
                                            (generate==1)  
                                                ? Tafel.PrüfRichtung(ar, out ax, out ay)
                                                //else muss 3 sein
                                                : Tafel.PrüfRichtungMitHinzuhinzufügen(ar, out ax, out ay)
                                    );
                                    
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


        for (var i = 1; i < Tafel.TW - 1; i++)
        {
            for (var j = 1; j < Tafel.TH - 4; j++)
            {
                var v0 = ar[i, j];
                var v1 = ar[i, j + 1];
                var v2 = ar[i, j + 2];
                var v3 = ar[i, j + 3];
                if ((v0 == !v1) && (v0 == !v2) && (v0 == v3))
                {
                    var r = rand.Next(0, 4);
                    ar[i, j + r] = !ar[i, j + r];

                    var new_pr = (generate==2) 
                                    ?    Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) 
                                    :  (
                                            (generate==1)  
                                                ? Tafel.PrüfRichtung(ar, out ax, out ay)
                                                //else muss 3 sein
                                                : Tafel.PrüfRichtungMitHinzuhinzufügen(ar, out ax, out ay)
                                    );
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


        for (var i = 1; i < Tafel.TW - 6; i++)
        {
            for (var j = 1; j < Tafel.TH - 1; j++)
            {
                var v0 = ar[i + 0, j];
                var v1 = ar[i + 1, j];
                var v2 = ar[i + 2, j];
                var v3 = ar[i + 3, j];
                var v4 = ar[i + 4, j];
                if ((v0 == v1) && (v0 == v2) && (v0 == v3) && (v0 == v4))
                {
                    var r = rand.Next(0, 5);
                    ar[i + r, j] = !ar[i + r, j];

                    var new_pr = (generate==2) 
                                    ?    Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) 
                                    :  (
                                            (generate==1)  
                                                ? Tafel.PrüfRichtung(ar, out ax, out ay)
                                                //else muss 3 sein
                                                : Tafel.PrüfRichtungMitHinzuhinzufügen(ar, out ax, out ay)
                                    );
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

        for (var i = 1; i < Tafel.TW - 1; i++)
        {
            for (var j = 1; j < Tafel.TH - 6; j++)
            {
                var v0 = ar[i, j + 0];
                var v1 = ar[i, j + 1];
                var v2 = ar[i, j + 2];
                var v3 = ar[i, j + 3];
                var v4 = ar[i, j + 4];
                if ((v0 == v1) && (v0 == v2) && (v0 == v3) && (v0 == v4))
                {
                    var r = rand.Next(0, 5);
                    ar[i, j + r] = !ar[i, j + r];

                    var new_pr = (generate==2) 
                                    ?    Tafel.PrüfRichtungMitHinzufügen(ar, out ax, out ay) 
                                    :  (
                                            (generate==1)  
                                                ? Tafel.PrüfRichtung(ar, out ax, out ay)
                                                //else muss 3 sein
                                                : Tafel.PrüfRichtungMitHinzuhinzufügen(ar, out ax, out ay)
                                    );
                    
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

        return replacementsmade;
    }

    public static bool[,] GenerateWithDensity(int generate, float density, out int pr, out int x, out int y, bool verbesser)
    {
        int i = 0;
        while (true)
        {
            i++;
            if (i > 1000000)
            {
                break;
            }
            bool[,] ar = Tafel.Random(rand.Next(0, 1000000), density);

            pr = generate == 1 ? Tafel.PrüfRichtung(ar, out x, out y)
                                        : (generate == 2 ? Tafel.PrüfRichtungMitHinzufügen(ar, out x, out y)
                                           : Tafel.PrüfRichtungMitHinzuhinzufügen(ar, out x, out y));

            if (pr == 1 || pr == -1)
            {

                if (verbesser)
                {
                    int c = 0;
                    while (Verbesser(ar, generate, ref x, ref y) && c < 100)
                    {
                        c++;
                    }
                }

                return ar;
            }
        }

        x = 0;
        y = 0;
        pr = -2;
        return null;
    }

    public static bool[,] mitNiveau(int niveau, out int d, out int x, out int y)
    {
        switch (niveau)
        {
            case 1:
                return Tafel.GenerateMaze(0, 0, false, 0, out d, out x, out y);
            case 2:
                var ar = Tafel.GenerateManyCrosses();
                d = Tafel.PrüfRichtung(ar, out x, out y);
                return ar;
            case 3:
                return GenerateWithDensity(1, 0.1f, out d, out x, out y, false);
            case 4:
                return Tafel.GenerateWithDensity(1, 0.5f, out d, out x, out y, false);
            case 5:
                return Tafel.GenerateWithDensity(1, 0.5f, out d, out x, out y, false);
            case 6:
                return Tafel.GenerateWithDensity(1, 0.5f, out d, out x, out y, true);
            case 7:
                return Tafel.GenerateWithDensity(1, 0.9f, out d, out x, out y, true);
            case 8:
                return Tafel.GenerateWithDensity(2, 0.1f, out d, out x, out y, false);
            case 9:
                return Tafel.GenerateWithDensity(2, 0.9f, out d, out x, out y, false);
            case 10:
                return Tafel.GenerateWithDensity(2, 0.5f, out d, out x, out y, false);
            case 11:
                return Tafel.GenerateWithDensity(3, 0.1f, out d, out x, out y, false);
            case 12:
                return Tafel.GenerateWithDensity(3, 0.9f, out d, out x, out y, false);
            case 13:
                return Tafel.GenerateWithDensity(3, 0.5f, out d, out x, out y, false);
            case 14:
                return Tafel.GenerateWithDensity(3, 0.5f, out d, out x, out y, false);
            case 15:
                return Tafel.GenerateWithDensity(3, 0.5f, out d, out x, out y, false);
            default:
                x = 0;
                y = 0;
                d = -2;
                return null;
        }
    }

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

                for (var i2 = i; i2 < TW - 1; i2++)
                {
                    for (var j2 = j+1; j2 < TH - 1; j2++)
                    {
                        if (ar[i2, j2] == true)
                        {
                            continue;
                        }
                        ar[i2, j2] = true;



                        {
                            //must not be solvable with this alone
                            ar[i, j] = false;

                            int x_this2, y_this2;
                            int r_this2 = PrüfRichtung(ar, out x_this2, out y_this2);
                            if (r_this2==1 || r_this2==-1){
                                r_kollektiv = -2;
                                ar[i2,j2] = false;
                                continue;
                            }
                            
                            ar[i, j] = true;

                        }

                        int x_this, y_this;
                        int r_this = PrüfRichtung(ar, out x_this, out y_this);

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


    public static string ToString(bool[,] ar){
        var s = "";
        
        for (int j = 0; j < Tafel.TH; j++)
        {        
            for (int i = 0; i < Tafel.TW; i++)
            {
                if (ar[i,j]){
                    s+="X";
                } else {
                    s+=".";
                }
            }
            s+="\n";
        }
        return s.Trim();
    }

    public static bool[,] FromString(string s)
    {
        var lines = s.Trim().Split('\n');
        var ar = new bool[TW, TH];
        for (int i = 0; i < Tafel.TW; i++)
        {
            for (int j = 0; j < Tafel.TH; j++)
            {        
                var c = lines[j][i];
                if (c != '.')
                {
                    ar[i, j] = true;
                }
            }
        };
        return ar;
    }

    private static System.Random rand = new System.Random();

    static bool[,] placeRandomCross(bool inverse, out int d, out int x, out int y)
    {
        d = rand.Next(0, 2) * 2 - 1;
        var o = d == -1 ? 1 : 0;

        if (inverse)
        {
            var a = Tafel.Random(0, 1);
            x = rand.Next(2, TW - 4);
            y = rand.Next(2, TH - 5);
            a[x + 1, y + 0] = false;
            a[x + 0, y + 1 + o] = false;
            a[x + 1, y + 1] = false;
            a[x + 2, y + 1 + o] = false;
            a[x + 1, y + 2] = false;
            a[x + 1, y + 3] = false;
            return a;
        }
        else
        {
            var a = Tafel.Random(0, 0);
            x = rand.Next(1, TW - 3);
            y = rand.Next(1, TH - 4);
            a[x + 1, y + 0] = true;
            a[x + 0, y + 1+o] = true;
            a[x + 1, y + 1] = true;
            a[x + 2, y + 1+o] = true;
            a[x + 1, y + 2] = true;
            a[x + 1, y + 3] = true;
            return a;
        }
    }

    public static bool[,] GenerateManyCrosses(){

        int d = rand.Next(0, 2) * 2 - 1;
        var o = d == -1 ? 1 : 0;

        // int minx = 1;
        // int miny = 1;

        // int maxx = TW-2;
        // int maxy = TW-2;

        // int w = (maxx - minx) / 2-1;
        // int h = (maxy - miny) / 2-1;

        // var dx = w + 1;
        // var dy = h + 1;

        var num = rand.Next(4, 7);

        while (true)
        {
            var a = Tafel.Random(0, 0);
            bool valid = true;
            for (int i = 0; i < num; i++)
            {
                int x = rand.Next(1, TW - 3);
                int y = rand.Next(1, TH - 4);

                if (a[x + 1, y - 1]                 ||
                    a[x, y] || a[x + 2, y]          ||
                    a[x - 1, y + 1 + o] || a[x + 3, y + 1 + o]   ||
                    a[x    , y + 2 - o] || a[x + 2, y + 2 - o]   ||
                    a[x, y+3] || a[x + 2, y+3] ||
                    a[x+1,y+4])
                {
                    valid = false;
                    break;
                }
                a[x + 1, y + 0] = true;
                a[x + 0, y + 1 + o] = true;
                a[x + 1, y + 1] = true;
                a[x + 2, y + 1 + o] = true;
                a[x + 1, y + 2] = true;
                a[x + 1, y + 3] = true;
            }

            if (valid)
            {
                return a;
            }
        }


    }

    public static bool[,] GenerateMaze(
        float dichte,
        int invers,//0 white, 1 invert, 2 mixed
        bool bgtidy,
        int fehlendemosaike,
        out int d,
        out int x,
        out int y)
    {

        if (dichte == 0)
        {
            var _invers = invers > 0;
            if (invers==2){
                _invers = rand.Next(0, 2) == 0;
            }
            var ar = placeRandomCross(_invers, out d, out x, out y);
            x--;
            y--;
            return ar;
        }

        d = 0;
        x = 0;
        y = 0;
        return null;
    }


    public Tafel()
    {
    }
}

