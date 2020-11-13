using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//I made a comment
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
        bool submit = false;
        while (true)
        {
            int actionCount = int.Parse(Console.ReadLine()); // the number of spells and recipes in play

            List<Recipe> BrewList = new List<Recipe>();
            List<Spell> SpellList = new List<Spell>();
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
                    BrewList.Add(new Recipe(actionId, delta0, delta1, delta2, delta3, price));
                    //Console.Error.WriteLine($"Added Potion {actionId}");
                }

                if (actionType == "CAST")
                {
                    SpellList.Add(new Spell(actionId, delta0, delta1, delta2, delta3, castable));
                    //Console.Error.WriteLine($"Added Spell {actionId}");
                }

            }

            Witch myWitch = new Witch();
            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int inv0 = int.Parse(inputs[0]); // tier-0 ingredients in inventory
                int inv1 = int.Parse(inputs[1]);
                int inv2 = int.Parse(inputs[2]);
                int inv3 = int.Parse(inputs[3]);
                int score = int.Parse(inputs[4]); // amount of rupees

                if (i == 0)
                    myWitch = new Witch(inv0, inv1, inv2, inv3);
                //Console.Error.WriteLine($"Added Witch {i}");
            }
            

            // Check which recipe should be followed
            double highestRatio = 0;
            int index = 0;
            int count = 0;
            foreach (Recipe potion in BrewList)
            {
                //Console.Error.WriteLine(Math.Abs(potion.score()));
                if (Math.Abs(potion.score()) > highestRatio)
                {
                    //Console.Error.WriteLine($"{count} selected");
                    highestRatio = Math.Abs(potion.score());
                    index = count;
                }
                count++;
            }
            //Now Brewlist[index] is the targeted potion
            Console.Error.WriteLine($"Targeted Potion {BrewList[index].ID}");

            //Determain the cost of the potion, compared to the ingrediants available/
            for (int i = 0; i < 4; i++)
            {
                myWitch.neededInv[i] = myWitch.inventory[i] + BrewList[index].cost[i];
                //Console.Error.WriteLine(myWitch.neededInv[i] + ", " + BrewList[index].cost[i]);
            }
            //Now myWitch.neededInv is an array of how my inventory is in comparison to the chosen recipe

            //Check which spells doable with the given inventory
            foreach (Spell spell in SpellList) {
                for (int i = 0; i < 4; i++)
                {
                    if (spell.doable)
                    {
                        spell.doable = (myWitch.inventory[i] >= Math.Abs(spell.cost()[i]));
                        //Console.Error.WriteLine($"{i}: {(myWitch.inventory[i] >= Math.Abs(spell.cost()[i]))}");
                    }
                }
                //Console.Error.WriteLine($"Spell {spell.ID} is: {spell.doable}");
            }
            //Now the Spell.doable tag is set for each spell, stating if the spell is possible with present inventory or not

            //Console.Error.WriteLine($"Submit: {submit}");
            string Command = "WAIT";
            bool flagRest = false;
            if (submit) {
                submit = false;
                flagRest = true;
                goto resting;
            }

            for (int i = 0; i < 4; i++)
            {
                if (myWitch.neededInv[i] < 0)
                {
                    //Console.Error.WriteLine($"Needed: {myWitch.neededInv[i]} for {i}");
                    foreach (Spell spell in SpellList)
                    {
                        if (spell.gain()[i] > 0) 
                        {
                            //Console.Error.WriteLine($"Spell {spell.ID} for {i}");
                            if (spell.doable && spell.castable)
                            {
                                Command = $"Cast {spell.ID}";
                                flagRest = false;
                            }
                            if (!spell.castable)
                            {
                                flagRest = true;
                            }
                            else if (!spell.doable && Command == "WAIT")
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    myWitch.neededInv[j] += spell.cost()[j];
                                    //Console.Error.WriteLine($"{myWitch.neededInv[j]}, {spell.cost()[j]}");
                                }
                                i = -1;
                                break;
                            }
                        }
                    }
                }
            }

        resting:
            if (flagRest)
            {
                Command = "Rest";
            }
            if (myWitch.neededInv.Min() >= 0)
            {
                Command = $"Brew {BrewList[index].ID}";
                submit = true;
            }
            

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            // in the first league: BREW <id> | WAIT; later: BREW <id> | CAST <id> [<times>] | LEARN <id> | REST | WAIT

            Console.WriteLine(Command);
        }
    }
}