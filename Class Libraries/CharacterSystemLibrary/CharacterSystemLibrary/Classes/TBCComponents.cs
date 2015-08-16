using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class TBCComponents
    {
        public bool is_empty=true;

        /*************CONSTRUCTORS***********************/
        public TBCComponents(){is_empty=false;}

        public XmlNode toXml(XmlDocument creator)
        {
            XmlNode compNode = creator.CreateElement("comp");

            return compNode;
        }
        public void fromXml(XmlNode node)
        {
            XmlNodeList is_emptyNode=((XmlElement)node).
                GetElementsByTagName("Is_Empty");
            this.is_empty = is_emptyNode.Count==0?false:Boolean.Parse( is_emptyNode.Item(0).InnerText );
        }

        internal TBCComponents copy(TBCComponents comp)
        {            
            TBCComponents copyComponent=comp;
            //copyComponent.is_empty = this.is_empty;
            return copyComponent;
        }

        //CASTING
        
    }
}
