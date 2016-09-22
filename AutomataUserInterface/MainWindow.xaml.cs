using CellularAutomata;
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

namespace AutomataUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string WatermarkText = "Type here to enter commands.";

        public InterfaceBackend CommandParser;

        ScrollViewer Scroller;

        private Stopwatch Watch = new Stopwatch();

        public MainWindow()
        {
            //TODO surround everything in a Try/Catch to easily save a stacktrace to file in case of crash
            InitializeComponent();            
            Title = "Cellular Automata Simulator  v" + typeof(MainWindow).Assembly.GetName().Version.ToString();
            Tools.OutputWindow = this;
            Scroller = GetDescendantByType(Document, typeof(ScrollViewer)) as ScrollViewer;
            ImageChangedListener.ImageField = ImageField;

            CommandParser = new InterfaceBackend(); //Init this last!
        }

        public void addSomeColoredText(string theText, Brush theColor)
        {
            TextHistory.Inlines.Add(new Run(theText) { Foreground = theColor });
            Scroller.ScrollToEnd();
        }

        private void UserInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addSomeColoredText("\n> " + UserInput.Text, Tools.PositiveColor);
                CommandParser.Run(UserInput.Text);
                UserInput.Text = "";
                e.Handled = true;
            }
        }

        public Visual GetDescendantByType(Visual element, Type type)
        {

            if (element == null) return null;
            if (element.GetType() == type) return element;

            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null) break;
            }
            return foundElement;
        }

        private void UserInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserInput.Text == WatermarkText)
            {
                UserInput.Text = string.Empty;
                UserInput.Foreground = Brushes.Black;
            }
        }

        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserInput.Text == string.Empty)
            {
                UserInput.Text = WatermarkText;
                UserInput.Foreground = Brushes.Gray;
            }
        }

        private void ImageField_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Tools.DisplayMessageLine("Saving current Automata image... ");
            Watch.Restart();
            CommandParser.CurrentAutomata.OutputAutomata();
            Watch.Stop();
            Tools.DisplayMessage("Done.");
            Tools.DisplayMessageLine($"Command took: {Watch.ElapsedMilliseconds} ms.");
        }
    }
}
