using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CloudBuh.Item;

namespace CloudBuh
{
    /// <summary>
    /// Main application class
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<TransactionListItem> _items;

        public static readonly DependencyProperty BalanceProperty;
        public string CurrentBalanceString
        {
            get => (string)GetValue(BalanceProperty);
            set => SetValue(BalanceProperty, $"Текущий баланс: {value}");
        }
        public static readonly DependencyProperty LoadingProperty;
        public bool Loading
        {
            get => (bool)GetValue(LoadingProperty);
            set => SetValue(LoadingProperty, value);
        }


        static MainWindow()
        {
            BalanceProperty = DependencyProperty.Register(
                                nameof(CurrentBalanceString),
                                typeof(string),
                                typeof(MainWindow));
            LoadingProperty = DependencyProperty.Register(
                                nameof(Loading),
                                typeof(bool),
                                typeof(MainWindow));
        }

        public MainWindow()
        {
            InitializeComponent();
            InitRestService();
            LoadContent();
        }

        private async void LoadContent()
        {
            Loading = true;
            var result = await RestService.GetTransactionsAsync();
            _items = new ObservableCollection<TransactionListItem>(result);
            TransactionListView.ItemsSource = _items;
            UpdateBalance();
            Loading = false;
        }

        private static readonly Regex _regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(SumTextBox.Text, out double transactionValue))
            {
                MessageBox.Show("Проверьте правильность введённой суммы");
                return;
            }
            if(!PlusRadio.IsChecked.Value && !MinusRadio.IsChecked.Value)
            {
                MessageBox.Show("Пожалуйста отметьте приход это или расход");
                return;
            }
            if (DescriptionTextBox.Text.Length==0)
            {
                MessageBox.Show("Пожалуйста введите краткое описание транзакции");
                return;
            }
            Loading = true;
            var result = await RestService.AddTransaction(transactionValue, DescriptionTextBox.Text, PlusRadio.IsChecked.Value, DateTime.Now);
            Loading = false;

            if (result == null)
            {
                MessageBox.Show("При загрузке обновления на сервер произошла ошибка,\nпопробуйте ещё раз");
                return;
            }

            _items.Add(new TransactionListItem(result));
            UpdateBalance();

            SumTextBox.Text = string.Empty;
            PlusRadio.IsChecked = false;
            MinusRadio.IsChecked = false;
            DescriptionTextBox.Text = string.Empty;
        }

        private void UpdateBalance()
        {
            double balance = 0;
            foreach (var item in _items)
            {
                if (item.Plus)
                    balance += item.Amount;
                else
                    balance -= item.Amount;
            }
            CurrentBalanceString = balance.ToString();
        }

        private void InitRestService()
        {
            RestService.Client.BaseAddress = new Uri("http://localhost:56460");
            RestService.Client.DefaultRequestHeaders.Accept.Clear();
            RestService.Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void DeleteItem(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            var item = fe.DataContext as TransactionListItem;
            if (item==null)
                return;
            if(await RestService.DeleteTransaction(item))
            {
                _items.Remove(item);
                UpdateBalance();
            }
        }
    }
}
