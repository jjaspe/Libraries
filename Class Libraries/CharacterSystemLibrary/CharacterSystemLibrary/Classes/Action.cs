using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    public class Action:TBCComponents
    {
        public Action()
        {
        }
        public Action copy(Action action)
        {
            Action copyAction = (Action) base.copy(action);
            base.copy(copyAction);
            return copyAction;
        }
    }
}
