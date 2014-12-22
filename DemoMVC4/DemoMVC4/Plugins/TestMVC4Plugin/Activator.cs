using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;

namespace TestMVC4Plugin
{
    public class Activator : IBundleActivator
    {
        public static IBundle Bundle { get; private set; }
        public void Start(IBundleContext context)
        {
            Bundle = context.Bundle;
        }

        public void Stop(IBundleContext context)
        {

        }
    }
}
