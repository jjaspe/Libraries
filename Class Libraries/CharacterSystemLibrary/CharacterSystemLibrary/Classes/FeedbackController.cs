using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    public class FeedbackController
    {
        private string myFeedback="";
        private string rollFeedback = "";
        private string effectFeedback = "";

        public string EffectFeedback
        {
            get { return effectFeedback; }
            set { effectFeedback  = value+effectFeedback; }
        }

        public string RollFeedback
        {
            get { return rollFeedback; }
            set { rollFeedback += value; }
        }
        private List<string> agentNames;
        private string currentAgent;
        static FeedbackController instance = new FeedbackController();
        
        private FeedbackController() { }

        public static FeedbackController getInstance()
        {
            return instance;
        }

        public string CurrentAgent
        {
            get { return currentAgent; }
            set { currentAgent = value; }
        }

        public List<string> AgentNames
        {
            get { return agentNames; }
            set { agentNames = value; }
        }

        public string MyFeedback
        {
            get { return myFeedback; }
            set { myFeedback = value + myFeedback; }
        }

        public void RollsStart(int numOfDice,int diceType)
        {
            RollFeedback="\nRoll Type:" + numOfDice.ToString() + "d" + diceType.ToString();
        }
        public void RollOutcome(int value)
        {
           RollFeedback = "\nDice Roll:" + value.ToString();
        }
        public void constantOutcome(double value)
        {
            RollFeedback = "\nConstant:" + value.ToString();
        }
        public void FactorOutcome(double value)
        {
            EffectFeedback = "\nRoll Outcome:" + value.ToString()+RollFeedback;
            rollFeedback = "";
        }
        public void EffectProperty(string name)
        {
            EffectFeedback = "\nChange to:" + currentAgent + " Property:" + name;
            MyFeedback = EffectFeedback;
            effectFeedback = "";
        }
        public void CheckProperty(string name)
        {
        }
        public void PropertyAtLimit(string name)
        {
            MyFeedback = "\nChange not applied because:" + currentAgent + " Property:" + name + " is at a limit";
        }
        public void BonusStart()
        {
            MyFeedback = "\nBonus to property";
        }
        public void BonusFrom(string name)
        {
            MyFeedback = "\nBonus from:" + name;
        }

        public void NewLine()
        {
            MyFeedback = "\n";
        }
    }
}
