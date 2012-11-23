using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace WindowPhoneDevelopmentFPIAH
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.goToGuideBtn.IsEnabled = false;
            // Check to see if new data loaded
            Model.UpdateHtml.HtmlUpdated += new EventHandler(UpdateHtml_HtmlUpdated);
            Model.UpdateHtml.PerformHTMLUpdate();
            

        }

        void UpdateHtml_HtmlUpdated(object sender, EventArgs e)
        {
            this.goToGuideBtn.IsEnabled = true;
        }

        private void goToGuideBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrowsePage.xaml", UriKind.Relative));
        }


        private void emailBtn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.To = "brendanarnold@outlook.com";
            ect.Show();
        }

        


 


    }
}