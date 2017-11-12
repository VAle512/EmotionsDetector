using System.Windows;
using System.Windows.Controls;

namespace Uniroma3.EmotionsDetector
{
    /// <summary>
    /// Logica di interazione per PresentationPage.xaml
    /// </summary>
    public partial class PresentationPage : Page
    {
        InstructionPage istructionPage;

        public PresentationPage()
        {
            this.istructionPage = new InstructionPage();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.istructionPage);
        }
    }
}
