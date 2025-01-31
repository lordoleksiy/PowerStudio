using PowerStudio.Interfaces;
using PowerStudio.Services;
using PowerStudio.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace PowerStudio
{
    /// <summary>
    /// Interaction logic for MainWindowControl.
    /// </summary>
    public partial class MainWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowControl"/> class.
        /// </summary>
        private readonly MainWindowViewModel _viewModel;
        public MainWindowControl(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            this.InitializeComponent();
            this.DataContext = _viewModel;
        }
        private void MyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                e.Handled = true;
                _viewModel.SaveCommand.Execute(null);
            }
        }
        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        //[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        //[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show(
        //        string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
        //        "MainWindow");
        //}
    }
}