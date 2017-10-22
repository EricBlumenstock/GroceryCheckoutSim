using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4
{
    class Customer
    {
     
        public double arrival {get; set; } // time customer is to enter register line
        public double processTime {get; set; } // time for customer to be processed once at end of line
        public int CustomerNumber { get; set; }

        public Customer ()
        {
            arrival = 0;
            processTime = 0;
        }

        public Customer(double arrive, double lineTime, int CustomerNum)
        {
            arrival = arrive;
            processTime = lineTime;
            CustomerNumber = CustomerNum;
        }

    }
}
