using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Coefficient : Factor
    {
        /**************MEMBERS****************/
       
        private string variableName;// Holds the name of the variable this

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }
        //coefficient will be multiplied by, empty if none

        /***************CONSTRUCTORS**********/
        public Coefficient()
        {
            is_constant = true;
            ItsConstantNum = 0;
            Denominator = 1;
            NumberOfDice = 0;
            DiceType = 0;
        }
        public Coefficient copy(Coefficient coef)
        {
            Coefficient copyCoefficient = (Coefficient)base.copy(coef);
            copyCoefficient.is_constant = this.is_constant;
            copyCoefficient.Denominator = this.Denominator;
            copyCoefficient.ItsConstantNum = this.ItsConstantNum;
            copyCoefficient.NumberOfDice = this.NumberOfDice;
            copyCoefficient.DiceType = this.DiceType;

            return copyCoefficient;
        }

        /* If coefficient is set to constant (is_constant=true), return 
         * constant alone. Else return the sum of a random value for each
         * dice ( with max given by DiceType, plus a constant part given 
         * by constant */
        public int getValue(){
            if(is_constant)
                return (int)(ItsConstantNum/Denominator);
            else{
                int tempSum=0;
                for(int i=0;i<NumberOfDice;i++)
                    tempSum+=Globals.RandomNumber(0,DiceType);
                return (tempSum + (int)(ItsConstantNum/Denominator));
            }
        }

        /* ********************/
        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);
            //Create Nodes
            XmlNode coefNode = creator.CreateElement("Coefficient");
            XmlElement variableNameNode = creator.CreateElement("variable_Name");

            //Set Node Values
            variableNameNode.Value = this.VariableName;

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                coefNode.AppendChild(parentNode.ChildNodes[i].Clone());
            coefNode.AppendChild(variableNameNode);

            return coefNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

            XmlNode variableNameNode = ((XmlElement)node).GetElementsByTagName("Variable_Name").Item(0);
            this.variableName = variableNameNode.InnerText;
        }
    }
}


