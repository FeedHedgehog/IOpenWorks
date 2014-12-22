using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UIShell.OSGi;

namespace LULU.FrmMain
{
    internal delegate void CloseFormDelegate();

    public class Activator : IBundleActivator
    {

        private MainForm _mainForm;

        public void Start(IBundleContext context)
        {
            _mainForm = new MainForm();
            context.AddService<Form>(_mainForm);
        }

        public void Stop(IBundleContext context)
        {
            context.RemoveService<Form>(_mainForm);

            CloseFormDelegate closeFormDel = delegate()
            {
                _mainForm.Close();
            };                
        }
    }
}
