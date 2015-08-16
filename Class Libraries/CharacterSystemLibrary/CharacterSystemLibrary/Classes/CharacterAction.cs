using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    public class CharacterAction : Action
    {
        public string itsName;
        protected string[] itsEffects;
        protected Character itsCharacter;
        protected Attribute[] itsGov_Attributes;
        protected Skill[] itsGov_Skills;
        protected Stat[] itsGov_Stats;
        /* itsCoefficients will be used to determine the value of effects.
         * itsCoefficients[i,] are the coefficients used for effect #i,
         * and its value will be calculated as so:
         * Value of Effect i=itsCoefficients[i,0]*itsProperties[0]+itsCoefficients[i,1]*itsProperties[1]...
         *      + itsCoefficients[i,lastProperty]*itsProperties[lastProperty] + itsCoefficients[i,lastProperty+1];
         * ***************************************************************************************************/
        protected Coefficient[,] itsCoefficients;

        /**************************CONSTRUCTORS******************************/
        public CharacterAction()
        {
            itsName = " ";
            itsEffects = new string[Globals.EFFECT_LIMIT];
            itsCharacter = new Character();
            itsCoefficients = new Coefficient[Globals.EFFECT_LIMIT, Globals.PROPERTY_LIMIT + 1];
        }
        public CharacterAction(string name)
        {
            itsName = name;
            itsEffects = new string[Globals.EFFECT_LIMIT];
            itsCharacter = new Character();
            itsCoefficients = new Coefficient[Globals.EFFECT_LIMIT, Globals.PROPERTY_LIMIT + 1];
        }

        public int effectIndex(string name){
            for(int i=0;i<Globals.EFFECT_LIMIT;i++){
                if(itsEffects[i].Equals(name))
                    return i;
            }
            return Globals.EFFECT_LIMIT;
        }
        /* For every effect, calculate the effect's value,
         * then subtract the value from the value of the 
         * corresponding property from target character.
         * If some effect couldn't be carried out because
         * it was turn ending, and it's value was too low,
         * return false; */
        public bool doAction(Character target)
        {
            /*
            if (!itsCharacter.is_empty)
            {
                int value;                
                Stat currentStat = new Stat();
                charProperty property = new charProperty();
                for (int i = 0; i < Globals.EFFECT_LIMIT; i++)
                {
                    value = 0;                    
                    for (int j = 0; j < Globals.PROPERTY_LIMIT; j++)
                    {
                        // Set property to the right character property
                        if (j < Globals.ATTRIBUTE_LIMIT)
                            property = itsGov_Attributes[j];
                        else if (j < Globals.ATTRIBUTE_LIMIT + Globals.SKILL_LIMIT)
                            property = itsGov_Skills[j - Globals.ATTRIBUTE_LIMIT];
                        else
                            property = itsGov_Stats[j - Globals.ATTRIBUTE_LIMIT - Globals.SKILL_LIMIT];

                        // Add up new value to previous total
                        if (!property.is_empty)
                            value += itsCoefficients[i, j].getValue() * property.Value;
                    }
                    value += itsCoefficients[i, Globals.PROPERTY_LIMIT].getValue();
                    /* If affected property is a stat, we're good.
                     * Else, the getStat method will return an empty
                     * stat, which is fine for our purposes, since its
                     * is_turn_ending member is false.          */
            /*
                    currentStat = target.getStat(itsEffects[i]);
                    if (currentStat.is_Turn_Ending)
                    {
                        if (currentStat.Value < value)
                            return false;
                    }*/
                    /* Since we don't know which type of property it is,
                     * we call all the decrease methods. If a property of the
                     * wanted name is not found, nothing will be done. */
            /*
                    target.decreaseStat(itsEffects[i], value);
                    target.decreaseAttribute(itsEffects[i], value);
                    target.decreaseSkill(itsEffects[i], value);

                }
                return true;
            }
            else
                return false;*/
            return false;
        
        }
        public bool isEffect(string name)
        {
           if(effectIndex(name)<Globals.EFFECT_LIMIT)
               return true;
           else
               return false;
        }
        /* Gets value of effect with given name
         * If effect is not in action, return 0. */
       
        public void setCharacter(Character character)
        {
            itsCharacter = character;
            itsGov_Attributes = itsCharacter.getAllAttributes();
            itsGov_Skills = itsCharacter.getAllSkills();
            itsGov_Stats = itsCharacter.getAllStats();
        }
    }
}
