using Client.Localization;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Views
{
    public partial class MainView : Window
    {
        private bool _isExpanded = true;
        private AppSection _currentSection = AppSection.None;

        public MainView()
        {
            InitializeComponent();
            initializeCustomSettings();
        }

        #region приватні допоміжні методи при ініціалізації
        private void initializeCustomSettings()
        {
            setCurrentLanguageInCombobox();
            openNeededSection();
        }

        private void setCurrentLanguageInCombobox()
        {
            foreach (ComboBoxItem item in LanguageSelector.Items)
            {
                if ((string)item.Tag == LanguageSettings.Language)
                {
                    LanguageSelector.SelectedItem = item;
                    break;
                }
            }
        }

        private void openNeededSection()
        {
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastSection))
            {
                if (Enum.TryParse(Properties.Settings.Default.LastSection, out AppSection section))
                {
                    _currentSection = section;
                    switch (section)
                    {
                        case AppSection.Suppliers: 
                            Suppliers_Click(null, null);
                            break;
                        case AppSection.PrintFormats: 
                            PrintFormats_Click(null, null); 
                            break;
                        case AppSection.Warehouses:
                            Warehouses_Click(null, null);
                            break;
                    }
                }
                Properties.Settings.Default.LastSection = "";
                Properties.Settings.Default.Save();
            }
            else
            {
                Suppliers_Click(null, null);
            }
        }

        #endregion

        #region приватні методи натискання кнопок, роботи експандера, підсвітки обраного меню, випадаючого списку мови
        private void ToggleExpander_Click(object sender, MouseButtonEventArgs e)
        {
            _isExpanded = !_isExpanded;
            ReferencesPanel.Visibility = _isExpanded ? Visibility.Visible : Visibility.Collapsed;
            var transform = (ScaleTransform)ArrowFlip;
            transform.ScaleY = _isExpanded ? -1 : 1;
        }

        private void Suppliers_Click(object sender, MouseButtonEventArgs e)
        {
            _currentSection = AppSection.Suppliers;
            MainContent.Content = new SuppliersControl();
            SectionTitle.Text = Properties.Resources.MainViewSuppliersMenu;
            HighlightSelected(SuppliersItem);
        }

        private void PrintFormats_Click(object sender, MouseButtonEventArgs e)
        {
            _currentSection = AppSection.PrintFormats;
            MainContent.Content = new TextBlock
            {
                Text = Properties.Resources.MainViewPrintFormatsMenu,
                FontSize = 16,
                Margin = new Thickness(10)
            };
            SectionTitle.Text = Properties.Resources.MainViewPrintFormatsMenu;
            HighlightSelected(FormatsItem);
        }

        private void Warehouses_Click(object sender, MouseButtonEventArgs e)
        {
            _currentSection = AppSection.Warehouses;
            MainContent.Content = new TextBlock
            {
                Text = Properties.Resources.MainViewWarehousesMenu,
                FontSize = 16,
                Margin = new Thickness(10)
            };
            SectionTitle.Text = Properties.Resources.MainViewWarehousesMenu;
            HighlightSelected(WarehousesItem);
        }

        private void HighlightSelected(TextBlock selected)
        {
            var allItems = new List<TextBlock> { SuppliersItem, FormatsItem, WarehousesItem };
            foreach (var item in allItems)
            {
                item.Foreground = Brushes.Black;
                item.FontWeight = FontWeights.Normal;
            }
            selected.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            selected.FontWeight = FontWeights.Bold;
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedCulture = selectedItem.Tag.ToString();
                if (!string.IsNullOrEmpty(selectedCulture) && selectedCulture != LanguageSettings.Language)
                {
                    Properties.Settings.Default.LastSection = _currentSection.ToString();
                    Properties.Settings.Default.Save();

                    LanguageSettings.ChangeCulture(selectedCulture);
                }
            }
        }

        #endregion

        private enum AppSection
        {
            None,
            Suppliers,
            PrintFormats,
            Warehouses
        }

    }
}
