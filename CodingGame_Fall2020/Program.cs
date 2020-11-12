using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


/** 
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        // game loop
        while (true)
        {
            int actionCount = int.Parse(Console.ReadLine()); // the number of spells and recipes in play
            int[] brewIdlist = new int[actionCount];
            int[] pricelist = new int[actionCount];
            int[,] deltalist = new int[actionCount, 4];
            int[,] castIdlist = new int[4, 7];
            //int[,] castDeltalist = new int[actionCount, 4];
            for (int i = 0; i < actionCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int actionId = int.Parse(inputs[0]); // the unique ID of this spell or recipe
                string actionType = inputs[1]; // in the first league: BREW; later: CAST, OPPONENT_CAST, LEARN, BREW
                int delta0 = int.Parse(inputs[2]); // tier-0 ingredient change
                int delta1 = int.Parse(inputs[3]); // tier-1 ingredient change
                int delta2 = int.Parse(inputs[4]); // tier-2 ingredient change
                int delta3 = int.Parse(inputs[5]); // tier-3 ingredient change
                int price = int.Parse(inputs[6]); // the price in rupees if this is a potion
                int tomeIndex = int.Parse(inputs[7]); // in the first two leagues: always 0; later: the index in the tome if this is a tome spell, equal to the read-ahead tax
                int taxCount = int.Parse(inputs[8]); // in the first two leagues: always 0; later: the amount of taxed tier-0 ingredients you gain from learning this spell
                bool castable = inputs[9] != "0"; // in the first league: always 0; later: 1 if this is a castable player spell
                bool repeatable = inputs[10] != "0"; // for the first two leagues: always 0; later: 1 if this is a repeatable player spell

                if (actionType == "BREW")
                {
                    brewIdlist[i] = actionId;
                    pricelist[i] = price;
                    deltalist[i, 0] = delta0;
                    deltalist[i, 1] = delta1;
                    deltalist[i, 2] = delta2;
                    deltalist[i, 3] = delta3;
                }

                if (delta0 > 0 && actionType == "CAST")
                {
                    castIdlist[0, 0] = actionId;
                    castIdlist[0, 1] = delta0;
                    castIdlist[0, 2] = delta1;
                    castIdlist[0, 3] = delta2;
                    castIdlist[0, 4] = delta3;
                    castIdlist[0, 5] = Convert.ToInt32(castable);
                    castIdlist[0, 6] = 1;
                }
                if (delta1 > 0 && actionType == "CAST")
                {
                    castIdlist[1, 0] = actionId;
                    castIdlist[1, 1] = delta0;
                    castIdlist[1, 2] = delta1;
                    castIdlist[1, 3] = delta2;
                    castIdlist[1, 4] = delta3;
                    castIdlist[1, 5] = Convert.ToInt32(castable);
                    castIdlist[1, 6] = 1;
                }
                if (delta2 > 0 && actionType == "CAST")
                {
                    castIdlist[2, 0] = actionId;
                    castIdlist[2, 1] = delta0;
                    castIdlist[2, 2] = delta1;
                    castIdlist[2, 3] = delta2;
                    castIdlist[2, 4] = delta3;
                    castIdlist[2, 5] = Convert.ToInt32(castable);
                    castIdlist[2, 6] = 1;
                }
                if (delta3 > 0 && actionType == "CAST")
                {
                    castIdlist[3, 0] = actionId;
                    castIdlist[3, 1] = delta0;
                    castIdlist[3, 2] = delta1;
                    castIdlist[3, 3] = delta2;
                    castIdlist[3, 4] = delta3;
                    castIdlist[3, 5] = Convert.ToInt32(castable);
                    castIdlist[3, 6] = 1;
                }

            }
            int[] inventory = new int[4];
            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int inv0 = int.Parse(inputs[0]); // tier-0 ingredients in inventory
                int inv1 = int.Parse(inputs[1]);
                int inv2 = int.Parse(inputs[2]);
                int inv3 = int.Parse(inputs[3]);
                int score = int.Parse(inputs[4]); // amount of rupees
                if (i == 0)
                {
                    inventory[0] = inv0;
                    inventory[1] = inv1;
                    inventory[2] = inv2;
                    inventory[3] = inv3;
                }

            }


            //Check if ingrediants are avaiable for all casts
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (castIdlist[i, j + 1] < 0)
                    {
                        Console.Error.WriteLine("List " + (j + 1) + ", " + castIdlist[i, j + 1] + ", " + inventory[j] + ".");
                        if (inventory[j] < 1)
                        {
                            inventory[j]--;
                            castIdlist[i, 6] = 0; //not enough items in the inventory
                            Console.Error.WriteLine("Can't Cast");
                        }
                    }
                }
            }

            //What's needed
            int Index = pricelist.ToList().IndexOf(pricelist.Max());
            Console.Error.WriteLine(Index + ", " + pricelist[Index] + ".");
            for (int i = 0; i < 4; i++)
            {
                inventory[i] += deltalist[Index, i];
                Console.Error.WriteLine(inventory[i] + ", " + deltalist[Index, i] + ", ");
            }

            //
            string[] writeCommand = new string[2];

            //Check if enough ingrediants are in the inventory
            bool flagRest = false;
            if (inventory[0] < 0 && castIdlist[0, 6] == 1)
            {
                if (castIdlist[0, 5] == 1)
                {
                    writeCommand[0] = "Cast";
                    writeCommand[1] = $"{castIdlist[0, 0]}";
                    flagRest = false;
                }
                else
                {
                    flagRest = true;
                    Console.Error.WriteLine("0");
                }
            }
            if (inventory[1] < 0 && castIdlist[1, 6] == 1)
            {
                if (castIdlist[1, 5] == 1)
                {
                    writeCommand[0] = "Cast";
                    writeCommand[1] = $"{castIdlist[1, 0]}";
                    flagRest = false;
                }
                else
                {
                    flagRest = true;
                    Console.Error.WriteLine("1");
                }
            }
            if (inventory[2] < 0 && castIdlist[2, 6] == 1)
            {
                if (castIdlist[2, 5] == 1)
                {
                    writeCommand[0] = "Cast";
                    writeCommand[1] = $"{castIdlist[2, 0]}";
                    flagRest = false;
                }
                else
                {
                    flagRest = true;
                    Console.Error.WriteLine("2");
                }
            }
            if (inventory[3] < 0 && castIdlist[3, 6] == 1)
            {
                if (castIdlist[3, 5] == 1)
                {
                    writeCommand[0] = "Cast";
                    writeCommand[1] = $"{castIdlist[3, 0]}";
                    flagRest = false;
                }
                else
                {
                    flagRest = true;
                    Console.Error.WriteLine("3");
                }
            }

            if (inventory.Min() >= 0)
            {
                writeCommand[0] = "Brew";
                writeCommand[1] = $"{brewIdlist[Index]}";
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            // in the first league: BREW <id> | WAIT; later: BREW <id> | CAST <id> [<times>] | LEARN <id> | REST | WAIT
            if (flagRest)
            {
                Console.WriteLine("Rest");
            }
            else
                Console.WriteLine($"{writeCommand[0]} {writeCommand[1]}");
        }
    }
}