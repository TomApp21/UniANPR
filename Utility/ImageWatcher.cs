using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using UniANPR.Interfaces;

namespace UniANPR.Utility
{
    class ImageWatcher
    {
        [Inject]
        private IRaceService thisRaceService { get; set; }

        private string folderPath;
        private FileSystemWatcher watcher;
        private TheosAPI theosAPI;
        private DirectoryInfo directoryInfo;
        

        //#region Parameter declerations

        //[Parameter] 
        //public OnNewImageDetectedHandler OnNewImageDetected { get; set; }

        //#endregion


        public ImageWatcher(string folderPath)
        {
            this.folderPath = folderPath;
            theosAPI = new TheosAPI();
            directoryInfo = new DirectoryInfo(folderPath);

        }

        public void Start()
        {
            // Create a new instance of FileSystemWatcher
            watcher = new FileSystemWatcher();

            // Set the path of the folder to monitor
            watcher.Path = folderPath;

            // Filter only image files
            watcher.Filter = "*.jpg"; // You can modify the filter according to your image file types

            // Subscribe to the event handlers
            watcher.Created += OnImageCreated;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("ImageWatcher started. Waiting for new images...");
        }

        public void Stop()
        {
            if (watcher != null)
            {
                watcher.Created -= OnImageCreated;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
            }

            Console.WriteLine("ImageWatcher stopped.");
        }

        private void OnImageCreated(object sender, FileSystemEventArgs e)
        {

            FileInfo[] files = directoryInfo.GetFiles(e.Name);
            thisRaceService.AddNewTrack("s");



            Console.WriteLine($"New image detected: {e.Name}");
            // You can perform any desired action here when a new image is added to the folder
        }
    }
}
