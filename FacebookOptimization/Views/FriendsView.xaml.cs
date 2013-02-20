using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FacebookOptimization.ViewModels;

namespace FacebookOptimization.Views
{
    /// <summary>
    /// Interaction logic for FriendsView.xaml
    /// </summary>
    public partial class FriendsView : UserControl
    {
        public FriendsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as FriendsViewModel).LoadFriends();
        }
    }
}
