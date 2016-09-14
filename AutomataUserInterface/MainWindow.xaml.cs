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
        public InterfaceBackend CommandParser = new InterfaceBackend();

        public MainWindow()
        {
            InitializeComponent();
            Title = "Cellular Automata Simulator  v" + typeof(MainWindow).Assembly.GetName().Version.ToString();
            Tools.OutputWindow = this;         
        }

        public void addSomeColoredText(string theText, Brush theColor)
        {
            TextHistory.Inlines.Add(new Run(theText) { Foreground = theColor });
        }

        private void UserInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addSomeColoredText("> " + UserInput.Text + "\n", Tools.PositiveColor);
                CommandParser.Run(UserInput.Text);
                UserInput.Text = "";
                e.Handled = true;
            }
        }
    }
}
