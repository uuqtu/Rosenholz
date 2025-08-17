using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{   
    public class Node
    {
        public string Name; // method name
        public decimal Time; // time spent in method
        public List<Node> Children;

        public static List<string> PrintTree(Node tree)
        {
            List<string> elements = new List<string>();

            List<Node> firstStack = new List<Node>();
            firstStack.Add(tree);

            List<List<Node>> childListStack = new List<List<Node>>();
            childListStack.Add(firstStack);

            while (childListStack.Count > 0)
            {
                List<Node> childStack = childListStack[childListStack.Count - 1];

                if (childStack.Count == 0)
                {
                    childListStack.RemoveAt(childListStack.Count - 1);
                }
                else
                {
                    tree = childStack[0];
                    childStack.RemoveAt(0);

                    string indent = "";
                    for (int i = 0; i < childListStack.Count - 1; i++)
                    {
                        indent += (childListStack[i].Count > 0) ? "|  " : "   ";
                    }

                    if (childStack.Count > 0)
                        elements.Add("|" + indent + "├─ " + tree.Name);
                    else
                        elements.Add("|" + indent + "└─ " + tree.Name);

                    if (tree?.Children?.Count > 0)
                    {
                        childListStack.Add(new List<Node>(tree.Children));
                    }
                }
            }
            elements.Add("|");
            return elements;
        }
    }
}
