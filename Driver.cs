using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Project4
{

    class Driver
    {
        public static Random r = new Random ( );

        [STAThread]
        static void Main ( string [ ] args )
        {
            PriorityQueue<Event> Events = new PriorityQueue<Event> ();
            Queue<Customer> Customers = new Queue<Customer> ( );
            List<Queue<Customer>> Registers = new List<Queue<Customer>> ( );
            List<Customer> allCustomers = new List<Customer> ( );
            List<double> skipAmount = new List<double> ( );

            int smallest = 0; // holds index of smallest queue
            int currentMax = 0; //holds max queue length
            //double skipAmount = 0; // how long it takes to process a customer
            int numCustomer = 600; // how many customers we expect during the store being open
            int numHours = 16; // how long the store will be open
            int numRegisters = 1; // how many registers will be open
            double expectedDuration = 6.25; // how long we expect a checkout to take - 2
            double totalForAvg = 0; // totals used for averaging
            double avgProcess = 0; // average processing time for customers
            int selection = 0; // menu selection
            int arrived = 0; // keeps track of how many customers have arrived
            int current = 0; // keeps track of how many customers there currently are in store
            int departed = 0; // keeps track of how many customers have departed the store

            Console.Title = "SuperMarket Simulation";
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            
            do
            {
                selection = 0;

                Console.Clear();
                Console.WriteLine ( "\n\n\t\tSimulation Menu" );
                Console.WriteLine ( "\t\t---------------\n" );
                Console.WriteLine ( "\t1. Set the number of customers" );
                Console.WriteLine ( "\t2. Set the number of hours of Operation" );
                Console.WriteLine ( "\t3. Set the number of registers" );
                Console.WriteLine ( "\t4. Set the expected checkout duration" );
                Console.WriteLine ( "\t5. Run the simulation" );
                Console.WriteLine ( "\t6. Exit program\n" );
                Console.Write ( "\t\tType the number of your choice from the menu: " );

                try
                {
                    selection = Convert.ToInt32 ( Console.ReadLine ( ) );
                }
                catch (Exception e)
                {
                   // do nothing
                }

                switch(selection)
                {
                    case 1:
                        Console.Clear ( );
                        Console.Write ( "\nHow many customers will be served in a day?: " );
                        numCustomer = Convert.ToInt32 ( Console.ReadLine ( ) );
                    break;


                    case 2:
                        Console.Clear ( );
                        Console.Write ( "\nHow many hours will the business be open?: " );
                        numHours = Convert.ToInt32 ( Console.ReadLine ( ) );
                    break;


                    case 3:
                        Console.Clear ( );
                        Console.Write ( "\nHow many lines are to be simulated?: " );
                        numRegisters = Convert.ToInt32 ( Console.ReadLine ( ) );
                    break;


                    case 4:
                        Console.Clear ( );
                        Console.WriteLine ( "\nWhat is the expected service time for a customer in minutes?\n" +
                                        "Example: Enter 5.5 for 5 and a half minutes (5 minutes, 30 seconds)");
                        expectedDuration = Convert.ToDouble ( Console.ReadLine ( ) );
                    break;


                    case 5:




                    #region
                    for ( int x = 0; x < numRegisters; x++ )
                    {
                        Registers.Add(new Queue<Customer>());
                    }

                    for ( int x = 0; x < Registers.Count; x++ )
                    {
                        skipAmount.Add ( 0 );
                    }

                    for ( int x = 0; x < numCustomer; x++ )
                    {
                        //random customer arrival time
                        TimeSpan OpenTime = new TimeSpan ( 0, r.Next ( numHours * 60 ), 0 );

                        //random customer process time
                        TimeSpan Process = new TimeSpan ( 0, ( int )( 2 + NegExp ( expectedDuration - 2 ) ), 0 );

                        // instantiate customers and arrival events
                        Customer cust = new Customer ( OpenTime.TotalMinutes, Process.TotalMinutes, x );

                        allCustomers.Add ( cust );
                        Events.Enqueue ( new Event ( EVENTTYPE.ENTER, OpenTime.TotalMinutes, x ) );
                        totalForAvg += cust.processTime;
                    }

                    int minutes = 0;
                    int mins = numHours * 60;

                    try
                    {
                        while ( minutes < mins || current != 0 ) // store is open && all customers have not shopped
                        {

                            if ( !Events.IsEmpty ( ) && minutes < mins ) // if not empty and store still open
                            {
                                
                                if ( Events.Peek ( ).Type == EVENTTYPE.ENTER )
                                {
                                    for ( int x = 0; x < allCustomers.Count; x++ )
                                    {
                                        if ( ( int )( allCustomers [ x ].arrival ) == minutes )
                                        {

                                            if ( Registers.Count == 1 ) // if there is only one register
                                                  Registers [ 0 ].Enqueue ( allCustomers [ x ] );
                                            else
                                            {
                                                for ( int y = 0; y < Registers.Count; y++ ) // check each register's length and enqueue in smallest
                                                {
                                                    if ( Registers [ y ].Count < Registers[smallest].Count )
                                                        smallest = y;
                                               
                                                }
                                                                                    
                                                // there are multiple registers
                                                Registers [ smallest ].Enqueue ( allCustomers [ x ] );
                                            }
                                            
                                            arrived++;
                                            current = arrived - departed;
                                            if ( Events.Count != 0 )
                                            {
                                                if ( Events.Peek ( ).Type == EVENTTYPE.ENTER )
                                                    Events.Dequeue ( );
                                            }
                                        }
                                    }
                                }
                            }

                            for ( int x = 0; x < Registers.Count; x++ )
                            {
                                if ( Registers [ x ].Count > 0 && skipAmount[x] <= 0 ) // skip if still being processed
                                {
                                    Events.Enqueue ( new Event ( EVENTTYPE.LEAVE, Registers [ x ].Peek ( ).processTime, Registers [ x ].Peek ( ).CustomerNumber ) );
                                    skipAmount[x] = Registers [ x ].Peek ( ).processTime;

                                    Registers [ x ].Dequeue ( ); // remove customer from queue since they left
                                }

                            }

                            for ( int x = 0; x < Registers.Count; x++ )
                            {
                                if ( !Events.IsEmpty ( ) ) // if not empty
                                {

                                    if ( Events.Peek ( ).Type == EVENTTYPE.LEAVE )
                                    {
                                        departed++;
                                        current = arrived - departed;
                                        Events.Dequeue ( );
                                    }
                                }
                            }

                            for ( int x = 0; x < numRegisters; x++ )
                            {
                                if (currentMax < Registers[x].Count)
                                currentMax = Registers [ x ].Count;
                            }
                                //if ( current > currentMax )
                                //    currentMax = current;

                                if ( current < 0 ) // oddball bug fix
                                    current = 0;

                            Console.Clear ( );
                            Console.WriteLine ( String.Format ( " Customers Arrived: {0}   \n Customers Departed: {1}   \n Current number of customers in line(s): {2} \n Max line queue: {3}", arrived, departed, current, currentMax ) );
                            for ( int y = 0; y < Registers.Count; y++ )
                            {
                                Console.WriteLine ( " Register " + ( y + 1 ) + ": " + Registers [ y ].Count );
                            }
                            Console.WriteLine ( "-------------" );
                            Thread.Sleep ( 15 );
                                //Console.WriteLine (" Amount of customers yet to shop: " + Events.Count );
                            minutes++;
                            for ( int x = 0; x < Registers.Count; x++ )
                            {
                                skipAmount [ x ]--;
                            }
                               // skipAmount--;
                        }
                    }
                    catch ( Exception e )
                    {
                        Console.WriteLine ( "\n\n There was an error. Please try again." + e );
                        Thread.Sleep ( 10000 );
                    }

               
                    avgProcess = totalForAvg / numCustomer;
                    current = numCustomer - departed;
                    departed = arrived;

                    Console.Clear ( );
                    Console.WriteLine(String.Format(" Customers Arrived: {0}   \n Customers Departed: {1}   \n Customers not processed because store closed: {2}  \n Average Process Time upon checking out: {3:###.00} minutes \n Max line queue: {4}", arrived, departed, current, avgProcess, currentMax));
  
                    Console.Write ( "\n\nPress ENTER to continue..." );
                    Console.ReadLine ( );

                    // clear out used variables for next run
                    avgProcess = 0;
                    currentMax = 0;
                    totalForAvg = 0;
                    minutes = 0;
                    arrived = 0;
                    departed = 0;
                    current = 0;
                    allCustomers.Clear ( );
                    Events.Clear ( );
                    Customers.Clear ( );
                    for ( int y = 0; y < Registers.Count; y++ )
                    {
                        Registers [ y ].Clear ( ); // clear all registers queue
                    }
                    Registers.Clear ( );
                    #endregion

                   







                    break;


                    case 6:
                        Console.Clear ( );
                        Console.Write ( "\n\nThank you for using this program." );
                        Thread.Sleep ( 1000 );
                    break;


                    default:
                        Console.Clear ( );
                        Console.Write ( "\n\nError: Invalid input. Try again." );
                        Thread.Sleep ( 1500 );
                    break;
                }
                    


            } while ( selection != 6 );
            
            

        }

        private static double NegExp (double ExpectedValue)
        {
            return -ExpectedValue * Math.Log (r.NextDouble (), Math.E);
        }

       
    }
}
