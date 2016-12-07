using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
The Gale–Shapley algorithm
function stableMatching {
    Initialize all m ? M and w ? W to free
    while ? free man m who still has a woman w to propose to {
       w = first woman on m’s list to whom m has not yet proposed
       if w is free
         (m, w) become engaged
       else some pair (m', w) already exists
         if w prefers m to m'
            m' becomes free
           (m, w) become engaged 
         else
           (m', w) remain engaged
    }
}
 */

namespace stableMarriageProblem
{
    // all partners are numbered from 1 to n (there is no 0 or negative)
    enum state {free, engaged}
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfIterations = int.Parse(Console.ReadLine());
            for (int i = 0; i<numberOfIterations; i++) 
            {
                int halfThePeople = int.Parse(Console.ReadLine());
                Matchmaker matchingSystem = new Matchmaker(halfThePeople);
                matchingSystem.loadData();
                //matchingSystem.debug_PrintData();
                matchingSystem.stableMatch();
                matchingSystem.printMatches();
            }
        }
    }

    abstract class Person
    {
        public int[] preference_table;
        public state availability;
        public int Partner {get; set;} 

        public Person(int halfThePeople)
        {
            preference_table = new int[halfThePeople];
            availability = state.free;
            Partner = -1; // non-existant partner number
        }
        public void setAvailability(state newState)
        {
            this.availability = newState;
        }
        public state getAvailability()
        {
            return this.availability;
        }
    }

    class Woman : Person
    {
        public Woman(int halfThePeople)
            : base(halfThePeople)
        {

        }
    }

    class Man : Person
    {
        public bool[] proposal_table;
        
        public Man(int halfThePeople) : base(halfThePeople)
        {
            proposal_table = new bool[halfThePeople];
            for (int i = 0; i < halfThePeople; i++)
            {
                proposal_table[i] = false;
            }
        }
    }

    class Matchmaker
    {
        Woman[] women;
        Man[] men;
        int HalfThePeople { get; set; }
        int takenMen;

        public Matchmaker(int halfThePeople)
        {
            
            women = new Woman[halfThePeople];
            men = new Man[halfThePeople];
            HalfThePeople = halfThePeople;
            takenMen = 0;
        }
        public void loadData()
        {
            for (int i = 0; i < HalfThePeople; i++)
            {
                Woman newWoman = new Woman(HalfThePeople);
                string currentInput = Console.ReadLine();
                string[] inputData = currentInput.Split();
                // ignore 1st element - person number - reduced by one it's the index in the Matchmaker array
                // ignore last element - empty space
                for (int j = 1; j < inputData.Length; j++)
                {
                    newWoman.preference_table[j - 1] = int.Parse(inputData[j]);
                    newWoman.preference_table[j - 1]--; // shift partner numbers to match array index numbers
                }
                women[i] = newWoman;
            }
            for (int i = 0; i < HalfThePeople; i++)
            {
                Man newMan = new Man(HalfThePeople);
                string currentInput = Console.ReadLine();
                string[] inputData = currentInput.Split();
                // ignore 1st element - person number - reduced by one it's the index in the Matchmaker array
                // ignore last element - empty space
                for (int j = 1; j < inputData.Length; j++)
                {
                    newMan.preference_table[j - 1] = int.Parse(inputData[j]);
                    newMan.preference_table[j - 1]--; // shift partner numbers to match array index numbers
                }
                men[i] = newMan;
            }
        }

        public void debug_PrintData()
        {
            Console.WriteLine("==============");
            Console.WriteLine("Women: ");
            for (int i = 0; i < HalfThePeople; i++)
            {
                Console.Write(i + 1);
                Console.Write(" prefers: ");
                foreach (var ele in women[i].preference_table){
                    Console.Write((ele+1) + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Men: ");
            for (int i = 0; i < HalfThePeople; i++)
            {
                Console.Write(i + 1);
                Console.Write(" prefers: ");
                foreach (var ele in men[i].preference_table)
                {
                    Console.Write((ele+1) + " ");
                }
                Console.WriteLine();
            }
        }

        public void stableMatch()
        {
            while (takenMen != HalfThePeople)
            {
                for (int i = 0; i < HalfThePeople; i++)
                {
                    if (men[i].availability == state.free)
                    {
                        int notProposedIndex = 0;
                        for (int j = 0; j < HalfThePeople; j++)
                        {
                            if (men[i].proposal_table[j] == false)
                            {
                                notProposedIndex = men[i].preference_table[j];
                                men[i].proposal_table[j] = true;
                                break;
                            }
                        }
                        if (women[notProposedIndex].availability == state.free)
                        {
                            women[notProposedIndex].availability = state.engaged;
                            men[i].availability = state.engaged;
                            women[notProposedIndex].Partner = i;
                            men[i].Partner = notProposedIndex;
                            takenMen++;
                            //Console.WriteLine("debug - engaging man : " + (women[notProposedIndex].Partner + 1) + " with woman " + (notProposedIndex+1));
                            //Console.ReadLine();
                        }
                        else
                        {
                            int currentPartner = women[notProposedIndex].Partner;
                            int currentPartnerIndex = -1;
                            int possibleSwitchPartnerIndex = -1;
                            for (int k = 0; k < HalfThePeople; k++)
                            {
                                if (women[notProposedIndex].preference_table[k] == currentPartner)
                                {
                                    currentPartnerIndex = k;
                                }
                                if (women[notProposedIndex].preference_table[k] == i)
                                {
                                    possibleSwitchPartnerIndex = k;
                                }
                            }
                            if (possibleSwitchPartnerIndex < currentPartnerIndex)
                            {
                                //women[notProposedIndex].availability = state.engaged;
                                men[i].availability = state.engaged;
                                women[notProposedIndex].Partner = i;
                                men[i].Partner = notProposedIndex;
                                men[currentPartner].availability = state.free;
                            }
                        }
                    }
                }
            }
        }

        public void printMatches()
        {
            for (int i = 0; i < HalfThePeople; i++)
            {
                Console.Write(i + 1);
                Console.Write(" " + (men[i].Partner+1));
                Console.WriteLine();
            }
        }
    }
}
