using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    class Node
    {
        public Event Item { get; set; }
        public Node Next { get; set; }

        public Node (Event value, Node link)
        {
            Item = value;
            Next = link;
        }
    }
}
