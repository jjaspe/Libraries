using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class coefficientList : System.Collections.Generic.LinkedList<Coefficient>
    {
        public Coefficient find(string name)
        {
            LinkedListNode<Coefficient> currentNode = this.First;
            while (currentNode != null)
            {
                if (currentNode.Value.VariableName == name)
                    return currentNode.Value;
                else
                    currentNode = currentNode.Next;
            }
            return null;
        }
        public coefficientList copy()
        {
            coefficientList copyList = new coefficientList();
            LinkedListNode<Coefficient> current = this.First;
            while (current != null)
            {
                copyList.AddLast(current.Value.copy(new Coefficient()));
                current = current.Next;
            }
            return copyList;
        }
        public Coefficient[] toArray()
        {
            return this.ToArray();
        }
        public void add(Coefficient newCoefficient)
        {
            if (this.Count == 0)
                this.AddFirst(newCoefficient);
            else
                this.AddAfter(this.Last, newCoefficient);
        }
        public XmlNode toXml(XmlDocument creator)
        {
            //Create Nodes
            XmlNode topNode = creator.CreateElement("coefficientList");
            Coefficient[] tempCoefs = this.toArray();
            foreach (Coefficient coef in tempCoefs)
            {
                //Create Node
                XmlNode currentNode = coef.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;                
        }
    }
}
