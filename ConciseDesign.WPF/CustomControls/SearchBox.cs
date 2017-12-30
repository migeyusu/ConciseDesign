using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConciseDesign.WPF.CustomControls
{
    public class SearchBox : Control
    {
        
        public ICommand SearchPressCommand  
        {
            get { return (ICommand)GetValue(SearchPressCommandProperty); }
            set { SetValue(SearchPressCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchPressCommandProperty =
            DependencyProperty.Register("SearchPressCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));



        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextChangedCommandProperty =
            DependencyProperty.Register("TextChangedCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));



        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(SearchBox), new PropertyMetadata(null));



        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));


        //Source="/ConciseDesign.WPF;component/ImageResources/Close-24.png"

        public DependencyObject SearchButtonIcon        
        {
            get { return (DependencyObject)GetValue(SearchButtonIconProperty); }
            set { SetValue(SearchButtonIconProperty, value); }
        }


        // Using a DependencyProperty as the backing store for SearchIcon.  This enables animation, styling, binding, etc...    
        public static readonly DependencyProperty SearchButtonIconProperty =
            DependencyProperty.Register("SearchButtonIcon", typeof(DependencyObject), typeof(SearchBox),
                new PropertyMetadata(null));
        
        
        static SearchBox()
        {

            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));

        }
    }
}
//            var _iconImage = new Image()
//            {
//                Source = new BitmapImage(new Uri(@"pack://application:,,,/ConciseDesign.WPF;component/ImageResources/Close-24.png")),
//                Stretch = Stretch.Uniform,
//                Opacity = 0.5d
//            };
//            SearchButtonIconProperty.OverrideMetadata(typeof(SearchBox),new FrameworkPropertyMetadata(_iconImage));