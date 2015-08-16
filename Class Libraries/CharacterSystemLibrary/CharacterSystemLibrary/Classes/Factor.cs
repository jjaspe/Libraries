using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Factor:TBCComponents
    {
        public bool is_constant;
        protected double itsConstantNum;
        protected int itsConstant_Den;
        protected int itsNumOfDice;
        protected int itsDiceType;

        public int DiceType
        {
            get { return itsDiceType; }
            set { itsDiceType = value; }
        }

        public double Numerator
        {
            get { return itsConstantNum; }
            set { itsConstantNum = value; }
        }

        public double ItsConstantNum
        {
            get { return Numerator; }
            set { Numerator = value; }
        }

        public int Denominator
        {
            get { return itsConstant_Den; }
            set { itsConstant_Den = value; }
        }

        public int NumberOfDice
        {
            get { return itsNumOfDice; }
            set { itsNumOfDice = value; }
        }
        

        public Factor()
        {
            this.is_constant = false;
            this.ItsConstantNum = 0;
            this.itsConstant_Den = 1;
            this.itsDiceType = 0;
            this.itsNumOfDice = 0;
        }
        public Factor(bool constant, double num, int den, int dType, int nDice)
        {
            this.is_constant = constant;
            this.ItsConstantNum = num;
            this.itsConstant_Den = den;
            this.itsDiceType = dType;
            this.itsNumOfDice = nDice;
        }
        
        public int getDenominator()
        {
            return itsConstant_Den;
        }
        public int getDiceType()
        {
            return DiceType;
        }
        public int getNumOfDice()
        {
            return itsNumOfDice;
        }
        public void setIs_Constant(bool value)
        {
            is_constant = value;
        }
        public void setNumerator(double value)
        {
            ItsConstantNum = value;
        }
        public void setDenominator(int value)
        {
            itsConstant_Den = value;
        }
        public void setNumOfDice(int value)
        {
            itsNumOfDice = value;
        }
        public void setDiceType(int value)
        {
            DiceType = value;
        }
        public void increaseConstant(double value)
        {
            ItsConstantNum += value * itsConstant_Den;
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);
            //Create Nodes
            XmlNode factorNode = creator.CreateElement("Factor");
            XmlElement isConstantNode = creator.CreateElement("Is Constant");
            XmlElement itsDiceTypeNode = creator.CreateElement("Dice Type");
            XmlElement itsConstantDenNode = creator.CreateElement("Constant Denominator");
            XmlElement ItsConstantNumNode = creator.CreateElement("Constant Numerator");
            XmlElement itsNumOfDiceNode = creator.CreateElement("Number Of Dice");

            //Set Node Values
            isConstantNode.Value = this.is_constant.ToString();
            itsDiceTypeNode.Value = this.getDiceType().ToString();
            itsConstantDenNode.Value = this.getDenominator().ToString();
            ItsConstantNumNode.Value = this.ItsConstantNum.ToString();
            itsNumOfDiceNode.Value = this.getNumOfDice().ToString();

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                factorNode.AppendChild(parentNode.ChildNodes[i].Clone());
            factorNode.AppendChild(isConstantNode);
            factorNode.AppendChild(itsDiceTypeNode);
            factorNode.AppendChild(itsConstantDenNode);
            factorNode.AppendChild(ItsConstantNumNode);
            factorNode.AppendChild(itsNumOfDiceNode);

            return factorNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

            XmlNode constantNode = ((XmlElement)node).GetElementsByTagName("Is Constant").Item(0);
            this.is_constant = Boolean.Parse(constantNode.InnerText);

            XmlNode numNode = ((XmlElement)node).GetElementsByTagName("Constant Numerator").Item(0);
            this.ItsConstantNum =double.Parse(numNode.InnerText);

            XmlNode denNode = ((XmlElement)node).GetElementsByTagName("Constant Denominator").Item(0);
            this.itsConstant_Den = int.Parse(denNode.InnerText);

            XmlNode numOfDiceNode = ((XmlElement)node).GetElementsByTagName("Number Of Dice").Item(0);
            this.itsNumOfDice = int.Parse(numOfDiceNode.InnerText);

            XmlNode diceTypeNode = ((XmlElement)node).GetElementsByTagName("Dice Type").Item(0);
            this.DiceType = int.Parse(diceTypeNode.InnerText);
        }


        public Factor copy(Factor copyFactor)
        {
            copyFactor=(Factor)base.copy(copyFactor);
            if (copyFactor == null)
                copyFactor = new Factor();
            copyFactor.itsConstant_Den = this.itsConstant_Den;
            copyFactor.ItsConstantNum = this.ItsConstantNum;
            copyFactor.itsNumOfDice = this.itsNumOfDice;
            copyFactor.DiceType = this.DiceType;
            copyFactor.is_constant = this.is_constant;
            return copyFactor;
        }
    }
}
