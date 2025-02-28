using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ArinaMazitova422_TrackerPet.Databases;

namespace ArinaMazitova422_TrackerPet
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
       public static TrackerPetEntities db = new TrackerPetEntities();

    }
}
