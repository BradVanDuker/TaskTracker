using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolutionInterfaceLibrary
{
    
    public class EventHub 
    {
        // events
        public EventHandler QuitEvent { get; set; }

        static protected EventHub _eventHub = null;
        protected EventHub() { }
        static public EventHub GetInstance()
        {
            return _eventHub ?? new EventHub();
        }
    }
}
