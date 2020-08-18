using System;
using System.Collections.Generic;
using System.Text;

namespace Scratchpaper
{
    class Controller
    {
        public event EventHandler QuitEventHandler;

        protected void OnQuitApp()
        {
            // ... do something ...

            // can only be invoked in this class
            QuitEventHandler?.Invoke(this, EventArgs.Empty); 
        }

        protected void QuitEventResponse(object sender, EventArgs args)
        {
            // ... graceful shutdown ...
        }

        // If I did this then other classes would have to know about 
        //QuitEventHandler AND RaiseQuitEvent, which feels redundant.
        public void RaiseQuitEvent(object sender, EventArgs args)
        {
            OnQuitApp();
        }
    }

    class SomeUserInterface
    {
        Controller controller;
        public SomeUserInterface(Controller controller)
        {
            this.controller = controller;
            controller.QuitEventHandler += QuitEventResponse;
        }

        // The call to quit could come from somewhere else
        protected void QuitEventResponse(object sender, EventArgs args)
        {
            // ... graceful shutdown ...
        }

        protected void UserWantsToQuit()
        {
            // This needs to tell everyone to quit.

            // I want to do something like this but it's not allowed
            //controller.QuitEventHandler?.Invoke(this, EventArgs.Empty); 
        }
    }
}
