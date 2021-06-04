using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ConciseDesign.WPF.Message;
using ConciseDesign.WPF.UserControls;
using Demo.Annotations;

namespace Demo
{
    public enum TestState
    {
        Default,
        Active,
    }
    
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private TestState _testState;

        public TestState TestState
        {
            get => _testState;
            set
            {
                if (value == _testState) return;
                _testState = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void CircularProgressBar_OnClick(object sender, RoutedEventArgs e)
        {
            
           
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.TestState = TestState.Active;
            /*var desktopMessageService = new DesktopMessageService();
            desktopMessageService.Raise("asdf",AlertType.Message);*/
            /*var dialogHostControl = DialogRegister.GetById("MainDialog");
            var dialogTest = new DialogTest();
            var dialogViewModel = new DialogViewModel(dialogTest);
            dialogTest.DataContext = dialogViewModel;
            await dialogHostControl.RaiseDialogAsync(dialogTest);*/
            // var raiseSubmitAsync = await dialogHostControl.RaiseSubmitAsync("sadfasd");

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
