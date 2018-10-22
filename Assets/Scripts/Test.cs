using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Hello 
{
    static void ZeigTafel(bool[,] tafel,int dir, int x, int y){
        var s = dir+"\t("+x+","+y+")\n";
        x++;
        y++;
        for (var j = 1; j < Tafel.TH - 1;j++){
            for (var i = 1; i < Tafel.TW - 1; i++)
            {

                bool Kreuzpunkt = (i == x + 1) && (j == y || j == y + 1 || j == y + 2 || j == y + 3);
                if (dir==0||dir==-2){
                    Kreuzpunkt = false;
                } else if (dir==1){
                    Kreuzpunkt = Kreuzpunkt || (i==x && j==y+1) || (i == x+2 && j == y + 1);
                } else if (dir==-1)
                {
                    Kreuzpunkt = Kreuzpunkt || (i == x && j == y + 2) || (i == x + 2 && j == y + 2);
                }

                if (tafel[i,j]){
                    if (Kreuzpunkt)
                    {
                        s += "X";
                    } else {
                        s += "O";
                    }
                } else
                {
                    if (Kreuzpunkt)
                    {
                        s += "#";
                    }
                    else
                    {
                        s += ".";
                    }
                }
            }
            s += "\n";
        }
        Console.Write("\n\n"+s);
    }

    private static string testString = @"..O.....X.....O......
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

    static void TryTest(){
        Console.WriteLine(testString);
        var ar = Tafel.FromString(testString);
        int x, y;
        var pr = Tafel.PrüfRichtungMitHinzufügen(ar, out x, out y);
        if (pr == 1 || pr == -1)
        {
            ZeigTafel(ar, pr, x, y);
        }
    }
    static void Main() 
    {
       // TryTest();
       // return;

        var r = new Random();
        int imax = 1000000;
        Parallel.For(1, 1000000, i =>
        {
            var density = 0.1f + 0.8f * (float)i/(float)imax;
            bool[,] ar = Tafel.Random(r.Next(),density);
            int x, y;
            var pr = Tafel.PrüfRichtung(ar, out x, out y);
            if (pr == 1 || pr == -1)
            {
                ZeigTafel(ar,pr,x,y);
            }
        });
    }
}
