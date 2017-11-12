using System.Windows;
using System.Windows.Controls;

namespace Uniroma3.EmotionsDetector
{
    /// <summary>
    /// Logica di interazione per PresentationPage.xaml
    /// </summary>
    public partial class InstructionPage : Page
    {
        MainPage mainPage;

        public InstructionPage()
        {
            this.mainPage = new MainPage();
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.mainPage);
        }
    }
}