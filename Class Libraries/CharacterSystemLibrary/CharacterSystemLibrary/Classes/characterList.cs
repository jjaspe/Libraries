using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSystemLibrary.Classes
{
    public class characterList:System.Collections.Generic.LinkedList<Character>
    {
        public Character find(string name)
        {
            LinkedListNode<Character> currentNode = this.First;
            while (currentNode != null)
            {
                if (currentNode.Value.Name == name)
                    return currentNode.Value;
                else
                    currentNode = currentNode.Next;
            }
            return null;
        }
        public characterList copy()
        {
            characterList copyList = new characterList();
            LinkedListNode<Character> current = this.First;
            while (current != null)
            {
                copyList.AddLast(current.Value.copy(new Character()));
                current = current.Next;
            }
            return copyList;
        }
        public Character[] toArray()
        {
            return this.ToArray();
        }
        public void add(Character newCharacter)
        {
            if (this.Count == 0)
                this.AddFirst(newCharacter);
            else
                this.AddAfter(this.Last, newCharacter);
        }

        public XmlNode toXml(XmlDocument creator)
        {
            //Create Nodes
            XmlNode topNode = creator.CreateElement("characterList");
            Character[] tempCharacters = this.toArray();
            foreach (Character character in tempCharacters)
            {
                //Create Node
                XmlNode currentNode = character.toXml(creator);
                //Join Node
                topNode.AppendChild(currentNode);
            }

            return topNode;
        }
    }
}
