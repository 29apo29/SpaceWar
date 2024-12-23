using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3
{
    internal class OutOfTheWindowException : Exception
    {
        Subject subject;

        public Subject Sub
        {
            get => this.subject;
        }
        public OutOfTheWindowException(Subject sub) 
        {
            this.subject = sub;
        }
    }
}
