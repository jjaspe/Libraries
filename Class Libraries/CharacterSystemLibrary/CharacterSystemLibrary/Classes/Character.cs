using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace CharacterSystemLibrary.Classes
{
    public class Character:TBCComponents
    {
        private string itsName;

        public string Name
        {
            get { return itsName; }
            set { itsName = value; }
        }
        protected propertyList itsAttributes;
        protected propertyList itsSkills;
        protected propertyList itsStats;
        protected characterList itsTargets;
        protected Weapon[] itsWeapons;
        private Weapon itsCurrentWeapon;

        public Weapon ItsCurrentWeapon
        {
            get { return itsCurrentWeapon; }
            set { itsCurrentWeapon = value; }
        }
        public List<Stat> Stats
        {
            get
            {
                charProperty[] statArray = itsStats.toArray();
                List<Stat> stats = new List<Stat>();
                foreach (charProperty c in statArray)
                    stats.Add((Stat)c);
                return stats;
            }
        }
        public List<Skill> Skills
        {
            get
            {
                charProperty[] skillArray = itsSkills.toArray();
                List<Skill> skills = new List<Skill>();
                foreach (charProperty c in skillArray)
                    skills.Add((Skill)c);
                return skills;
            }
        }
        public List<Attribute> Attributes
        {
            get
            {
                charProperty[] attArray = itsAttributes.toArray();
                List<Attribute> attributes = new List<Attribute>();
                foreach (charProperty c in attArray)
                    attributes.Add((Attribute)c);
                return attributes;
            }
        }

        /*************************CONSTRUCTORS************************/
        public Character()
        {
            Name = " ";
            initializeLists();
        }
        public Character(string name)
        {
            Name = name; 
            initializeLists();
            itsTargets = new characterList();
        }

        public  Character copy(Character copyCharacter)
        {
            copyCharacter=(Character)base.copy(copyCharacter);
            if (isInitialized())
            {
                copyCharacter.Name = this.Name;
                copyCharacter.itsCurrentWeapon=this.ItsCurrentWeapon.copy(new Weapon());
                copyCharacter.itsAttributes = this.itsAttributes.copy();
                copyCharacter.itsSkills = this.itsSkills.copy();
                copyCharacter.itsStats = this.itsStats.copy();
                copyCharacter.itsTargets = this.itsTargets.copy();

                for (int i = 0; i < itsWeapons.Length; i++)
                    this.itsWeapons[i].copy(copyCharacter.itsWeapons[i]);
            }
            return copyCharacter;
        }

        private void initializeLists()
        {
            itsStats = new propertyList();
            itsSkills = new propertyList();
            itsAttributes = new propertyList();
            itsTargets = new characterList();
            itsWeapons = new Weapon[Globals.WEAPON_LIMIT];
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
                itsWeapons[i] = new Weapon();
        }

        private bool isInitialized()
        {
            return (this.itsAttributes != null && this.itsSkills != null && this.itsStats != null);
        }

        #region ACCESSORS
        public void decreaseValue(charProperty property,int value)
        {
            property.decreaseValue(value);
        }
        public void increaseValue(charProperty property,int value)
        {
            property.increaseValue(value);
        }
        public void decreaseValue(string property, int value)
        {
            charProperty prop = this.getProperty(property);
            if (prop != null)
                prop.decreaseValue(value);
        }
        public void increaseValue(string property, int value)
        {
            charProperty prop = this.getProperty(property);
            if (prop != null)
                prop.increaseValue(value);
        }

        public void decreaseValue(charProperty property, double value)
        {
            property.decreaseValue(value);
        }
        public void increaseValue(charProperty property, double value)
        {
            property.increaseValue(value);
        }

        public void decreaseValue(string property, double value)
        {
            charProperty prop = this.getProperty(property);
            if(prop!=null)
                prop.decreaseValue(value);
        }
        public void increaseValue(string property, double value)
        {
            charProperty prop = this.getProperty(property);
            if (prop != null)
                prop.increaseValue(value);
        }

        public void setValue(string property, double value)
        {
            charProperty prop = this.getProperty(property);
            if (prop != null)
                prop.BaseValue = value;
        }

        public double getValue(string property)
        {
            charProperty prop = this.getProperty(property);
            if (prop != null)
                return prop.Value;
            return Double.NaN;
        }
        public Attribute[] getAllAttributes()
        {
            return Attributes.ToArray();
        }        
        public bool addAttribute(Attribute newAttribute)
        {
            if (itsAttributes.Count < Globals.ATTRIBUTE_LIMIT)
            {
                itsAttributes.add(newAttribute);
                return true;
            }
            return false;
        }
        public bool removeAttribute(Attribute oldAttribute)
        {
            if(isAttribute(oldAttribute.Name))
            {
                itsAttributes.Remove(oldAttribute);
                return true;
            }
            else
                return false;
        }
        public bool isAttribute(string name)
        {
            return itsAttributes.find(name) != null;
        }
        public Attribute getAttribute(string name)
        {
            return (Attribute)itsAttributes.find(name);
        }
        /* Sets the value for this attribute 
         */
        public bool setAttribute(string name, int value)
        {
            Attribute foundAttribute = getAttribute(name);
            if (foundAttribute != null)
            {
                foundAttribute.BaseValue = value;
                return true;
            }
            return false;
        }
        public bool increaseAttribute(string name, int value)
        {
            Attribute foundAttribute = getAttribute(name);
            if (foundAttribute != null)
            {
                foundAttribute.increaseValue(value);
                return true;
            }
            return false;
        }
        public bool decreaseAttribute(string name, int value)
        {
            Attribute foundAttribute = getAttribute(name);
            if (foundAttribute != null)
            {
                foundAttribute.decreaseValue(value);
                return true;
            }
            return false;
        }

        public Skill[] getAllSkills()
        {           
            return Skills.ToArray();
        }
        public bool addSkill(Skill newSkill)
        {
            if (itsSkills.Count < Globals.SKILL_LIMIT)
            {
                itsSkills.add(newSkill);
                return true;
            }
            return false;
        }
        public bool removeSkill(Skill oldSkill)
        {
            if (isSkill(oldSkill.Name))
            {
                itsSkills.Remove(oldSkill);
                return true;
            }
            else
                return false;
        }
        public bool isSkill(string name)
        {
            return itsSkills.find(name) != null;
        }
        public Skill getSkill(string name)
        {
            return (Skill)itsSkills.find(name);
        }
        /* Sets the value for this attribute 
         */
        public bool setSkill(string name, int value)
        {
            Skill foundSkill = getSkill(name);
            if (foundSkill != null)
            {
                foundSkill.BaseValue = value;
                return true;
            }
            return false;
        }
        public bool increaseSkill(string name, int value)
        {
            Skill foundSkill = getSkill(name);
            if (foundSkill != null)
            {
                foundSkill.increaseValue(value);
                return true;
            }
            return false;
        }
        public bool decreaseSkill(string name, int value)
        {
            Skill foundSkill = getSkill(name);
            if (foundSkill != null)
            {
                foundSkill.decreaseValue(value);
                return true;
            }
            return false;
        }

        public Stat[] getAllStats()
        {
            return Stats.ToArray(); 
        }
        public bool addStat(Stat newStat)
        {
            if (itsStats.Count < Globals.STAT_LIMIT)
            {
                itsStats.add(newStat);
                return true;
            }
            return false;
        }
        public bool removeStat(Stat oldStat)
        {
            if (isStat(oldStat.Name))
            {
                itsStats.Remove(oldStat);
                return true;
            }
            else
                return false;
        }
        public bool isStat(string name)
        {
            return itsStats.find(name) != null;
        }
        /// <summary>
        /// returns null if stat not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Stat getStat(string name)
        {
            return (Stat)itsStats.find(name);
        }
        /* Sets the value for this attribute 
         */
        public bool setStat(string name, double value)
        {
            Stat foundStat = getStat(name);
            if (foundStat != null)
            {
                foundStat.BaseValue = value;
                return true;
            }
            return false;
        }
        public bool increaseStat(string name, int value)
        {
            Stat foundStat = getStat(name);
            if (foundStat != null)
            {
                foundStat.increaseValue(value);
                return true;
            }
            return false;
        }
        public bool decreaseStat(string name, int value)
        {
            Stat foundStat = getStat(name);
            if (foundStat != null)
            {
                foundStat.decreaseValue(value);
                return true;
            }
            return false;
        }

        public charProperty getProperty(string name)
        {
            Stat st = getStat(name);
            if (st != null)
                return st;

            Skill sk = getSkill(name);
            if (sk != null)
                return sk;

            Attribute att = getAttribute(name);
            if (att != null)
                return att;

            return null;

        }


        public Character[] getAllTargets()
        {
            return (Character[])itsTargets.toArray();
        }
        public bool addTarget(Character newTarget)
        {
            if (itsTargets.Count < Globals.CHARACTER_LIMIT)
            {
                itsTargets.add(newTarget);
                return true;
            }
            return false;
        }
        public bool removeTarget(Character oldTarget)
        {
            if (isTarget(oldTarget.Name))
            {
                itsTargets.Remove(oldTarget);
                return true;
            }
            else
                return false;
        }
        public bool isTarget(string name)
        {
            return itsTargets.find(name) != null;
        }
        public Character getTarget(string name)
        {
            return itsTargets.find(name);
        }

        public int weaponIndex(string name)
        {
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
            {
                if (itsWeapons[i].itsName == name)
                {
                    return i;
                }
            }
            return Globals.WEAPON_LIMIT;
        }
        public bool addWeapon(Weapon newWeapon)
        {
            for (int i = 0; i < Globals.WEAPON_LIMIT; i++)
            {
                if (itsWeapons[i].is_empty)
                {
                    itsWeapons[i] = newWeapon;
                    itsWeapons[i].is_empty = false;
                    return true;
                }
            }
            return false;
        }
        public bool removeWeapon(Weapon oldWeapon)
        {
            int i = weaponIndex(oldWeapon.itsName);
            if (i < Globals.WEAPON_LIMIT)
            {
                itsWeapons[i].is_empty = true;
                return true;
            }
            else
                return false;
        }
        public bool isWeapon(string name)
        {
            if (weaponIndex(name) < Globals.WEAPON_LIMIT)
                return true;
            else
                return false;
        }
        /* If Weapons wanted is not in actions array, a
         * new empty Weapons will be returned. */
        public Weapon getWeapon(string name)
        {
            foreach (Weapon weapon in itsWeapons)
            {
                if (weapon.itsName == name)
                    return weapon;
            }
            return new Weapon();
        }

        public bool setCurrentWeapon(string name)
        {
            int i = weaponIndex(name);
            if (i < Globals.WEAPON_LIMIT)
            {
                itsCurrentWeapon = itsWeapons[i];
                return true;
            }
            else
                return false;
        }
        public Weapon getCurrentWeapon()
        {
            if (itsCurrentWeapon.is_empty)
                //We can't just return current weapon here
                // since it might not be empty even if its
                // is_empty member is true
                return new Weapon();
            else
                return itsCurrentWeapon;
        }

        #endregion

        /*************************WEAPON STUFF************************/
        public bool addUserToWeapon()
        {
            if (itsCurrentWeapon.is_empty)
                return false;
            else
            {
                itsCurrentWeapon.addUser(this);
                return true;
            }
        }
        public bool removeUserFromWeapon()
        {
            if (itsCurrentWeapon.is_empty)
                return false;
            else
            {
                return (itsCurrentWeapon.removeUser(this));
            }
        }
        public bool addTargetToWeapon(string targetName)
        {
            if (!itsCurrentWeapon.is_empty)
            {
                if (isTarget(targetName))
                {
                    itsCurrentWeapon.addTarget(itsTargets.find(targetName));
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public bool removeTargetFromWeapon(string targetName)
        {
            if (!itsCurrentWeapon.is_empty)
            {
                if (isTarget(targetName))
                {
                    return itsCurrentWeapon.removeTarget(itsTargets.find(targetName));
                    
                }
                else
                    return false;
            }
            else
                return false;
        }                
        
        public bool weaponAction(string name)
        {
            if (itsCurrentWeapon.is_empty)
                return false;
            else
                return itsCurrentWeapon.doAction(name);
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);

            //Create Nodes
            XmlNode characterNode = creator.CreateElement("Character");

            if (isInitialized())
            {
                XmlNode attributeListNode = creator.CreateElement("Attributes");
                XmlNodeList attributeNodes = this.itsAttributes.toXml(creator).ChildNodes;
                for (int i = 0; i < attributeNodes.Count; i++)
                    attributeListNode.AppendChild(attributeNodes[i].Clone());

                XmlNode skillListNode = creator.CreateElement("Skills");
                XmlNodeList skillNodes = this.itsSkills.toXml(creator).ChildNodes;
                for (int i = 0; i < skillNodes.Count;i++ )
                    skillListNode.AppendChild(skillNodes[i].Clone());

                XmlNode statListNode = creator.CreateElement("Stats");
                XmlNodeList statNodes = this.itsStats.toXml(creator).ChildNodes;
                for (int i = 0; i < statNodes.Count; i++)
                    statListNode.AppendChild(statNodes[i].Clone());

                XmlElement nameNode = creator.CreateElement("Name");
                nameNode.InnerText= this.Name;

                /* Join Nodes, starting with parent nodes */
                for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                    characterNode.AppendChild(parentNode.ChildNodes[i].Clone());
                characterNode.AppendChild(nameNode);
                characterNode.AppendChild(attributeListNode);
                characterNode.AppendChild(skillListNode);
                characterNode.AppendChild(statListNode);
            }

            return characterNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

            this.itsName = ((XmlElement)node).
                GetElementsByTagName("Name").Item(0).InnerText;

            XmlNodeList attributeList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Attributes").Item(0)).GetElementsByTagName("Attribute");
            Attribute currentAtt;
            foreach (XmlNode attributeNode in attributeList)
            {
                currentAtt = new Attribute();
                currentAtt.fromXml(attributeNode);
                itsAttributes.addOrSet(currentAtt);
            }

            XmlNodeList statList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Stats").Item(0)).GetElementsByTagName("Stat");
            Stat currentStat;
            foreach (XmlNode Node in statList)
            {
                currentStat = new Stat();
                currentStat.fromXml(Node);
                itsStats.addOrSet(currentStat);
            }

            XmlNodeList skillList = ((XmlElement)((XmlElement)node).
                 GetElementsByTagName("Skills").Item(0)).GetElementsByTagName("Skill");
            Skill current;
            foreach (XmlNode skillNode in skillList)
            {
                current = new Skill();
                current.fromXml(skillNode);
                itsSkills.addOrSet(current);
            }
        }

        
    }
}
