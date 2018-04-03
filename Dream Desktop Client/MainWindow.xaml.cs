using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Dream_Desktop_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            LoadGameOverview();
            LoadGameSearch();
        }

        public async void LoadGameOverview()
        {
            string response = await client.GetStringAsync("https://backend.dream.riccardoparrello.nl/games");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic result = serializer.DeserializeObject(response);
            spotlightGameImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + result["spotlight"][0]["id"] + "/image");
            spotlightGameImage.Tag = result["spotlight"][0]["id"];
            discoverGame1Image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + result["discover"][0]["id"] + "/image");
            discoverGame1Image.Tag = result["discover"][0]["id"];
            discoverGame2Image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + result["discover"][1]["id"] + "/image");
            discoverGame2Image.Tag = result["discover"][1]["id"];
            discoverGame3Image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + result["discover"][2]["id"] + "/image");
            discoverGame3Image.Tag = result["discover"][2]["id"];
            discoverGame4Image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + result["discover"][3]["id"] + "/image");
            discoverGame4Image.Tag = result["discover"][3]["id"];
        }

        public async void LoadGameSearch()
        {
            string response = await client.GetStringAsync("https://backend.dream.riccardoparrello.nl/search");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic result = serializer.DeserializeObject(response);

            int count = result.Length;

            if (count > 9) count = 9;
            for (int i = 1; i <= count; i++)
            {
                var image = (Image)FindName("store" + i + "Image");
                int id = result[i - 1]["id"];
                image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + id + "/image");
                image.Tag = id;
            }
        }

        public async void LoadGameData(int p_id)
        {
            string response = await client.GetStringAsync("https://backend.dream.riccardoparrello.nl/games/" + p_id);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic result = serializer.DeserializeObject(response);

            GameImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("https://backend.dream.riccardoparrello.nl/games/" + p_id + "/image");
            GameTitle.Text = result["title"];
            GameDescription.Text = result["description"];
            Tabs.SelectedIndex = 2;
        }

        private void GameImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id = (int)((Image)sender).Tag;
            LoadGameData(id);
        }

        private void StoreImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id;
            try { id = (int)((Image)sender).Tag; }
            catch { return; }
            LoadGameData(id);
        }
    }
}
