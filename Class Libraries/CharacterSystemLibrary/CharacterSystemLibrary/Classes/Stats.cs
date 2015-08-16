using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Stat : charProperty
    {       
        

        /*************************CONSTRUCTORS***********************/
        public Stat()
        {
            initialize();
        }
        public Stat(string name, double value)
        {
            initialize(name, value);
        }
        public Stat(string name, double value, int bonus,int max,int min)
        {
            initialize(name,value,bonus,max,min);
        }
        public override charProperty copy()
        {
            return this.copy(new Stat());
        }
        public Stat copy(Stat copyStat)
        {
            copyStat=(Stat)base.copy(copyStat);
            if (copyStat == null)
                copyStat = new Stat();

            return copyStat;
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);
            //Create Nodes
            XmlNode statNode = creator.CreateElement("Stat");

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                statNode.AppendChild(parentNode.ChildNodes[i].Clone());

            return statNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

           
        }
    }
}
