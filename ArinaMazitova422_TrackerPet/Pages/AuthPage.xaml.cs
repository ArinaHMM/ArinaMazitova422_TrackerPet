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

namespace ArinaMazitova422_TrackerPet.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginBox.Text;


            {
                var user = App.db.User.FirstOrDefault(u => u.Password == username);
                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {user.Name}!");

                    NavigationService.Navigate(new PetsViewPage(user));
                }
                else
                {
                    MessageBox.Show("Неверное имя или пароль!");
                }
            }
        }

    }
}