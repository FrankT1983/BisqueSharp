using BisqueWebHelper;
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
            var metaData = file.MetaData;          
            
            var tags = BisqueXmlHelper.TagsFromXml(metaData);
            this.OutputText.Text = "Metadata for last file (" + file.ImageName + ") :\n";
            foreach (var tag in tags)
            {                
                this.OutputText.Text += tag.Item1 + " " + tag.Item2 + "\n";
            }           
        }

        private void ClickedPutMetaData(object sender, RoutedEventArgs e)
        {
            var file = this.session.GetImages().Last();            
            var xml = file.GetXmlTagDocument();          
            BisqueXmlHelper.WriteTagsToXml(xml, new Tuple<string, string>("fromCode", "Foo"));
            file.SetTagsFromDocument(xml);
        }

        private void ClickedDownload(object sender, RoutedEventArgs e)
        {
            var file = this.session.GetImages().Last();
            this.OutputText.Text = "downloading (sync) : " + file.ImageName + "\n";            
            file.Download("c:\\tmp\\");
            this.OutputText.Text += "download complette";
        }

        private void ClickedSlice(object sender, RoutedEventArgs e)
        {
            var file = this.session.GetImages().Last();
            var bytes = file.GetSlice(1,1,1,1);
        }
    }
}