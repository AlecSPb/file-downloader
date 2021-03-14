using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Net;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Linq;

namespace FileDownloader
{
    public interface IDownloaded
    {
        string fileName { get; set; }
        string fileThumbnail { get; set; }
        string filePath { get; set; }
    }

    public class DownloadedFile : IDownloaded
    {
        public string fileName { get; set; }
        public string fileThumbnail { get; set; }
        public string filePath { get; set; }
    }
    public partial class MainWindow : Window
    {
        public ObservableCollection<DownloadedFile> FileCollection = new ObservableCollection< DownloadedFile>();

        public MainWindow()
        {
            InitializeComponent();

            ProcessDownloadedFiles();
            ListBoxFiles.ItemsSource = FileCollection;
        }

        //путь к папке, в которую сохраняются все файлы"
        private string filePath = @"C:\Users\vandr\Desktop\FileDownloader\Downloaded\";

        //скачивание файла
        private void downloading()
        {

            WebClient webload = new WebClient();

            webload.DownloadFileAsync(new Uri($"{TextBoxLink.Text}"), filePath + CreateFileName());

            if (IsDownloaded() == true)
            {
                MessageBox.Show("Файл успешно загружён");

            } 
            else MessageBox.Show("Файл не загружён");

            AddDownloadedFile();
        }

        //проверка, скачан ли файл
        private bool IsDownloaded()
        {
            return File.Exists(filePath + CreateFileName());
        }

        //создание имена файла исходя из ссылки на него
        private string CreateFileName()
        {
            string tempName = "";
            char[] array = TextBoxLink.Text.ToCharArray();
            Array.Reverse(array);
            foreach (char ch in array)
            {
                if (ch == '/')
                {
                    break;
                }
                else
                {
                    tempName += ch;
                }
            }
            array = tempName.ToCharArray();
            Array.Reverse(array);
            string fileName = "";
            foreach (char ch in array)
            {
                fileName += ch;
            }
            return fileName;
        }

        //добавление загруженного файла в коллекцию всех файлов папки 
        public void AddDownloadedFile()
        {
            var thumbToUse = "";

            if (CreateFileName().EndsWith(".jpg") ||
                CreateFileName().EndsWith(".png") ||
                CreateFileName().EndsWith(".bmp") ||
                CreateFileName().EndsWith(".gif"))
            {
                thumbToUse = filePath + CreateFileName();
            }
            else if (CreateFileName().EndsWith(".txt"))
            {
                thumbToUse = "Images/notepad.jpg";
            }
            else
            {
                thumbToUse = "Images/doc_unknown.png";
            }

            FileCollection.Add(new DownloadedFile()
            {
                filePath = filePath + CreateFileName(),
                fileName = CreateFileName(),
                fileThumbnail = thumbToUse
            });
        }

        //создание коллекции из файлов, которые уже находятся в папке
        public void ProcessDownloadedFiles()
        {
            var Folder = new DirectoryInfo(filePath.Remove(filePath.Length-1));
            var downloadedFiles = Folder.GetFiles("*");

            foreach (var file in downloadedFiles)
            {
                var thumbToUse = "";

                if (file.FullName.EndsWith(".jpg") ||
                    file.FullName.EndsWith(".png") ||
                    file.FullName.EndsWith(".bmp") ||
                    file.FullName.EndsWith(".gif"))
                {
                    thumbToUse = file.FullName;
                }
                else if (file.FullName.EndsWith(".txt"))
                {
                    thumbToUse = "Images/notepad.jpg";
                }
                else
                {
                    thumbToUse = "Images/doc_unknown.png";
                }

                FileCollection.Add(new DownloadedFile()
                {
                    filePath = file.FullName,
                    fileName = file.Name,
                    fileThumbnail = thumbToUse
                });
            }
        }

        //открытие файла с помощью двойного нажатия
        public void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                StackPanel temp = (StackPanel)sender;
                DownloadedFile newfile = (DownloadedFile)temp.DataContext;
                Process.Start(newfile.filePath);
            }
        }

        //кнопка "Скачать"
        private void Button_Download(object sender, RoutedEventArgs e)
        {
            try
            {
                downloading();
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так.");
            }
        }

        private void ListBoxFiles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
