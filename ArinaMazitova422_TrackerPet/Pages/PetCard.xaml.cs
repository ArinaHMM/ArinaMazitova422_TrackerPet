using ArinaMazitova422_TrackerPet.Databases;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для PetCard.xaml
    /// </summary>
    public partial class PetCard : UserControl
    {
        public Posts Posts { get; }
        public PetCard(Posts pset )
        {
            InitializeComponent();
            DataContext = pset;

            Posts = pset;

            LoadProductImage();
        }
        private void LoadProductImage()
        {
            if (Posts.Image != null && Posts.Image.Length > 0)
            {
                using (var ms = new MemoryStream(Posts.Image))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePet.Source = bitmap;
                }
            }
            else
            {
                ImagePet.Source = new BitmapImage(new Uri("C:\\Users\\222113\\source\\repos\\ArinaMazitova422_TrackerPet\\ArinaMazitova422_TrackerPet\\Image\\cats.png"));

            }
        }
    }
}
