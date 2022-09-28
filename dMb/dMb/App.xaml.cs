using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.IO;
using dMb.Services;



namespace dMb
{
    public partial class App : Application
    {
        public static string LocalPath
        {
            get => Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "localDataBase.db3");
        }



        // TO DeLeTe
        //public static string LocalPath
        //{
        //    get
        //    {
        //        switch (Device.RuntimePlatform)
        //        {
        //            case Device.Android:
        //                return "/storage/emulated/0/Documents/test.txt";
        //            case Device.UWP:
        //                return AppContext.BaseDirectory;
        //            default:
        //                return null;
        //        }
        //    }
        //    /*
        //     * LocalApplicationData - not visible
        //     * ApplicationData - not visible
        //     * CommonApplicationData - don't work
        //     * ProgramFiles - don't work
        //     * MyDocuments - not visible
        //     * UserProfile - not visible
        //     * Personal + '\\'... - da fuk?
        //     * InternetCache - nope
        //     * 
        //     * ? leave it
        //     */
        //}

        static MovieDatabase moviedatabase;

        public static MovieDatabase MovieDatabase
        {
            get
            {
                if (moviedatabase == null)
                {
                    moviedatabase = new MovieDatabase("Here should be a path of the database file.");
                }
                return moviedatabase;
            }
        }

        static SQLDataBase database;

        public static SQLDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLDataBase(LocalPath);
                }
                return database;
            }
        }


        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
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
