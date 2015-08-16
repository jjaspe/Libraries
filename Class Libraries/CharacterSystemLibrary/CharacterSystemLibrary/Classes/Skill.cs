using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Skill:charProperty
    {
        /****************MEMBERS**************************/
        
        // List of weapons this skill modifies
        protected Weapon[] itsWeaponModifiers; 

        // Lists of properties that modify this skill
        protected propertyList itsGoverningAttributes ;
        protected propertyList itsGoverningStats ;
        // Weapons that modify this skill
        protected Weapon[] itsGov_Weapons;

        protected coefficientList itsAtt_Coefficients ;
        

        /****************CONSTRUCTORS*********************/
        public Skill()
        {
            initialize();
            initializeLists();
        }
        public Skill(string name)
        {
            initialize(name);

            initializeLists();
        }
        public Skill(string name, double value)
        {
            initialize(name,value); 
           initializeLists();
        }
        public Skill(string name, double value, int bonus,int max,int min)
        {
            initialize(name,value,bonus,max,min);
        }
        public override charProperty copy()
        {
            return this.copy(new Skill());
        }
        public Skill copy(Skill skill)
        {
            
            Skill copySkill =(Skill) base.copy(skill);
            //copySkill.Name = this.Name;
            //copySkill.Value = this.Value;

            for (int i = 0; i < itsWeaponModifiers.Length; i++)
                this.itsWeaponModifiers[i].copy(copySkill.itsWeaponModifiers[i]);

            
            copySkill.itsGoverningAttributes = this.itsGoverningAttributes.copy();

            for (int i = 0; i < itsGov_Weapons.Length; i++)
                this.itsGov_Weapons[i].copy(copySkill.itsGov_Weapons[i]);

            copySkill.itsGoverningStats  = this.itsGoverningStats .copy();

            copySkill.itsAtt_Coefficients = this.itsAtt_Coefficients.copy();

            return copySkill;
        }
        private void initializeLists()
        {
            itsWeaponModifiers = new Weapon[Globals.WEAPON_LIMIT];
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
                itsWeaponModifiers[i] = new Weapon();

            itsAtt_Coefficients = new coefficientList();
            itsGoverningAttributes = new propertyList();
            itsGoverningStats = new propertyList();
            itsGov_Weapons = new Weapon[0];
        }

        #region ACCESSORS
        //public void setValue(int newValue) { itsValue = newValue; }
        //override public int getValue() { return itsValue; }

        /*************************************************
         * Get index of given weapon. If not found, return
         * WEAPON_LIMIT, which is 1 more than the last index
         * **********************************************/
        public bool isWeapon(string name)
        {
            if (weaponIndex(name) < Globals.WEAPON_LIMIT)
                return true;
            else
                return false;
        }
        public int weaponIndex(string name)
        {
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
            {
                if (itsWeaponModifiers[i].itsName == name)
                {
                    return i;
                }
            }
            return Globals.WEAPON_LIMIT;
        }
        public bool addWeapon(Weapon newWeapon)
        {
            // Make sure a weapon with the same name doesn't exist.
            if(!isWeapon(newWeapon.itsName))
            {
                for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
                {
                    if (itsWeaponModifiers[i].is_empty)
                    {
                        itsWeaponModifiers[i] = newWeapon;
                        itsWeaponModifiers[i].is_empty = false;
                        return true;
                    }
                }
                return false;
            }return false;
        }
        public bool removeWeapon(Weapon oldWeapon)
        {
            int i=weaponIndex(oldWeapon.itsName);
            if (i < Globals.WEAPON_LIMIT)
            {
                itsWeaponModifiers[i].is_empty = true;
                return true;
            }
            else
                return false;
        }

        public bool isGoverningAttribute(string name){
            return itsGoverningAttributes.find(name)==null;
        }        
        public bool addAttribute(Attribute newAttribute)
        {
            // Make sure an att. with the same name doesn't exist.
            if(!isGoverningAttribute(newAttribute.Name))
            {
                if (itsGoverningAttributes.Count < Globals.ATTRIBUTE_LIMIT)
                {
                    if (itsGoverningAttributes.Count == 0)
                        itsGoverningAttributes.add(newAttribute);
                    return true;
                }
                return false;
            }else
                return false;

        }
        public bool removeAttribute(Attribute oldAttribute)
        {
            Attribute foundAttribute = (Attribute) itsGoverningAttributes.find(oldAttribute.Name);
            if (foundAttribute != null)
            {
                itsGoverningAttributes.Remove(oldAttribute);
                return true;
            }
            else
                return false;
        }
        public Attribute getAttribute(string name)
        {
            Attribute foundAttribute = (Attribute)itsGoverningAttributes.find(name);
            if (foundAttribute != null)
            {                
                return foundAttribute;
            }
            else
                return null;
        }
       

        public bool changeCoefficient(Attribute modAttribute, Coefficient newCoef)
        {
            Coefficient current=itsAtt_Coefficients.find(modAttribute.Name);
            current=newCoef;//if its found, then its set, if not then current is null and nothing happens

            return (current==null);
        }
        public bool changeCoefficient(string name, Coefficient newCoef)
        {
            if (isGoverningAttribute(name))
            {
                changeCoefficient(getAttribute(name), newCoef);
                return true;
            }
            else
                return false;
        }
        /* These two methods change the last coefficient, i.e. the one that is not 
         * multiplied by an attribute when recalculating the skill's value. So any change
         * to this coefficient changes the skill by the same amount. ***********/
        public new void increaseValue(double incValue)
        {
            itsAtt_Coefficients.Last.Value.increaseConstant(incValue);
        }
        public new void decreaseValue(double decValue)
        {
            if (itsAtt_Coefficients.Last.Value.getValue() < decValue)
                itsAtt_Coefficients.Last.Value.setNumerator(0);
            else
                itsAtt_Coefficients.Last.Value.increaseConstant(-1 * decValue);
        }
        /* This method returns the coefficient associated with the passed attribute*/
        public Coefficient getCoefficient(string attributeName)
        {
            foreach (Coefficient coef in itsAtt_Coefficients)
            {
                if (coef.VariableName == attributeName)
                    return coef;
            }
            return null;
        }

        public bool isGov_Weapon(string name)
        {
            return (weaponIndex(name) < Globals.WEAPON_LIMIT);
        }
        public int Mod_WeaponIndex(string name)
        {
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
            {
                if (itsGov_Weapons[i].itsName == name)
                {
                    return i;
                }
            }
            return Globals.WEAPON_LIMIT;
        }
        public bool addMod_Weapon(Weapon newWeapon)
        {
            // Make sure a weapon with the same name doesn't exist.
            if (!isGov_Weapon(newWeapon.itsName))
            {
                for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
                {
                    if (itsGov_Weapons[i].is_empty)
                    {
                        itsGov_Weapons[i] = newWeapon;
                        itsGov_Weapons[i].is_empty = false;
                        return true;
                    }
                }
                return false;
            }
            else
                return false;

        }
        public bool removeMod_Weapon(Weapon oldWeapon)
        {
            int i = weaponIndex(oldWeapon.itsName);
            if (i < Globals.WEAPON_LIMIT)
            {
                itsGov_Weapons[i].is_empty = true;
                return true;
            }
            else
                return false;
        }
        public Weapon getMod_Weapon(string name)
        {
            int index = weaponIndex(name);
            if (index < Globals.WEAPON_LIMIT)
                return itsGov_Weapons[index];
            else
                return new Weapon();
        }
        public Weapon getMod_Weapon(int i)
        {
            if (i >= 0 && i < Globals.WEAPON_LIMIT)
                return itsGov_Weapons[i];
            else
                return new Weapon();
        }
        #endregion

        private void calculateValue()
        {
            // Value obtained from governing Attributes
            Attribute[] tempGovAttributes = (Attribute[])itsGoverningAttributes.toArray();
            Coefficient[] tempCoef = itsAtt_Coefficients.toArray();
            for (int i = 0; i < Globals.ATTRIBUTE_LIMIT; i++)
            {
                if (!tempGovAttributes[i].is_empty)
                    BaseValue += tempCoef[i].getValue() * tempGovAttributes[i].BaseValue;
            }
            if (!tempGovAttributes[Globals.ATTRIBUTE_LIMIT].is_empty)
                BaseValue += tempGovAttributes[Globals.ATTRIBUTE_LIMIT].BaseValue;

            // Value obtained after weapon modifiers
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
            {
                BaseValue += itsGov_Weapons[i].getSkillModifier(Name);
            }
        }
        public bool modifyGovAtt(string att_name, double newValue)
        {
            Attribute foundAttribute = (Attribute) itsGoverningAttributes.find(att_name);
            if (foundAttribute != null)
            {
                foundAttribute.BaseValue=newValue;
                return true;
            }
            else
                return false;           
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);

            //Create Nodes
            XmlNode skillNode = creator.CreateElement("Skill");

            XmlNode attributeListNode = creator.CreateElement("Governing_Attributes");
            XmlNodeList attributeNodes = this.itsGoverningAttributes.toXml(creator).ChildNodes;
            for (int i = 0; i < attributeNodes.Count; i++)
                attributeListNode.AppendChild(attributeNodes[i].Clone());

            XmlNode coefficientListNode = creator.CreateElement("Coefficients");

            XmlNodeList coefficientNodes=this.itsAtt_Coefficients.toXml(creator).ChildNodes;
            for (int i = 0; i < coefficientNodes.Count; i++)
                coefficientListNode.AppendChild(coefficientNodes[i].Clone());

            XmlNode statListNode = creator.CreateElement("Governing_Stats");
            XmlNodeList statNodes = this.itsGoverningStats.toXml(creator).ChildNodes;
            for (int i = 0; i < statNodes.Count; i++)
                statListNode.AppendChild(statNodes[i].Clone());


            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                skillNode.AppendChild(parentNode.ChildNodes[i].Clone());
            skillNode.AppendChild(attributeListNode);
            skillNode.AppendChild(coefficientListNode);
            skillNode.AppendChild(statListNode);

            return skillNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);
            XmlNodeList attributeList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Governing_Attributes").Item(0)).GetElementsByTagName("Attribute");
            Attribute currentAtt;
            foreach (XmlNode attributeNode in attributeList)
            {
                currentAtt = new Attribute();
                currentAtt.fromXml(attributeNode);
                itsGoverningAttributes.addOrSet(currentAtt);
            }

            XmlNodeList statList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Governing_Stats").Item(0)).GetElementsByTagName("Stat");
            Stat currentStat;
            foreach (XmlNode Node in statList)
            {
                currentStat = new Stat();
                currentStat.fromXml(Node);
                itsGoverningStats.addOrSet(currentStat);
            }

            XmlNodeList coefficientList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Coefficients").Item(0)).GetElementsByTagName("Coefficient");
            Coefficient currentCoefficient;
            foreach (XmlNode Node in coefficientList)
            {
                currentCoefficient = new Coefficient();
                currentCoefficient.fromXml(Node);
                this.itsAtt_Coefficients.add(currentCoefficient);
            }
        }
    }
}
