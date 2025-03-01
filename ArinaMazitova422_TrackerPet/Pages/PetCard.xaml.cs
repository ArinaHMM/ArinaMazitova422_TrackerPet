using ArinaMazitova422_TrackerPet.Databases;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ArinaMazitova422_TrackerPet.Pages
{
    public partial class PetCard : UserControl
    {
        public string PetName { get; set; }  
        public string Rate { get; set; }    

        public Posts Posts { get; }

        public PetCard()
        {
            InitializeComponent();
        }

        public PetCard(Posts post) : this()
        {
            Posts = post;
            LoadData();
            LoadImage();
            DataContext = this; 
        }

        private void LoadData()
        {
            var pet = App.db.Pet.FirstOrDefault(p => p.id == Posts.idPet);
            PetName = pet?.Name ?? "Неизвестный питомец";

            var postRate = App.db.PostRate.FirstOrDefault(r => r.id == Posts.idRate);
            Rate = postRate?.Name ?? "Без характеристики";
        }

        private void LoadImage()
        {
            if (Posts?.Image != null && Posts.Image.Length > 0)
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
                ImagePet.Source = new BitmapImage(new Uri("pack://application:,,,/Image/cats.png"));
            }
        }
    }
}
