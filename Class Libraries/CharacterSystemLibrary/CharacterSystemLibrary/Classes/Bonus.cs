using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Bonus:TBCComponents
    {
        private double itsValue;
        private Factor itsFactor;
        public Factor ItsFactor
        {
            get { return itsFactor; }
            set { itsFactor = value; }
        }
        private charProperty itsProperty;
        public charProperty ItsProperty
        {
            get { return itsProperty; }
            set { itsProperty = value; }
        }
        public double Value
        {
            get { return itsValue; }
            set { itsValue = value; }
        }

        public Bonus(Factor factor, charProperty property)
        {
            ItsFactor = factor;
            ItsProperty = property;
        }

        public Bonus()
        {
            this.ItsFactor = new Factor();
            this.ItsProperty = new charProperty();
        }

        public bool isInitialized()
        {
            return (ItsProperty != null && ItsFactor!=null);
        }

        public Bonus copy(Bonus copyBonus)
        {
            copyBonus = (Bonus)base.copy(copyBonus);
            if (copyBonus == null)
                copyBonus = new Bonus();
            this.itsProperty.copy(copyBonus.itsProperty);
            this.ItsFactor.copy(copyBonus.itsFactor);
            copyBonus.Value=this.Value;
            return copyBonus;
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);

            //Create Nodes
            XmlNode bonusNode = creator.CreateElement("Bonus");
            XmlNode itsPropertyNode = ItsProperty.toXml(creator);
            XmlNode itsFactorNode = ItsFactor.toXml(creator);
            XmlNode itsValueNode = creator.CreateElement("Value");

            itsValueNode.InnerText = this.Value.ToString();

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                bonusNode.AppendChild(parentNode.ChildNodes[i].Clone());
            bonusNode.AppendChild(itsValueNode);
            bonusNode.AppendChild(itsFactorNode);
            bonusNode.AppendChild(itsPropertyNode);

            return bonusNode;
        }
        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);
            this.Value = Int32.Parse(((XmlElement)node).
                GetElementsByTagName("Value").Item(0).InnerText);
            this.itsFactor.fromXml(((XmlElement)node).GetElementsByTagName("Factor").Item(0));
            this.itsProperty.fromXml(((XmlElement)node).GetElementsByTagName("Property").Item(0));
        }
    }
}
