using ArinaMazitova422_TrackerPet.Databases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ArinaMazitova422_TrackerPet.Pages
{
    public partial class AddPostPage : Page
    {
        private byte[] _selectedImageBytes = null;
        private User _currentUser;
        private Posts _editingPost;

        public AddPostPage(User user, Posts post = null)
        {
            InitializeComponent();
            _currentUser = user;
            _editingPost = post;

            LoadPetOptions();
            LoadPostRates();

            if (_editingPost != null)
            {
                LoadPostData();
            }
        }

        private void LoadPetOptions()
        {
            if (_currentUser.IdRole == 1) 
            {
                PetComboBox.Items.Add(new { Id = 1, Name = "Ра" });
            }
            else if (_currentUser.IdRole == 2) 
            {
                PetComboBox.Items.Add(new { Id = 2, Name = "Нуби" });
            }
            PetComboBox.DisplayMemberPath = "Name";
            PetComboBox.SelectedIndex = 0; 
        }

        private void LoadPostRates()
        {
            List<string> rates = new List<string> { "Сытый", "Голодный", "Довольный", "Недовольный", "Чистый", "Грязный" };
            PostRateComboBox.ItemsSource = rates;
            PostRateComboBox.SelectedIndex = 0; 
        }

        private void LoadPostData()
        {
            DescriptionTextBox.Text = _editingPost.Description;

            if (_editingPost.Image != null && _editingPost.Image.Length > 0)
            {
                using (var ms = new MemoryStream(_editingPost.Image))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    PreviewImage.Source = bitmap;
                }
            }

            var pet = PetComboBox.Items.Cast<dynamic>().FirstOrDefault(p => p.Id == _editingPost.idPet);
            if (pet != null)
            {
                PetComboBox.SelectedItem = pet;
            }

            var postRate = App.db.PostRate.FirstOrDefault(r => r.id == _editingPost.idRate);
            if (postRate != null)
            {
                PostRateComboBox.SelectedItem = postRate.Name;
            }
        }

        private void SelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                PreviewImage.Source = bitmap;
            }
        }

        private void SavePost_Click(object sender, RoutedEventArgs e)
        {
            if (PetComboBox.SelectedItem == null || PostRateComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedPet = (dynamic)PetComboBox.SelectedItem;
            int petId = selectedPet.Id;
            string description = DescriptionTextBox.Text;
            string postRate = PostRateComboBox.SelectedItem.ToString();

            try
            {
                PostRate postRateEntry = App.db.PostRate.FirstOrDefault(r => r.Name == postRate);

                if (postRateEntry == null)
                {
                    postRateEntry = new PostRate { Name = postRate };
                    App.db.PostRate.Add(postRateEntry);
                    App.db.SaveChanges();
                }

                if (_editingPost == null)
                {
                    var newPost = new Posts
                    {
                        idPet = petId,
                        idRate = postRateEntry.id,
                        Description = description,
                        DateNTime = DateTime.Now,
                        Image = _selectedImageBytes
                    };

                    App.db.Posts.Add(newPost);
                }
                else
                {
                    _editingPost.idPet = petId;
                    _editingPost.idRate = postRateEntry.id;
                    _editingPost.Description = description;
                    _editingPost.DateNTime = DateTime.Now;

                    if (_selectedImageBytes != null) 
                    {
                        _editingPost.Image = _selectedImageBytes;
                    }
                }

                App.db.SaveChanges();
                MessageBox.Show("Запись успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
