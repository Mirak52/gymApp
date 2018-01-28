using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gymApp.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : TabbedPage
    {
        public MenuPage ()
        {
            InitializeComponent();
        }

        private void Hiit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HiitPage(), false);
        }
    }
}