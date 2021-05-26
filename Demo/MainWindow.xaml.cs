using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ConciseDesign.WPF.UserControls;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CircularProgressBar_OnClick(object sender, RoutedEventArgs e)
        {
            
           
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dialogHostControl = DialogRegister.GetById("MainDialog");
            var dialogTest = new DialogTest();
            var dialogViewModel = new DialogViewModel(dialogTest);
            dialogTest.DataContext = dialogViewModel;
            await dialogHostControl.RaiseDialogAsync(dialogTest);
            // var raiseSubmitAsync = await dialogHostControl.RaiseSubmitAsync("sadfasd");

        }
    }
}
