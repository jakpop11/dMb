using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using dMb.Models;



namespace dMb.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCheckBox : CheckBox
    {
        public State State { get; set; } = State.UNCHECKED;


        public CustomCheckBox()
        {
            InitializeComponent();
        }
    }
}