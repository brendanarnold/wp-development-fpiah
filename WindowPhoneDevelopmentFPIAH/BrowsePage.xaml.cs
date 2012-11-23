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
using System.IO.IsolatedStorage;

namespace WindowPhoneDevelopmentFPIAH
{
    public partial class BrowsePage : PhoneApplicationPage
    {
        public BrowsePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Uri uri;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LastURI"))
            {
                uri = (Uri)IsolatedStorageSettings.ApplicationSettings["LastURI"];
            }
            else
            {
                uri = new Uri("HTML/index.html", UriKind.Relative);
            }
            this.webBrowser.Navigate(uri);

        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Uri uri = this.NavStack.Peek();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LastURI"))
            {
                IsolatedStorageSettings.ApplicationSettings["LastURI"] = uri;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add("LastURI", uri);
            }
            base.OnNavigatedFrom(e);
        }


        private Stack<Uri> NavStack = new Stack<Uri>();

        private string GetFileName(string inp)
        {
            string[] els = inp.Split(new char[] { '#' });
            return els[0];    
        }

        private void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavStack.Count > 0)
            {
                Uri uri = this.NavStack.Peek();
                if (this.GetFileName(uri.ToString()) == this.GetFileName(e.Uri.ToString()))
                 return;
            }
            this.NavStack.Push(e.Uri);

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavStack.Count > 1)
            {
                this.NavStack.Pop();
                this.webBrowser.Navigate(this.NavStack.Pop());
                e.Cancel = true;
            }
            
            base.OnBackKeyPress(e);
        }

        private void appHomeAppBarMenuItem_Click(object sender, System.EventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void webBrowser_Navigating(object sender, Microsoft.Phone.Controls.NavigatingEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri)
            {
                MessageBoxResult res = MessageBox.Show("This link is outside of the app. Do you want to load it up in Internet Explorer?", "Go online?", MessageBoxButton.OKCancel);
                e.Cancel = true;
                if (res == MessageBoxResult.OK)
                {
                    WebBrowserTask wbt = new WebBrowserTask();
                    wbt.Uri = e.Uri;
                    wbt.Show();
                }
            }
        }




    }

   


}
