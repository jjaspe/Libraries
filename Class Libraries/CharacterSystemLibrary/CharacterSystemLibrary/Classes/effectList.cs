using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class effectList : System.Collections.Generic.LinkedList<Effect>
    {
        private int[] itsThreshholdId;

        public int[] ItsThreshholdId
        {
            private get { return itsThreshholdId; }
            set { itsThreshholdId = value; }
        }

        public Effect find(int i)
        {            
            return this.toArray()[i];
        }
        public effectList copy()
        {
            effectList copyList = new effectList();
            LinkedListNode<Effect> current = this.First;
            while (current != null)
            {
                copyList.AddLast(current.Value.copy(new Effect()));
                current = current.Next;
            }
            if (ItsThreshholdId != null)
            {
                copyList.ItsThreshholdId = new int[this.itsThreshholdId.Length];
                for (int i = 0; i < ItsThreshholdId.Length; i++)
                    copyList.itsThreshholdId[i] = this.itsThreshholdId[i];
            }

            return copyList;
        }
        public Effect[] toArray()
        {
            return this.ToArray();
        }
        public void add(Effect newEffect)
        {
            if (this.Count == 0)
                this.AddFirst(newEffect);
            else
                this.AddAfter(this.Last, newEffect);
        }
        public bool checkThreshholds(int[] checkTH)
        {
            if(ItsThreshholdId!=null && checkTH!=null && itsThreshholdId.Length==checkTH.Length)
            {
                for(int i=0;i<itsThreshholdId.Length;i++)
                {
                    if(itsThreshholdId[i]!=checkTH[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }
        public void doEffects()
        {
            foreach (Effect eff in this)
                eff.doEffect();
        }
        public List<string> getEffectResults()
        {
            List<string> results = new List<string>();
            foreach(Effect effect in this)
                results.Add(effect.myFeedback);

            return results;
        }
            

        public XmlNode toXml()
        {
            //Create Nodes
            XmlDocument creator = new XmlDocument();
            XmlNode topNode = creator.CreateElement("effectList");
            XmlNode itsThreshholdsNode = creator.CreateElement("Threshholds");

            //Set Threshhold Node Values
            foreach (int threshhold in itsThreshholdId)
            {
                XmlElement currentThreshhold = creator.CreateElement("Threshhold");
                currentThreshhold.Value = threshhold.ToString();
                itsThreshholdsNode.AppendChild(currentThreshhold);
            }
            topNode.AppendChild(itsThreshholdsNode);

            Effect[] tempEffects = this.toArray();
            foreach (Effect effect in tempEffects)
            {
                //Create Node
                XmlNode currentNode = effect.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;
        }
    }
}
