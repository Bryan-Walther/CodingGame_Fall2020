using System;
using System.Collections.Generic;
using System.Text;


class Spell
{
    public int ID;
    public int[] delta = new int[4];
    public bool castable;
    public bool doable;

    public int[] cost()
    {
        int[] price = new int[4];
        for (int i = 0; i < 4; i++)
        {

            if (delta[i] < 0)
                price[i] = delta[i];
            else
                price[i] = 0;
        }
        return price;
    }
    public int[] gain()
    {
        int[] profit = new int[4];
        for (int i = 0; i < 4; i++)
        {

            if (delta[i] > 0)
                profit[i] = delta[i];
            else
                profit[i] = 0;
        }
        return profit;
    }

    public Spell(int cID, int cD0, int cD1, int cD2, int cD3, bool cCast)
    {
        ID = cID;
        delta[0] = cD0;
        delta[1] = cD1;
        delta[2] = cD2;
        delta[3] = cD3;
        castable = cCast;
        doable = true;
    }

}

