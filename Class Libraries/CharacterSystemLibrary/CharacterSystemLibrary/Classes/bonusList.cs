using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class bonusList: System.Collections.Generic.LinkedList<Bonus>
    {
        public Bonus find(string name)
        {
            foreach (Bonus bonus in this)
                if (bonus.ItsProperty.Name == name)
                    return bonus;
            return null;
        }
        public bonusList copy()
        {
            bonusList copyList = new bonusList();
            LinkedListNode<Bonus> current = this.First;
            while (current != null)
            {
                copyList.AddLast(current.Value.copy(new Bonus()));
                current = current.Next;
            }          

            return copyList;
        }
        public Bonus[] toArray()
        {
            return this.ToArray();
        }
        public void add(Bonus newBonus)
        {
            if (this.Count == 0)
                this.AddFirst(newBonus);
            else
                this.AddAfter(this.Last, newBonus);
        }
        public void addOrSet(Bonus newBonus)
        {
            Bonus foundBonus = this.find(newBonus.ItsProperty.Name);
            if (foundBonus != null)
                newBonus.copy(foundBonus);
            else
                add(newBonus);
        }


        public XmlNode toXml(XmlDocument creator)
        {
            //Create Nodes
            XmlNode topNode = creator.CreateElement("bonusList");
            XmlNode itsThreshholdsNode = creator.CreateElement("Threshholds");            

            Bonus[] tempBonus = this.toArray();
            foreach (Bonus bonus in tempBonus)
            {
                //Create Node
                XmlNode currentNode = bonus.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;
        }
        
    }
}
