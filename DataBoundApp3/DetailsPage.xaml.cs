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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace DataBoundApp3
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            DataContext = TData;
        }

        private ObservableCollection<TInfo> _tData = new ObservableCollection<TInfo>();

        public ObservableCollection<TInfo> TData
        {
            get { return _tData; }
            set { _tData = value; }
        }


        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);

                Dispatcher.BeginInvoke(() => {
                    prepareDC(index);
                });
            }
        }

        private void prepareDC(int index)
        {

            TData.Clear();

            var now = 0;
            var doc = XDocument.Load("data/tourism_2012.xml");
            foreach (var document in doc.Elements())
            {
                foreach (var tour in document.Elements())
                {
                    var item = new TInfo();
                    foreach (var data in tour.Elements())
                    {
                        if (now == index)
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
                                    item.Telephone = "+39" + data.Value.Replace("-", "");
                                    break;
                                case "fax":
                                    item.Fax = "+39" + data.Value.Replace("-", ""); ;
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
                        
                    }
                    if (now == index)
                        DataContext = item;

                    now++;
                }
            }
        }


        

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

            //call
            var ph = new PhoneCallTask();
            ph.DisplayName = (DataContext as TInfo).Name;
            ph.PhoneNumber = (DataContext as TInfo).Telephone;
            ph.Show();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            var mt = new EmailComposeTask();
            mt.To = (DataContext as TInfo).EMail;
            mt.Show();
        }
    }
}