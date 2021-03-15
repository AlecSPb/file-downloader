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

namespace FileDownloader
{
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

        //получение расширения для только что загруженного файла
        public string FileExtension()
        {
            string[] words = CreateFileName().Split('.');
            string extension = "." + words[words.Length - 1];
            return extension;
        }

        //создание пути иконки
        public string CreateThumbToUse(string extension, string fullname)
        {
            var thumbToUse = "";

            switch (extension)
            {
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".jpeg":
                case ".gif":
                    thumbToUse = fullname; break;
                case ".txt":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\txt.png"; break;
                case ".doc":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\doc.png"; break;
                case ".rtf":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\rtf.png"; break;
                case ".pdf":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\pdf.png"; break;
                case ".mp4":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\mp4.png"; break;
                case ".avi":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\avi.png"; break;
                case ".mp3":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\mp3.png"; break;
                case ".css":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\css.png"; break;
                case ".dbf":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\dbf.png"; break;
                case ".dwg":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\dwg.png"; break;
                case ".exe":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\exe.png"; break;
                case ".fla":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\fla.png"; break;
                case ".html":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\html.png"; break;
                case ".iso":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\iso.png"; break;
                case ".js":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\javascript.png"; break;
                case ".json":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\json-file.png"; break;
                case ".ppt":
                case ".pptx":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\ppt.png"; break;
                case ".psd":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\psd.png"; break;
                case ".xls":
                case ".xlsm":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\xls.png"; break;
                case ".xml":
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\xml.png"; break;
                default:
                    thumbToUse = @"C:\Users\vandr\Desktop\FileDownloader\Images\file.png"; break;
            }
            return thumbToUse;
        }

        //добавление загруженного файла в коллекцию всех файлов папки 
        public void AddDownloadedFile()
        {

            FileCollection.Add(new DownloadedFile()
            {
                filePath = filePath + CreateFileName(),
                fileName = CreateFileName(),
                fileThumbnail = CreateThumbToUse(FileExtension(), filePath + CreateFileName())
            });
        }

        //создание коллекции из файлов, которые уже находятся в папке
        public void ProcessDownloadedFiles()
        {
            var Folder = new DirectoryInfo(filePath.Remove(filePath.Length-1));
            var downloadedFiles = Folder.GetFiles("*");

            foreach (var file in downloadedFiles)
            {
                FileCollection.Add(new DownloadedFile()
                {
                    filePath = file.FullName,
                    fileName = file.Name,
                    fileThumbnail = CreateThumbToUse(file.Extension, file.FullName)
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
