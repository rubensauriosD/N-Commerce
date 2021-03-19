using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Aplicacion.Constantes
{
    public abstract class Validador : IDisposable
    {
        protected List<ItemValidable> lstValidables;

        public Validador()
        {
            lstValidables = new List<ItemValidable>();
        }

        public void Add(Control ctrl, bool obligatorio = false)
        {
            ctrl.KeyPress += KeyPress;
            ctrl.Validating += Validating;
            lstValidables.Add(new ItemValidable
                {
                    Control = ctrl,
                    Obligatorio = obligatorio
                });
        }

        public void Dispose()
            => Dispose();

        public void Remove(Control ctrl)
            => lstValidables.Remove(lstValidables.First(x => x.Control == ctrl));

        protected virtual bool Aplicar(string txt)
        {
            return true;
        }

        protected virtual void Validating(object sender, CancelEventArgs e)
        {
            if (!Aplicar(((Control)sender).Text))
                e.Cancel = true;
        }

        protected virtual void KeyPress(object sender, KeyPressEventArgs e)
        { 
        }

    }
}
