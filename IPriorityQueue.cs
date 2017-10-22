using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    interface IPriorityQueue<T> : IContainer<T>
                     where T : IComparable
    {
        //Inserts item base on its priority
        void Enqueue ( Event item );
    
        //Removes first item in the queue
        void Dequeue ( );

        //Query
        Event Peek ( );
    
    }
}
