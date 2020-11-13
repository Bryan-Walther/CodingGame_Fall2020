using System;
using System.Collections.Generic;
using System.Text;

    class Recipe
{
    public int ID;
    public int[] cost = new int[4];
    public int price;
        
    public int score()
    {
        int weightedCost = cost[0] * 1 + cost[1] * 2 + cost[2] * 3 + cost[3] * 4 + 1;
        double ratio = (double)price / (double)weightedCost;
        return (int)(ratio*100);
    }

    public Recipe(int cID, int cD0, int cD1, int cD2, int cD3, int cPrice)
    {
        ID = cID;
        cost[0] = cD0;
        cost[1] = cD1;
        cost[2] = cD2;
        cost[3] = cD3;
        price = cPrice;
    }
}

