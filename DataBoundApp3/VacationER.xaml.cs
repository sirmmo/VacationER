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
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;

namespace DataBoundApp3
{
    public partial class VacationER : PhoneApplicationPage
    {
        public VacationER()
        {
             InitializeComponent();
            DataContext = TData;
            Dispatcher.BeginInvoke(() => {
                 prepareDC();
            });

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        
           ObservableCollection<TInfo> ts = new ObservableCollection<TInfo>();

        public ObservableCollection<TInfo> TData
        {
            get { return ts; }
            set { ts = value; }
        }
        // Constructor
       
           

        private void prepareDC()
        {

            TData.Clear();

            var doc = XDocument.Load("data/tourism_2012.xml");
            foreach (var document in doc.Elements())
            {
                foreach (var tour in document.Elements())
                {
                    var item = new TInfo();
                    foreach (var data in tour.Elements())
                    {
                        switch (data.Name.LocalName)
                        {
                            case "anno":
                                item.Year = data.Value;
                                break;
                            case "denominazione":
                                item.Name = data.Value;
                                break;
                            case "indirizzo":
                                item.Address = data.Value;
                                break;
                            case "comune":
                                item.City = data.Value;
                                break;
                            case "telefono":
                                item.Telephone = "+39"+ data.Value.Replace("-","");
                                break;
                            case "fax":
                                item.Fax = "+39"+ data.Value.Replace("-","");;
                                break;
                            case "ISTATcomune":
                                item.ISTAT = data.Value;
                                break;
                            case "tipologia":
                                item.Type = data.Value;
                                break;

                            case "email":
                                item.EMail = data.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    TData.Add(item);
                }
            }


        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("App by MMo.IT\nData by Regione Emilia Romagna (dati.emilia-romagna.it)", "About", MessageBoxButton.OK);
        }
    }
}