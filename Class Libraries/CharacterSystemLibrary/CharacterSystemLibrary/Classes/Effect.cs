using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class Effect:TBCComponents
    {
        charProperty itsAffectedProperty;
        Factor itsFactor;
        Bonus itsBonus;
        string feedback;
        FeedbackController myFeedbackController;
        public IFeedback myFeedbackBehavior;

        public Effect()
        {
            MyFeedbackController = FeedbackController.getInstance();
        }
        public FeedbackController MyFeedbackController
        {
            get { return myFeedbackController; }
            set { myFeedbackController = value; }
        }

        public string myFeedback
        {
            get { return feedback; }
            set { feedback = value; }
        }
        
        public Bonus ItsBonus
        {
            get { return itsBonus; }
            set { itsBonus = value; }
        }

        public Factor ItsFactor
        {
            get { return itsFactor; }
            set { itsFactor = value; }
        }

        public charProperty ItsAffectedProperty
        {
            get { return itsAffectedProperty; }
            set { itsAffectedProperty = value; }
        }

        public bool isInitialized()
        {
            return (ItsAffectedProperty != null);
        }

        public Effect copy(Effect copyEffect)
        {
            copyEffect=(Effect)base.copy(copyEffect);
            if (copyEffect == null)
                copyEffect = new Effect();
            this.itsAffectedProperty.copy(copyEffect.itsAffectedProperty);
            this.ItsFactor.copy(copyEffect.ItsFactor);
            this.ItsBonus.copy(copyEffect.ItsBonus);
            return copyEffect;
        }

        //Increase affected property by "value" amount
        public virtual void doEffect()
        {
            List<object> feedbackStrings = new List<object>();
            if (isInitialized())
            {
                //Factor bonusFactor=new BonusFactor;
                double value;
                if (ItsFactor != null)
                {
                    value = calculateFactor(ItsFactor, true);
                    feedbackStrings.Add("Factor Value:" + value);
                    myFeedbackController.NewLine();
                    if (itsAffectedProperty.increaseValue(value) && itsAffectedProperty.Name != "")
                    {
                        MyFeedbackController.EffectProperty(itsAffectedProperty.Name);
                        myFeedback = itsAffectedProperty.Name + " changed by " + value.ToString();
                    }
                    else
                    {
                        if (itsAffectedProperty.Name == "")
                            myFeedback = "No Change";
                        else
                        {
                            feedbackStrings.Add("Property At Limit:");
                            MyFeedbackController.PropertyAtLimit(itsAffectedProperty.Name);
                            myFeedback = itsAffectedProperty.Name + " No Change";
                        }
                    }
                }
                if (ItsBonus != null)
                {
                    //Get value of bonus                    
                    value = calculateFactor(ItsBonus.ItsFactor, false);
                    feedbackStrings.Add("Bonus Value:" + value);
                    itsBonus.Value = value;
                    if (value != 0)
                    {
                        MyFeedbackController.BonusStart();
                        MyFeedbackController.BonusFrom(ItsBonus.ItsProperty.Name);
                    }
                    //Update bonus in property
                    myFeedback += " Bonus from:" + ItsBonus.ItsProperty.Name + " set at " + value.ToString();
                    this.ItsAffectedProperty.ItsBonuses.addOrSet(ItsBonus);
                }

            }
            else
            {
                feedbackStrings.Add("Effect Not Initialized");
            }
            if (myFeedbackBehavior != null)
                myFeedbackBehavior.parseFeedback(feedbackStrings);
        }

        protected double calculateFactor(Factor fac,bool updateFeedback)
        {
            if (isInitialized())
            {
                Random rand = new Random();
                double value = 0, constantPart = 0;
                int roll = 0;

                //Used to determine whether property will be increased or decreased
                int addSubtract =fac.getNumOfDice()<0?-1:1;

                if (updateFeedback)
                    myFeedbackController.RollsStart(fac.getNumOfDice(), fac.getDiceType());
                for (int i = 0; i < (fac.getNumOfDice() * addSubtract); i++)
                {
                    roll = addSubtract * rand.Next(fac.getDiceType());
                    if(updateFeedback)
                        MyFeedbackController.RollOutcome(roll);
                    value += roll;
                }

                constantPart=fac.ItsConstantNum / fac.getDenominator();
                if(updateFeedback)
                    MyFeedbackController.constantOutcome(constantPart);
                value +=constantPart ;

                if(updateFeedback)
                    MyFeedbackController.FactorOutcome(value);
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
            XmlNode effectNode = creator.CreateElement("Effect");
            XmlNode affectedPropertyNode = ItsAffectedProperty.toXml(creator);
            XmlNode itsFactorNode = ItsFactor.toXml(creator);


            /* Join Nodes, starting with parent nodes */
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                effectNode.AppendChild(parentNode.ChildNodes[i].Clone());
            effectNode.AppendChild(itsFactorNode);
            effectNode.AppendChild(affectedPropertyNode);

            return effectNode;
        }
    }
}
