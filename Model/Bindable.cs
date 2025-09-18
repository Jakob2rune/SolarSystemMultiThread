using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemMultiThread.Model
{
    /// <summary>
    /// A base class that implements INotifyPropertyChanged to support data binding.
    /// Very Important, for the MVVM pattern
    /// </summary>
    public class Bindable:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void propertyIsChanged([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
