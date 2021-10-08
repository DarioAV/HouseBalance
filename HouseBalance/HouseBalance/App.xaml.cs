using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HouseBalance.MainPage;

namespace HouseBalance
{
    public partial class App : Application
    {
        static SQLiteHelper db;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static SQLiteHelper SQLiteDB //Si no existiera una base de datos, esta clase la crea
        {
            get 
            {
                if (db == null)
                {
                    var path = Xamarin.Essentials.FileSystem.AppDataDirectory;
                    var file = Path.Combine(path, "HouseBalance.db3");

                    db = new SQLiteHelper(file); 
                }
                return db;
            }
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
