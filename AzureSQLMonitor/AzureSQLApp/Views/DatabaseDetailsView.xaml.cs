﻿using System;
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
using AzureSQLApp.Common;
using AzureSQLApp.ViewModels;
using AzureSQLApp.Models;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AzureSQLApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DatabaseDetailsView : Page
    {
        Databases selectedDatabase = new Databases();

        private NavigationHelper navigationHelper;
        DatabaseDetailsViewModel _databasedetails = new DatabaseDetailsViewModel();



        DispatcherTimer DispatchTimer;


        public void DispactcherTimeSetup()
        {
            DispatchTimer = new DispatcherTimer();
            DispatchTimer.Tick += DispatchTimer_Tick;
            DispatchTimer.Interval = new TimeSpan(0, 0, 5);
            DispatchTimer.Start();

        }


        async void DispatchTimer_Tick(object sender, object e)
        {
            await DatabaseDetails.GetConnectionCount(selectedDatabase.DatabaseName);
            await DatabaseDetails.GetResourceUsage(selectedDatabase.DatabaseName);
        }


            public DatabaseDetailsViewModel DatabaseDetails
            {
                get { return _databasedetails; }
            }

        public DatabaseDetailsView()
        {
            this.InitializeComponent();
               this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

          private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            selectedDatabase = (Databases)e.NavigationParameter;
            
            await DatabaseDetails.GetTablesCommand(selectedDatabase.DatabaseName);
            await DatabaseDetails.GetConnectionCount(selectedDatabase.DatabaseName);
             
              
              DispactcherTimeSetup();

        }


        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void ResourceDetails_Click(object sender, RoutedEventArgs e)
        {
            BasicDetailsGrid.Visibility = Visibility.Collapsed;
            ResourceUsageGrid.Visibility = Visibility.Visible;
            await DatabaseDetails.GetResourceUsage(selectedDatabase.DatabaseName);
            DispactcherTimeSetup();
        }

        private void BasicDetails_Click(object sender, RoutedEventArgs e)
        {
            ResourceUsageGrid.Visibility = Visibility.Collapsed;
            BasicDetailsGrid.Visibility = Visibility.Visible;
        }

        
    }
}
