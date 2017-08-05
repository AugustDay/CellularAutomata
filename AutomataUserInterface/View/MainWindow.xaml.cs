using AutomataUserInterface.Tools;
using CellularAutomata;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            Printer.OutputWindow = this;
            Scroller = GetDescendantByType(Document, typeof(ScrollViewer)) as ScrollViewer;
            ImageTools.ImageField = ImageField;

            CommandParser = new InterfaceBackend(); //Init this last!
            AddCurrentAutomataToRecentMenu();
            CommandParser.AutomataGenerated += OnAutomataGenerated;

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
                addSomeColoredText("\n> " + UserInput.Text, Printer.PositiveColor);
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

        #region Context Menu

        private void MenuDownload_Click(object sender, RoutedEventArgs e)
        {
            Printer.DisplayMessageLine("Download option clicked.", Printer.GreenColor);
            CommandParser.Run("d"); //TODO rename to d|download
        }

        private void MenuCenterImage_Click(object sender, RoutedEventArgs e)
        {
            border.Reset();
        }

        private void MenuSimulateMore_Click(object sender, RoutedEventArgs e)
        {
            Printer.DisplayMessageLine("Simulate option clicked.", Printer.GreenColor);
            CommandParser.Run("f");
        }

        private void MenuNewRandomRule_Click(object sender, RoutedEventArgs e)
        {
            Printer.DisplayMessageLine("New Automata option clicked.", Printer.GreenColor);
            CommandParser.Run("n");
        }

        private void AddCurrentAutomataToRecentMenu()
        {
            MenuItem newAutomataMenuItem = new MenuItem();
            newAutomataMenuItem.Header = "Rule #" + CommandParser.CurrentAutomata.Rules.RuleNumber;
            newAutomataMenuItem.Click += new RoutedEventHandler(RecentAutomataMenuItem_Click);
            UncheckAllAutomataInRecentMenu();
            newAutomataMenuItem.IsChecked = true;
            MenuViewHistory.Items.Add(newAutomataMenuItem);
        }

        private void UncheckAllAutomataInRecentMenu()
        {
            foreach (MenuItem i in MenuViewHistory.Items)
            {
                i.IsChecked = false;
            }
        }

        public void OnAutomataGenerated(object source, EventArgs e)
        {
            Printer.DisplayMessageLine("history list changed.");
            MenuItem newAutomataMenuItem = new MenuItem();
            newAutomataMenuItem.Header = "Rule #" + CommandParser.CurrentAutomata.Rules.RuleNumber;
            newAutomataMenuItem.Click += new RoutedEventHandler(RecentAutomataMenuItem_Click);
            UncheckAllAutomataInRecentMenu();
            newAutomataMenuItem.IsChecked = true;
            MenuViewHistory.Items.Add(newAutomataMenuItem);
            if (InterfaceBackend.MAXIMUM_HISTORY_SIZE < MenuViewHistory.Items.Count)
            {
                MenuViewHistory.Items.RemoveAt(0);
            }
        }

        private void MenuStartingCondition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuNewSpecificRule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecentAutomataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem chosenItem = sender as MenuItem;
            UncheckAllAutomataInRecentMenu();
            int counter = 0;
            foreach (MenuItem i in MenuViewHistory.Items)
            {
                if (i == chosenItem)
                {
                    Printer.DisplayMessageLine("Clicked on: " + i.ToString());
                    i.IsChecked = true;
                    break;
                }
                counter++;
            }

            CommandParser.SetCurrentAutomata(counter);

            //MenuViewHistory.Items.RemoveAt(counter);
        }

        #endregion

        #region Menu Bar

        /// <summary>
        /// Displays information about the program, including attributions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            string version = typeof(MainWindow).Assembly.GetName().Version.ToString();
            Printer.DisplayMessageLine("Application by: Austin Ingraham");
            Printer.DisplayMessageLine("Version " + version);
            Printer.DisplayMessageLine("Some icons by Yusuke Kamiyamane, from http://p.yusukekamiyamane.com/");
            //TODO full popup about screen, with complete attribution.
        }

        /// <summary>
        /// Selects whether to color cells based on their value (state), or by their neighborhood lookup. 
        /// </summary>
        private void MenuItemColor_CellValue_Click(object sender, RoutedEventArgs e)
        {
            //TODO add mouseover on these options to display a tooltip for informing the user.
            MenuItemCellColor_Value.IsChecked = false;
            MenuItemCellColor_NeighborhoodLookup.IsChecked = false;
            MenuItem colorThemeChoice = sender as MenuItem;
            if (colorThemeChoice == MenuItemCellColor_Value)
            {
                Printer.DisplayMessageLine("Displaying cells by their value.");
                MenuItemCellColor_Value.IsChecked = true;
                CommandParser.CurrentAutomata.DisplayNeighborhoodLookup = false;
                CommandParser.CurrentAutomata.Imager.ChooseColorTheme(CommandParser.CurrentAutomata.Imager.CurrentColorThemeEnum);
                CommandParser.CurrentAutomata.RefreshDisplay();
            }
            else
            {
                Printer.DisplayMessageLine("Displaying cells by their neighborhood lookup.");
                MenuItemCellColor_NeighborhoodLookup.IsChecked = true;
                CommandParser.CurrentAutomata.Imager.ChooseColorTheme(CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.NeighborhoodLookup);
                CommandParser.CurrentAutomata.DisplayNeighborhoodLookup = true;
                CommandParser.CurrentAutomata.RefreshDisplay();
            }

            Printer.DisplayMessageLine("Unimplemented.");
            
        }

        /// <summary>
        /// Selects the color theme which the program will use to display cells. 
        /// </summary>
        private void MenuItemColorTheme_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllColorMenuItems();
            MenuItem colorThemeChoice = sender as MenuItem;
            if (colorThemeChoice == MenuItemColor_Default)
            {
                Printer.DisplayMessageLine("Default color theme chosen.");
                MenuItemColor_Default.IsChecked = true;
                CommandParser.CurrentAutomata.Imager.ChooseColorTheme(CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Default);
                CommandParser.CurrentAutomata.Imager.CurrentColorThemeEnum = CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Default;
            }
            else if (colorThemeChoice == MenuItemColor_Choice2)
            {
                Printer.DisplayMessageLine("Unimplemented, color choice 2 chosen.");
                MenuItemColor_Choice2.IsChecked = true;
                CommandParser.CurrentAutomata.Imager.ChooseColorTheme(CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Choice2);
                CommandParser.CurrentAutomata.Imager.CurrentColorThemeEnum = CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Choice2;
            }
            else if (colorThemeChoice == MenuItemColor_Choice3)
            {
                Printer.DisplayMessageLine("Unimplemented, color choice 3 chosen.");
                MenuItemColor_Choice3.IsChecked = true;
                CommandParser.CurrentAutomata.Imager.ChooseColorTheme(CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Choice3);
                CommandParser.CurrentAutomata.Imager.CurrentColorThemeEnum = CellularAutomata.OneDimensionalCA.Imager1D.ColorThemes.Choice3;
            }

            if (MenuItemCellColor_Value.IsChecked)
            {
                CommandParser.CurrentAutomata.RefreshDisplay();
            }
        }

        private void UncheckAllColorMenuItems()
        {
            MenuItemColor_Default.IsChecked = false;
            MenuItemColor_Choice2.IsChecked = false;
            MenuItemColor_Choice3.IsChecked = false;
        }


        #endregion

    }
}
