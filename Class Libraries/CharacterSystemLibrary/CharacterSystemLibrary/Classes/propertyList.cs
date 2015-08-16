using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class propertyList:System.Collections.Generic.LinkedList<charProperty>
    {
        public propertyList()
        {
            
        }
        public charProperty find(string itsName)
        {
            LinkedListNode<charProperty> currentNode = this.First;
            while (currentNode != null)
            {
                if (currentNode.Value.Name == itsName)
                    return currentNode.Value.getThis();
                else
                    currentNode = currentNode.Next;
            }
            return null;
        }        

        public propertyList copy()
        {
            propertyList copyList=new propertyList();
            LinkedListNode<charProperty> current=this.First;
            while(current!=null)
            {
                copyList.AddLast(current.Value.copy());
                current=current.Next;
            }
            return copyList;
        }

        public charProperty[] toArray()
        {
            return this.ToArray();
        }



        public void add(charProperty newProperty)
        {
            if (this.Count == 0)
                this.AddFirst(newProperty);
            else
                this.AddAfter(this.Last, newProperty);
        }

        public void addOrSet(charProperty newProperty)
        {
            charProperty foundProperty = this.find(newProperty.Name);
            //if something with that name and it was the same type, replace, else add it
            if (foundProperty != null && foundProperty.GetType()==newProperty.GetType())
                newProperty.copy(foundProperty);
            else
                add(newProperty);
        }

        public XmlNode toXml(XmlDocument creator)
        {
            //Create Nodes
            XmlNode topNode = creator.CreateElement("propertyList");
            //Check Type to call appropriate list method
            
            charProperty[] tempProperties = this.toArray();
            Stat[] stats=tempProperties.OfType<Stat>().ToArray();
            Skill[] skills = tempProperties.OfType<Skill>().ToArray();
            Attribute[] atts = tempProperties.OfType<Attribute>().ToArray();
            XmlNode currentNode;
            foreach (Stat stat in stats)
            {
                //Create Node
                currentNode = stat.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }
            foreach (Skill skill in skills)
            {
                //Create Node
                currentNode = skill.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }
            foreach (Attribute attribute in atts)
            {
                //Create Node
                currentNode = attribute.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;
        }
    }
}
