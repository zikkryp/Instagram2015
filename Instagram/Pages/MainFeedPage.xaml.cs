using Instagram.API;
using Instagram.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Instagram.API;
using Windows.UI.ViewManagement;

namespace Instagram.Pages
{
    public sealed partial class MainFeedPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public MainFeedPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            GetFeed(true);
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetFeed(true);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GetFeed(false);
        }

        private async void GetFeed(bool refresh)
        {
            progressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;

            var result = await InstagramAPI.GetFeedAsync(refresh);

            this.DefaultViewModel["Feed"] = result.Items;

            progressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var orientation = ApplicationView.GetForCurrentView().Orientation;

            if (orientation == ApplicationViewOrientation.Portrait)
            {
                Application.Current.Resources["AppWidth"] = (e.NewSize.Width - 75).ToString();

                VisualStateManager.GoToState(this, "Portrait", true);
            }
            else if (orientation == ApplicationViewOrientation.Landscape)
            {
                VisualStateManager.GoToState(this, "Landscape", true);
            }
            else
            {
                throw new Exception("Unknown orientation");
            }
        }
    }

    public class SnappedView
    {
        public SnappedView()
        {

        }

        public SnappedView(Size size)
        {
            this.Width = (int)size.Width;
        }

        public int Width { get; set; }

        public int GridWidth
        {
            get { return this.Width - 100; }
        }

        public int ImageWidth
        {
            get { return this.Width - 150; }
        }
        
        public int ProfileWidth
        {
            get { return 75; }
        }
    }
}
