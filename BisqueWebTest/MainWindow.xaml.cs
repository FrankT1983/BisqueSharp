using System;
using System.Linq;
using System.Windows;

namespace BisqueWebTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BisqueSession session;

        public MainWindow()
        {
            InitializeComponent();
            this.session = new BisqueSession("http://bisquedemo");
        }
                  
        private void Button_Click2(object sender, RoutedEventArgs e)
        {                                                    
        }            

        private void ClickedLogin(object sender, RoutedEventArgs e)
        {
            var success = this.session.Login();            
            this.OutputText.Text = "Login was succsess full : " + success;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OutputText.Text = "Images:\n";           
            foreach ( var image in this.session.GetImages())
            {
                this.OutputText.Text += image.ImageName + " :  " + image.URI + "\n ";           
            }
        }

        private void ClickedUpload(object sender, RoutedEventArgs e)
        {
            this.session.Upload("C:\\tmp\\Convalaria.jpg");
        }

        private void ClickedGetMetaData(object sender, RoutedEventArgs e)
        {
            var file = this.session.GetImages().Last();
            this.session.GetEmbeddedMetaData(file);

            var metaData = this.session.GetMetaData(file);
            var tags = BisqueXmlHelper.TagsFromXml(metaData);

            this.OutputText.Text = "Images:\n";
            foreach (var tag in tags)
            {                
                this.OutputText.Text += tag.Item1 + " " + tag.Item2 + "\n";
            }           
        }

        private void ClickedPutMetaData(object sender, RoutedEventArgs e)
        {
            var file = this.session.GetImages().Last();

            var xml = this.session.GetXmlDocumentForTagManupulation(file);
            BisqueXmlHelper.WriteTagsToXml(xml, new Tuple<string, string>("fromCode", "Foo"));
            this.session.SetMetaData(file, BisqueXmlHelper.XmlDocToString(xml) );            
        }      
    }
}