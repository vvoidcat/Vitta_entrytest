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

namespace VittatestApp.View
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TableView), new PropertyMetadata(String.Empty));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty InsertionValueTitleProperty_1 =
            DependencyProperty.Register("InsertionValueTitle_1", typeof(string), typeof(TableView), new PropertyMetadata(String.Empty));
        public string InsertionValueTitle_1
        {
            get { return (string)GetValue(InsertionValueTitleProperty_1); }
            set { SetValue(InsertionValueTitleProperty_1, value); }
        }

        public static readonly DependencyProperty InsertionValueTitleProperty_2 =
            DependencyProperty.Register("InsertionValueTitle_2", typeof(string), typeof(TableView), new PropertyMetadata(String.Empty));
        public string InsertionValueTitle_2
        {
            get { return (string)GetValue(InsertionValueTitleProperty_2); }
            set { SetValue(InsertionValueTitleProperty_2, value); }
        }

        public static readonly DependencyProperty InsertionValueTitleProperty_3 =
            DependencyProperty.Register("InsertionValueTitle_3", typeof(string), typeof(TableView), new PropertyMetadata(String.Empty));
        public string InsertionValueTitle_3
        {
            get { return (string)GetValue(InsertionValueTitleProperty_3); }
            set { SetValue(InsertionValueTitleProperty_3, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(TableView), new PropertyMetadata(null));
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ReadonlyProperty =
            DependencyProperty.Register("IsReadonly", typeof(bool), typeof(TableView), new PropertyMetadata(null));
        public bool IsReadonly
        {
            get { return (bool)GetValue(ReadonlyProperty); }
            set { SetValue(ReadonlyProperty, value); }
        }

        public static readonly DependencyProperty IsDeletionEnabledProperty =
            DependencyProperty.Register("IsDeletionEnabled", typeof(bool), typeof(TableView), new PropertyMetadata(false));
        public bool IsDeletionEnabled
        {
            get { return (bool)GetValue(IsDeletionEnabledProperty); }
            set { SetValue(IsDeletionEnabledProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(TableView), new UIPropertyMetadata(null));
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty InsertCommandProperty =
            DependencyProperty.Register("InsertCommand", typeof(ICommand), typeof(TableView), new UIPropertyMetadata(null));
        public ICommand InsertCommand
        {
            get { return (ICommand)GetValue(InsertCommandProperty); }
            set { SetValue(InsertCommandProperty, value); }
        }

        public TableView()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e is not null && e.Column is not null && e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;
            }
        }
    }
}
