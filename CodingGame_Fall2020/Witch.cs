using System;
using System.Collections.Generic;
using System.Text;

class Witch
{
    public int[] inventory = new int[4];
    public int[] neededInv = new int[4];

    public Witch(int cD0, int cD1, int cD2, int cD3)
    {
        inventory[0] = cD0;
        inventory[1] = cD1;
        inventory[2] = cD2;
        inventory[3] = cD3;
    }

    public Witch()
    {
        //This should never be used on it's own
    }
}

