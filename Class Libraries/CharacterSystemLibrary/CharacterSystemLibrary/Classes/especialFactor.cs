using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class especialFactor : Factor
    {
        charProperty itsProperty;
        public charProperty ItsProperty
        {
            get { return itsProperty; }
            set { itsProperty = value; }
        }
        public especialFactor()
        {
            this.ItsProperty = new charProperty();
        }
        public especialFactor(bool constant, charProperty property, int den, int dType, int nDice)
        {
            this.is_constant = constant;
            this.ItsProperty = property;
            this.ItsConstantNum = this.ItsProperty == null ? 0 : this.ItsProperty.Value;
            this.Denominator = den;
            this.DiceType = dType;
            this.NumberOfDice = nDice;
        }

        public especialFactor copy(especialFactor copyFactor)
        {
            copyFactor = (especialFactor)base.copy(copyFactor);
            if (copyFactor == null)
                copyFactor = new especialFactor();
            copyFactor.ItsProperty = this.ItsProperty.copy(new charProperty());
            return copyFactor;
        }

        public new double ItsConstantNum
        {
            get { return ItsProperty.Value; }
            set { base.ItsConstantNum = value; }
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);
            //Create Nodes
            XmlNode factorNode = creator.CreateElement("Dispute Factor");
            XmlNode itsPropertyNode = creator.CreateElement("Its Property");

            //Set Node Values
            itsPropertyNode = this.ItsProperty.toXml(creator);

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                factorNode.AppendChild(parentNode.ChildNodes[i].Clone());
            factorNode.AppendChild(itsPropertyNode);

            return factorNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

            XmlNode propertyNode = ((XmlElement)node).GetElementsByTagName("Its Property").Item(0);
            ItsProperty = new charProperty();
            ItsProperty.fromXml(propertyNode);

        }

    }
}
