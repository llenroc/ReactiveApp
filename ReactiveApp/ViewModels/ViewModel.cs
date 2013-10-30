using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    public class ViewModel : ReactiveObject, IActivate, IDeactivate, IClose, IViewAware
    {
        #region IActivate

        public bool IsActive
        {
            get { throw new NotImplementedException(); }
        }

        public void Activate()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDeactivate

        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IClose

        public bool CanClose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IViewAware

        public void AttachView(object view)
        {
            throw new NotImplementedException();
        }

        public object GetView()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
