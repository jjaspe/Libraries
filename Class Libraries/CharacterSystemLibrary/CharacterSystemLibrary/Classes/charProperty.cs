using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    /* Remember that properties can go above or below max,min with bonuses*/
    public class charProperty:TBCComponents
    {
        private string itsName;
        private double itsValue;
        private int itsMin, itsMax;
        private bonusList itsBonuses;

        public bonusList ItsBonuses
        {
            get { return itsBonuses; }
            set { itsBonuses = value; }
        }

        public charProperty()
        {
            initialize();
        }
        public charProperty(string name, double value)
        {
            initialize(name, value);
        }
        public charProperty(string name, double value,int bonus,int max,int min)
        {
            initialize(name, value, bonus, max, min);
        }
        public virtual charProperty getThis()
        {
            return this;
        }

        public void initialize(string name="", double value=0.0f, int bonus=0, int max=1000, int min=0)
        {
            this.Name = name;
            this.ItsBonuses=new bonusList();
            this.ItsMin = min;
            this.ItsMax = max;
            this.BaseValue = value;
        }        

        public int ItsMax
        {
            get { return itsMax; }
            set { itsMax = value; }
        }

        public int ItsMin
        {
            get { return itsMin; }
            set { itsMin = value; }
        }

        public double Value
        {
            get { return calculateValue(); }
        }

        public double BaseValue
        {
            get {return itsValue;}
            set { itsValue = Math.Max(Math.Min(value,itsMax),ItsMin); }
        }

        public string Name
        {
            get { return itsName; }
            set { itsName = value; }
        }
        public virtual charProperty copy()
        {
            return this.copy(new charProperty());
        }

        public charProperty copy(charProperty prop)
        {
            if (prop == null)
                prop = new charProperty();
            charProperty newChar=(charProperty)base.copy(prop);
            copyProperty(newChar);
            return newChar;
        }
        internal charProperty copyProperty(charProperty copyProperty)
        {
            base.copy(copyProperty);
            if (copyProperty == null)
                copyProperty = new charProperty();
            copyProperty.Name = this.Name;
            copyProperty.ItsMax = this.ItsMax;
            copyProperty.ItsMin = this.ItsMin;
            copyProperty.BaseValue = this.BaseValue;
            copyProperty.itsBonuses = this.itsBonuses.copy();
           
            return new charProperty();
        }

        public bool increaseValue(double value)
        {
            if (value + itsValue > ItsMax)
                return false;
            else
            {
                BaseValue = value + itsValue;
                return true;
            }
        }
        public virtual void decreaseValue(double value)
        {
            BaseValue = Math.Max(itsValue - value, ItsMin);
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);

            XmlNode propertyNode = creator.CreateElement("Property");
            XmlElement NameNode = creator.CreateElement("Name");
            XmlElement BaseValueNode = creator.CreateElement("BaseValue");
            XmlElement BonusesNode = creator.CreateElement("Bonuses");
            XmlElement MaxNode = creator.CreateElement("Max");
            XmlElement MinNode = creator.CreateElement("Min");

            //Set Node Values
            NameNode.InnerText = this.Name;
            BaseValueNode.InnerText = this.BaseValue.ToString();

            XmlNodeList bonusNodes = this.itsBonuses.toXml(creator).ChildNodes;
            foreach (XmlNode node in bonusNodes)
                BonusesNode.AppendChild(node.Clone());

            MaxNode.InnerText = this.ItsMax.ToString();
            MinNode.InnerText = this.itsMin.ToString();

            //Join Nodes
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                propertyNode.AppendChild(parentNode.ChildNodes[i].Clone());
            propertyNode.AppendChild(NameNode);
            propertyNode.AppendChild(BaseValueNode);
            propertyNode.AppendChild(BonusesNode);
            propertyNode.AppendChild(MaxNode);
            propertyNode.AppendChild(MinNode);

            return propertyNode;
        }
        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);
            this.itsName = ((XmlElement)node).
                GetElementsByTagName("Name").Item(0).InnerText;
            this.BaseValue = Int32.Parse(((XmlElement)node).
                GetElementsByTagName("BaseValue").Item(0).InnerText);

            this.ItsMax = ((XmlElement)node).
                GetElementsByTagName("Max").Count==0?(int)this.BaseValue:Int32.Parse(((XmlElement)node).
                GetElementsByTagName("Max").Item(0).InnerText);
            this.ItsMin = ((XmlElement)node).
                GetElementsByTagName("Min").Count == 0 ? 0 : Int32.Parse(((XmlElement)node).
                GetElementsByTagName("Min").Item(0).InnerText);
            

            XmlNodeList bonusList =((XmlElement)node).
                GetElementsByTagName("Bonuses").Count==0?null: ((XmlElement)((XmlElement)node).
               GetElementsByTagName("Bonuses").Item(0)).GetElementsByTagName("Bonus");
            Bonus currentBonus;
            if (bonusList != null)
            {
                foreach (XmlNode bonusNode in bonusList)
                {
                    currentBonus = new Bonus();
                    currentBonus.fromXml(bonusNode);
                    itsBonuses.addOrSet(currentBonus);
                }
            }
        }

        private double calculateValue()
        {
            double temp = 0;
            foreach (Bonus bonus in ItsBonuses)
            {
                if (bonus != null)
                    temp += bonus.Value;
            }
            return itsValue + temp;
        }
    }
}
