using ArinaMazitova422_TrackerPet.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArinaMazitova422_TrackerPet.Pages
{
    public partial class PetsViewPage : Page
    {
        private List<Posts> _posts; // Список постов

        public PetsViewPage()
        {
            InitializeComponent();
            LoadPosts(); // Загружаем данные при инициализации страницы
        }

        // Метод для загрузки данных
        private void LoadPosts()
        {
           
            
                _posts = App.db.Posts.ToList(); // Получаем все посты из базы данных
            

            PetsWP.Children.Clear();

            foreach (var post in _posts)
            {
                var petCard = new PetCard(post); 
                PetsWP.Children.Add(petCard); 
            }
        }

        // Обработчик события для поиска
        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTb.Text.ToLower();

            // Фильтруем посты по тексту поиска
            var filteredPosts = _posts
                .Where(p => p.Description.ToLower().Contains(searchText))
                .ToList();

            // Очищаем WrapPanel и добавляем отфильтрованные карточки
            PetsWP.Children.Clear();
            foreach (var post in filteredPosts)
            {
                var petCard = new PetCard(post);
                PetsWP.Children.Add(petCard);
            }
        }

        // Обработчик события для фильтрации
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (Filter.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Фильтруем посты в зависимости от выбранного фильтра
            var filteredPosts = _posts;
            if (selectedFilter != "Все")
            {
                filteredPosts = _posts
                    .Where(p => p.Description.Contains(selectedFilter))
                    .ToList();
            }

            // Очищаем WrapPanel и добавляем отфильтрованные карточки
            PetsWP.Children.Clear();
            foreach (var post in filteredPosts)
            {
                var petCard = new PetCard(post);
                PetsWP.Children.Add(petCard);
            }
        }
    }
}