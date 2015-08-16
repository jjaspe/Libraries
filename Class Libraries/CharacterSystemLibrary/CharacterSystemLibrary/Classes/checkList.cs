using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class checkList:System.Collections.Generic.LinkedList<Check>
    {
        private int[] lastThreshholds;

        public int[] LastThreshholds
        {
            get { return lastThreshholds; }
            set { lastThreshholds = value; }
        }
        public Check find()
        {            
            return null;
        }
        public checkList copy()
        {
            checkList copyList = new checkList();
            LinkedListNode<Check> current = this.First;
            while (current != null)
            {
                copyList.AddLast(current.Value.copy(new Check()));
                current = current.Next;
            }
            copyList.LastThreshholds = new int[this.LastThreshholds.Length];
            LastThreshholds.CopyTo(copyList.LastThreshholds, 0);
            return copyList;
        }
        public Check[] toArray()
        {
            return this.ToArray();
        }
        public void add(Check newCheck)
        {
            if (this.Count == 0)
                this.AddFirst(newCheck);
            else
                this.AddAfter(this.Last, newCheck);
        }

        //Returns list of threshholds obtained from all checks in this list
        public int[] doChecks()
        {
            int[] threshHolds = new int[this.Count];
            LinkedListNode<Check> node = this.First;
            for (int i = 0; i < this.Count; i++)
            {
                if (node.Value.isInitialized())
                    threshHolds[i] = node.Value.doCheck();
                node = node.Next;
            }
            lastThreshholds=threshHolds;
            return threshHolds;
        }

        public XmlNode toXml(XmlDocument creator)
        {
            //Create Nodes
            XmlNode topNode = creator.CreateElement("checkList");
            Check[] tempChecks = this.toArray();
            foreach (Check check in tempChecks)
            {
                //Create Node
                XmlNode currentNode = check.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;
        }
        public checkList fromArray(Check[] checks)
        {
            checkList newList = new checkList();
            if (checks != null)
            {
                for (int i = 0; i < checks.Length; i++)
                    newList.add(checks[i]);
            }
            return newList;
        }
    }
}
