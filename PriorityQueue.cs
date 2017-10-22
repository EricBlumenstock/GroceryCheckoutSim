using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    class PriorityQueue<T> : IPriorityQueue<T>
        where T : IComparable
    {
        private Node top;

        public int Count { get; set; }

        public void Enqueue ( Event item )
        {
            if ( Count == 0 )
            {
                top = new Node ( item, null );
            }
            else
            {
                Node current = top;
                Node previous = null;

                //Search for the first node in the linked structure that is smaller in priority than item
                while ( current != null && current.Item.CompareTo ( item ) >= 0 )
                {
                    previous = current;
                    current = current.Next;
                }

                //Have found the place to insert the new node
                Node newNode = new Node ( item, current );

                // If there is a previous node, set it to link to the new node
                if ( previous != null )
                {
                    previous.Next = newNode;
                }
                else
                {
                    top = newNode;
                }
            }

            Count++; // add 1 to the number of nodes in the priority queue
        }


        public void Dequeue ( )
        {
            if ( IsEmpty ( ) )
            {
                throw new InvalidOperationException ( "Cannot remove from empty queue." );
            }
            else
            {
                Node oldNode = top;
                top = top.Next;
                Count--;
                oldNode = null; // do this so the removed node can be garbage collected
            }
        }

        //Empty priority queue
        public void Clear ( )
        {
            top = null;
        }

        //Retrieve top item on priority queue
        public Event Peek()
        {
            if ( !IsEmpty ( ) )
            {
                return top.Item;
            }
            else
            {
                throw new InvalidOperationException ( "Cannot obtain top of empty priority queue." );
            }
        }


        // Asks whether the priority queue is empty
        public bool IsEmpty()
        {
            return Count == 0;
        }


    }
}
