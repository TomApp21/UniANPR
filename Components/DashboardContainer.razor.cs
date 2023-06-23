using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Interfaces;
using UniANPR.Models;
using UniANPR.Models.Services;
using UniANPR.Utility;

namespace UniANPR.Components
{
  public partial class DashboardContainer
    {
        #region Injections

        [Inject] 
        IRaceService _thisRaceService { get; set; }

        #endregion

        #region Declarations

        private PopupCreateRace _thisCreateRacePopupRef { get; set; }
        private PopupCreateTrack _thisCreateTrackPopupRef { get; set; }

        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
        }

        protected override void OnFirstRender()
        {
        }

        protected override void OnDispose()
        {
        }

        #endregion

        private void test()
        {

        }

        #region Event Handlers

        protected void ScheduleRace()
        {
            _thisCreateRacePopupRef.ShowCreateRaceForm();
        }

        protected void AddNewTrack()
        {
            _thisCreateTrackPopupRef.ShowCreateTrackForm();



        }




        #endregion

        #region Event Callbacks

        private void HandleRaceCreationConfirmed()
        {
            Race_VM raceToCreate = _thisCreateRacePopupRef.RaceData;

            // Add To DAL

        }

        private void HandleRaceCreationCancelled()
        {

        }

        private void HandleTrackCreationConfirmed()
        {
            string trackToCreate = _thisCreateTrackPopupRef.TrackData.TrackName;

            if (_thisRaceService.CheckIfTrackNameExists(trackToCreate))
            {
                // Alert user
                // Show Telerik Notification
            }
            else
            {
                bool blnSucess = _thisRaceService.AddNewTrack(trackToCreate);

                if (blnSucess)
                {
                    // Show Notification
                }
            }
        }

        private void HandleTrackCreationCancelled()
        {

        }


      

            // service check if exists
            // Add to DAL

        #endregion


        private TheosAPI thisAPI = new TheosAPI();
        protected async void TestAPI()
        {
            List<NumberPlate> detectedObjects = new List<NumberPlate>();
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\testANPR";

            DirectoryInfo directoryInfo = new DirectoryInfo(downloadsFolder);
            FileInfo[] files = directoryInfo.GetFiles("*.jpg"); // Change the file extension to match your image format

            if (files.Length > 0)
            {
                FileInfo imageFile = files[0]; // Assuming you want to retrieve the first image file
                Console.WriteLine("Image file found: " + imageFile.FullName);

                DateTime timeBeforeRequest = DateTime.Now;
                detectedObjects = await thisAPI.Detect(imageFile, confThres: 0.25f, iouThres: 0.45f, ocrModel: "medium", ocrClasses: "numberplate", retries: 5, delay: 2);
                TimeSpan elapsedTime = (DateTime.Now - timeBeforeRequest);
                int ms = (int)elapsedTime.TotalMilliseconds;

                // Perform further processing with the image file
            }
            else
            {
                Console.WriteLine("No image files found in the Downloads folder.");
            }
        }




    }
}
