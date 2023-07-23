using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.IO;
using dMb.Services;
using System.Diagnostics;

namespace dMb
{
    public partial class App : Application
    {
        public static string DbName
        {
            get => Path.GetFileName(LocalPath);
        }

        public static string LocalPath
        {
            get => Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "localDataBase.db3");
        }

        static string rootPath
        {
            get => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "databases");
        }


        /*
         * [WIN] ApplicationData: C:\Users\jakpop11\AppData\Roaming
         * [AND] ApplicationData: /data/user/0/com.companyname.dmb/files/.config
         * [WIN] LocalApplicationData: C:\Users\jakpop11\AppData\Local\Packages\5425c65a-7be5-4e00-ad45-1c8a9e953a42_q5exf0whbv3wr\LocalState
         * [AND] LocalApplicationData: /data/user/0/com.companyname.dmb/files/.local/share
         * [WIN] CommonApplicationData: C:\Users\jakpop11\AppData\Local\Packages\5425c65a-7be5-4e00-ad45-1c8a9e953a42_q5exf0whbv3wr\LocalState\ProgramData
         * [AND] CommonApplicationData: /usr/share
         */


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

            Directory.CreateDirectory(rootPath);
            //string filePath = Path.Combine(rootPath, "file.txt");
            Debug.WriteLine("\n===========================");
            Debug.WriteLine($"Root path: {rootPath}");
            Debug.WriteLine($"Db Name: {DbName}");
            //Debug.WriteLine($"File path: {filePath}");
            Debug.WriteLine("===========================\n");
            //File.WriteAllText(filePath, "Hello");
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
