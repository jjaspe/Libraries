using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Check:TBCComponents
    {
        charProperty firstProperty, secondProperty;
        Factor itsFactor; 
        int[] itsThreshholds;

        public int[] ItsThreshholds
        {
          get { return itsThreshholds; }
          set { itsThreshholds = value; }
        }

        public Check()
        {
            firstProperty = new charProperty();
            secondProperty = new charProperty();
            itsFactor = new Factor();
        }

        public Check(charProperty fProperty, charProperty sProperty, Factor factor)
        {
            this.FirstProperty = fProperty;
            this.SecondProperty = sProperty;
            this.ItsFactor = factor;
        }

        public bool isInitialized()
        {
            return (firstProperty!=null && secondProperty!=null && ItsFactor!=null);
        }

        public Check copy(Check copyCheck)
        {
            copyCheck=(Check)base.copy(copyCheck);
            this.FirstProperty.copy(copyCheck.FirstProperty);
            this.SecondProperty.copy( copyCheck.SecondProperty);
            this.ItsFactor.copy(copyCheck.ItsFactor);
            copyCheck.itsThreshholds=new int[this.itsThreshholds.Length];
            for(int i=0; i<this.itsThreshholds.Length;i++)
                copyCheck.ItsThreshholds[i]=this.ItsThreshholds[i];

            return copyCheck;
        }

        #region ACCESSORS
        public charProperty SecondProperty
        {
            get { return secondProperty; }
            set { secondProperty = value; }
        }

        public charProperty FirstProperty
        {
            get { return firstProperty; }
            set { firstProperty = value; }
        }       

        public Factor ItsFactor
        {
            get { return itsFactor; }
            set { itsFactor = value; }
        }       
        #endregion

        //Returns index of highest threshhold that is <= to roll
        public int doCheck()
        {            
            if(isInitialized())
            {
                double result=firstProperty.Value - secondProperty.Value+calculateFactor();
                for(int i=itsThreshholds.Length;i>0;i--)
                {
                    if(result>itsThreshholds[i-1] )
                        return i;
                }
                //if it never returned in for-loop, it means result is <= to first threshhold
                return 0;
            }
            return 0;//CAREFUL HERE, PROBABLY BEST TO CHECK isInitialized BEFORE CALLING doCheck()            
        }
        public double getResult()
        {
            return FirstProperty.Value - SecondProperty.Value + calculateFactor();
        }
        private double calculateFactor()
        {
            if(isInitialized())
            {
                Random rand = new Random();
                double value = 0;
                int addSubtract=1;
                if(itsFactor.getNumOfDice()<0)//Do we add or subtract roll part?
                {
                    addSubtract=-1;
                    itsFactor.setNumOfDice(itsFactor.getNumOfDice()*-1);
                }
                for (int i = 0; i < ItsFactor.getNumOfDice(); i++)
                    value += addSubtract*rand.Next(ItsFactor.getDiceType());
                value += ItsFactor.ItsConstantNum/ ItsFactor.getDenominator();

                return value;
            }
            else
                return 0;
        }

        public new XmlNode toXml(XmlDocument creator)
        {
            //Get Parent Nodes
            XmlNode parentNode = base.toXml(creator);
            //Create Nodes
            XmlNode checkNode = creator.CreateElement("Check");
            XmlNode firstPropertyNode = firstProperty.toXml(creator);
            XmlNode secondPropertyNode = secondProperty.toXml(creator);
            XmlNode itsFactorNode = ItsFactor.toXml(creator);
            XmlNode itsThreshholdsNode=creator.CreateElement("Threshholds");

            //Set Node Values
            foreach(int threshhold in itsThreshholds)
            {
                XmlElement currentThreshhold=creator.CreateElement("Threshhold");
                currentThreshhold.Value=threshhold.ToString();
                itsThreshholdsNode.AppendChild(currentThreshhold);
            }

            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                checkNode.AppendChild(parentNode.ChildNodes[i]);
            checkNode.AppendChild(firstPropertyNode);
            checkNode.AppendChild(secondPropertyNode);
            checkNode.AppendChild(itsFactorNode);
            checkNode.AppendChild(itsThreshholdsNode);

            return checkNode;
        }

        public new void fromXml(XmlNode node)
        {
            base.fromXml(node);

            XmlNode fPropertyNode = ((XmlElement)node).GetElementsByTagName("Property").Item(0);
            this.FirstProperty = new charProperty();
            this.FirstProperty.fromXml(fPropertyNode);

            XmlNode sPropertyNode = ((XmlElement)node).GetElementsByTagName("Property").Item(1);
            this.SecondProperty = new charProperty();
            this.SecondProperty.fromXml(sPropertyNode);

            XmlNode factorNode = ((XmlElement)node).GetElementsByTagName("Factor").Item(0);
            this.ItsFactor = new Factor();
            this.ItsFactor.fromXml(factorNode);

            XmlNodeList threshholdList = ((XmlElement)((XmlElement)node).
                GetElementsByTagName("Threshholds").Item(0)).GetElementsByTagName("Thressholds");
            for (int i = 0; i < threshholdList.Count; i++)
            {
                itsThreshholds[i] = Int32.Parse(threshholdList[i].InnerText);
            }
        }
    }
}
