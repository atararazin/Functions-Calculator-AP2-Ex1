﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Ex1
{
    /// <summary>
    /// this class is a Composed mission. it has a name which is gets at the construction and then using
    /// the biulder desgin pattern creates the other missions. the biulder method is called Add(). 
    /// the class can calculate the final result using a queue.
    /// </summary>
    public class ComposedMission : IMission
    {
        private string name;
        public String Name
        {
            get { return name; }
        }

        private string type;
        public String Type
        {
            get { return type; }
        }
        public event EventHandler<double> OnCalculate;
        private Queue<Function> queueOfFuncs;//used a Queue because a composed mission is calculated in a FILO manner

        /// <summary>
        /// Constructor. creates the queue of functions and defines the 
        /// type as "Composed". gets the name as a param.
        /// </summary>
        /// <param name="name"></param>
        public ComposedMission(string name)
        {
            this.name = name;
            this.type = "Composed";
            this.queueOfFuncs = new Queue<Function>();
        }

        /// <summary>
        /// implemented method from interface IMission. 
        /// make a deep copy of the queue so that it doenst affect the actul member.
        /// then using FILO, calculates the equation.
        /// Activates the EventHandler.
        /// This method is the execute part of the Command design pattern. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>result</returns>
        public double Calculate(double value)
        {
            Queue<Function> tempQ = new Queue<Function>(this.queueOfFuncs);//deep copy of the queue
            double result = tempQ.Dequeue().Invoke(value);
            Function curr;
            while(tempQ.Count != 0)
            {
                curr = tempQ.Dequeue();
                result = curr.Invoke(result);
            }
            OnCalculate?.Invoke(this, result); //activates event handler
            return result;
        }

        /// <summary>
        /// uses the biulder design pattern to create a ComposedMission.
        /// therefore, simply adds the Function to the queue of Functions 
        /// and then returns the cureent object
        /// </summary>
        /// <param name="missionToAdd"></param>
        /// <returns>the object with the new function in its queue</returns>
        public ComposedMission Add(Function missionToAdd)
        {
            this.queueOfFuncs.Enqueue(missionToAdd);
            return this;
        }
    }
}
