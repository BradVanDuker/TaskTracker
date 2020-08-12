using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Controllers
{
    
    abstract class EventHub
    {
        readonly static List<EventHandler> eventList = new List<EventHandler>();

        static public void AddNewEvent(string eventName)
        { 
            
        }

        static public void RemoveEvent(string eventName)
        {

        }



    }
}
