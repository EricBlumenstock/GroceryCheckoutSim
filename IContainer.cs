using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    interface IContainer<T>
    {
        //Remove all objects from the container
        void Clear ( );
        //Returns true if container is empty
        bool IsEmpty ( );
        //Returns the number of entries in the container
        int Count { get; set; }

    }
}
