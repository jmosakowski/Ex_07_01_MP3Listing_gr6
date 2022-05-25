using System;           // needed for AppContext
using System.IO;        // needed for File
using System.Windows;   // needed for RoutedEventArgs, Window
using Microsoft.Win32;  // needed for OpenFileDialog

namespace Ex_07_01_MP3Listing_gr6
{
    // Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**********************************************************************/

        // This method runs after clicking "LoadPath" in Menu -> File -> Load/Save
        private void MenuFileLoadPath_Click(object sender, RoutedEventArgs e)
        {
            // Load a file with the MP3 folder path (copy text from a file to the TextBox)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppContext.BaseDirectory; // or @"C:\Windows\"
            openFileDialog.Filter = "txt files (*.txt)|*.txt|all files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
                TextPath.Text = File.ReadAllText(openFileDialog.FileName);
        }

        /**********************************************************************/

        // This method runs after clicking "SavePath" in Menu -> File -> Load/Save
        private void MenuFileSavePath_Click(object sender, RoutedEventArgs e)
        {
            // Create a file with the MP3 folder path (save text from the TextBox to a file)
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = AppContext.BaseDirectory; // or @"C:\Windows\"
            saveFileDialog.FileName = "path";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|all files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            
            if (result == true)
                File.WriteAllText(saveFileDialog.FileName, TextPath.Text);
        }

        /**********************************************************************/

        // This method runs after clicking "Exit" in Menu -> File
        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            // Prepare setting for a Message Box
            string text = "The program will be closed.";
            string windowName = "Exit";
            var buttons = MessageBoxButton.OKCancel;
            var iconInside = MessageBoxImage.Information;
            
            // Open a Message Box with the given settings
            var whatWasClicked = MessageBox.Show(text, windowName, buttons, iconInside);
            if (whatWasClicked == MessageBoxResult.OK)
                Application.Current.Shutdown();
            // else do nothing - the exit window will close itself automatically
        }

        /**********************************************************************/

        // This method runs after clicking "About" in Menu -> Info
        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MP3 Listing using WPF\nJacek Mosakowski, WSEI Krakow");
        }

        /**********************************************************************/

        // This method runs after clicking the "Create my mp3 list!" button
        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            // Load the path from the TextBox, adding "\" at the end for safety
            string dir = TextPath.Text;
            if (dir[dir.Length - 1] != '\\') // or (dir[^1] != '\\') since C# 8.0
                dir += '\\';
            
            // Include subfolders or not
            SearchOption searchOpt = (CheckBoxSubfolders.IsChecked == true) ?
                SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            
            // Get an array with names of all files ending with ".mp3"
            string[] allMP3Files = Directory.GetFiles(dir, "*.mp3", searchOpt);

            // Save the names array to a file
            string outputFile = dir + "mp3_list.txt";
            using StreamWriter writer = new StreamWriter(outputFile);
            foreach (string file in allMP3Files)
                writer.WriteLine(file);

            // Inform the user about the result
            MessageBox.Show("Done!", "Status info");
        }
    }
}
