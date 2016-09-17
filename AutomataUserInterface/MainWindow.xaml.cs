using CellularAutomata;
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

namespace AutomataUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public InterfaceBackend CommandParser;

        ScrollViewer Scroller;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Cellular Automata Simulator  v" + typeof(MainWindow).Assembly.GetName().Version.ToString();
            Tools.OutputWindow = this;
            Scroller = GetDescendantByType(Document, typeof(ScrollViewer)) as ScrollViewer;
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
    }
}
