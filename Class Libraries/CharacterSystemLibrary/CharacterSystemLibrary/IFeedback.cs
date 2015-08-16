using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterSystemLibrary
{
    public interface IFeedback
    {
        void parseFeedback(List<object> data);
    }
}
