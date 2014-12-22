using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;

namespace UIShell.SimpleWinFormShellPlugin
{
    public class Activator : IBundleActivator
    {
        public static IBundleContext Context { private set; get;}

        public void Start(IBundleContext context)
        {
            Context = context;
        }

        public void Stop(IBundleContext context)
        {
            Context = null;
        }
    }
}
