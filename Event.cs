using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    enum EVENTTYPE {ENTER = 1, LEAVE = 0};
    class Event :IComparable
    {
        public EVENTTYPE Type { get; set; }
        public double Time { get; set; }
        public int Customer { get; set; }

        public Event ()
        {
            Type = EVENTTYPE.ENTER;
            Time = 0.0;
            Customer = -1;

        }

        public Event (EVENTTYPE type, double time, int customer)
        {
            Type = type;
            Time = time;
            Customer = customer;
        }

        public int CompareTo (Object obj)
        {
            if ( !( obj is Event ) )
                throw new ArgumentException ( "The argument is not an Event object." );

            Event e = ( Event )obj;
            return ( e.Type.CompareTo(Type));
        }
    }
}
