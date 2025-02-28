using ArinaMazitova422_TrackerPet.Databases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArinaMazitova422_TrackerPet.Pages
{
    public partial class PetsViewPage : Page
    {
        private ObservableCollection<Posts> _posts = new ObservableCollection<Posts>();
        private User _currentUser;

        public PetsViewPage(User user) 
        {
            InitializeComponent();
            _currentUser = user;
            LoadPosts();
        }

        private void LoadPosts()
        {
            try
            {
                if (_currentUser.IdRole == 1) 
                {
                    _posts = new ObservableCollection<Posts>(App.db.Posts.Where(p => p.idPet == 1).ToList());
                }
                else if (_currentUser.IdRole == 2) 
                {
                    _posts = new ObservableCollection<Posts>(App.db.Posts.Where(p => p.idPet == 2).ToList());
                }
                else
                {
                    _posts = new ObservableCollection<Posts>();
                }

                PetsListView.ItemsSource = _posts; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTb.Text.ToLower();

            var filteredPosts = _posts
                .Where(p => p.Description.ToLower().Contains(searchText))
                .ToList();

            PetsListView.ItemsSource = filteredPosts; 
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Filter.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedFilter = selectedItem.Content.ToString();

                if (selectedFilter == "Все")
                {
                    PetsListView.ItemsSource = _posts;
                }
                else
                {
                    // Находим id для выбранного состояния из PostRate
                    var selectedRate = App.db.PostRate.FirstOrDefault(r => r.Name == selectedFilter);
                    if (selectedRate != null)
                    {
                        var filteredPosts = _posts.Where(p => p.idRate == selectedRate.id).ToList();
                        PetsListView.ItemsSource = filteredPosts;
                    }
                    else
                    {
                        PetsListView.ItemsSource = new List<Posts>(); 
                    }
                }
            }
        }


        private void PetsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void AddPostBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPostPage(_currentUser)); 
        }

        private void EditPostBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PetsListView.SelectedItem is Posts selectedPost)
            {
                NavigationService.Navigate(new AddPostPage(_currentUser, selectedPost)); 
            }
            else
            {
                MessageBox.Show("Выберите пост для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeletePostBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PetsListView.SelectedItem is Posts selectedPost)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот пост?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    App.db.Posts.Remove(selectedPost);
                    App.db.SaveChanges();

                    _posts.Remove(selectedPost); 
                    PetsListView.Items.Refresh(); 
                }
            }
            else
            {
                MessageBox.Show("Выберите пост для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PetsListView.ItemsSource = _posts;
            LoadPosts();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
