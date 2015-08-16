using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Attribute:charProperty
    {        
        // List of skills this Attribute modifies
        protected propertyList itsSkills;

        /****************CONSTRUCTORS*********************/
        public Attribute()
        {
            initialize();
            itsSkills = new propertyList();
        }
        public Attribute(string name)
        {
            initialize(name);
            itsSkills = new propertyList();
        }
        public Attribute(string name, double value, int bonus,int max,int min)
        {
            initialize(name,value,bonus,max,min);
        }
        public override charProperty copy()
        {
            return this.copy(new Attribute());
        }
        public Attribute copy(Attribute att)
        {

            Attribute copyAttribute =(Attribute) base.copy(att);
            
            //copyAttribute.Name = this.Name;
            //copyAttribute.BaseValue = this.BaseValue;
            copyAttribute.itsSkills = this.itsSkills.copy();

            return copyAttribute;
        }
        public override charProperty getThis()
        {
            return this;
        }

        #region ACCESSORS

       
        public bool addSkills(Skill newSkill)
        {
            if (itsSkills.Count < Globals.SKILL_LIMIT)
            {
                itsSkills.add(newSkill);
                return true;
            }
            return false;
        }
        // This method only sets is_empty to true
        // on the skill.
        public bool removeSkills(Skill oldSkill)
        {
            Skill foundSkill=(Skill)itsSkills.find(oldSkill.Name);
            if (foundSkill!=null)
            {
                itsSkills.Remove(oldSkill);
                return true;
            }
            else
                return false;
        }
        public bool isSkill(string name)
        {
            if (itsSkills.find(name)!=null)
                return true;
            else
                return false;
        }
        /* If Skill wanted is not in target array, a
         * new empty Skill will be returned. */
        public Skill getSkill(string name)
        {
            foreach (Skill skill in itsSkills)
            {
                if (skill.Name == name)
                    return skill;
            }
            return new Skill();
        }
        #endregion

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);

            //Create Nodes
            XmlNode attributeNode = creator.CreateElement("Attribute");
            XmlNode skillListNode = creator.CreateElement("Governed_Skills");
            XmlNodeList skillNodes = this.itsSkills.toXml(creator).ChildNodes;
            for (int i = 0; i < skillNodes.Count; i++)
                skillListNode.AppendChild(skillNodes[i].Clone());
            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count;i++)
                attributeNode.AppendChild(parentNode.ChildNodes[i].Clone());
            attributeNode.AppendChild(skillListNode);

            return attributeNode;
        }
        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);
            XmlNodeList skillList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Governed_Skills").Item(0)).GetElementsByTagName("Skill");
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
