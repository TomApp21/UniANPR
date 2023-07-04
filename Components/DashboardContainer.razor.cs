using UniANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
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
        private PopupEditRace _thisEditRacePopupRef { get; set; }
        TelerikNotification NotificationReference { get; set; }


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
            
            bool blnSucess = _thisRaceService.ScheduleNewRace(raceToCreate);

            if (blnSucess)
            {
                ShowNotification("Race created successfully.", true);
                // Show Notification
            }
            else
            {
                 ShowNotification("Race creation failed.", false);
            }
        }

        private void HandleRaceCreationCancelled()
        { }

        

        private void HandleTrackCreationConfirmed()
        {
            string trackToCreate = _thisCreateTrackPopupRef.TrackData.TrackName;

            if (_thisRaceService.CheckIfTrackNameExists(trackToCreate))
            {
                ShowNotification("Track Name already exists in system, please enter a different one.", false);
            }
            else
            {
                bool blnSucess = _thisRaceService.AddNewTrack(trackToCreate);

           
            if (blnSucess)
            {
                ShowNotification("Track created successfully.", true);
                // Show Notification
            }
            else
            {
                 ShowNotification("Track creation failed.", false);
            }
            }
        }

        private void HandleTrackCreationCancelled()
        { }

        


      

            // service check if exists
            // Add to DAL

        #endregion

        private void ShowNotification(string notificationText, bool successTheme)
        {
            NotificationReference.Show(new NotificationModel()
            {
                Text = GetLocalisedString(notificationText),
                ThemeColor = successTheme ? Telerik.Blazor.ThemeConstants.Notification.ThemeColor.Success : ThemeConstants.Notification.ThemeColor.Error,
                ShowIcon = true,
                CloseAfter = 3000
            });
        }


        private TheosAPI thisAPI = new TheosAPI();
        protected async void TestAPI()
        {
            List<NumberPlate> detectedObjects = new List<NumberPlate>();
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Uni Test\\apitest100";

            DirectoryInfo directoryInfo = new DirectoryInfo(downloadsFolder);
            FileInfo[] files = directoryInfo.GetFiles("*.jpg"); // Change the file extension to match your image format

            if (files.Length > 0)
            {
                foreach (FileInfo file in files)
                {
                    FileInfo imageFile = file; // Assuming you want to retrieve the first image file
                    Console.WriteLine("Image file found: " + imageFile.FullName);

                    DateTime timeBeforeRequest = DateTime.Now;
                    detectedObjects = await thisAPI.Detect(imageFile, confThres: 0.5f, iouThres: 0.5f, ocrModel: "small", ocrClasses: "numberplate", retries: 5, delay: 2);
                    TimeSpan elapsedTime = (DateTime.Now - timeBeforeRequest);
                    int ms = (int)elapsedTime.TotalMilliseconds;

                    if (detectedObjects.Count == 0)
                    {
                        Console.WriteLine("No number plate found");
                        Console.WriteLine("Time in MS: " + ms.ToString());

                    }
                    else if (detectedObjects.Count == 1)
                    {
                        Console.WriteLine("Recognised Text: " + detectedObjects[0].Text);
                        Console.WriteLine("Time in MS: " + ms.ToString());

                    }
                    else if (detectedObjects.Count == 2)
                    {
                        Console.WriteLine("Recognised Text: " + detectedObjects[0].Text);
                        Console.WriteLine("Recognised Text: " + detectedObjects[1].Text);
                        Console.WriteLine("Time in MS: " + ms.ToString());
                    }
                    else if (detectedObjects.Count == 3)
                    {
                        Console.WriteLine("Recognised Text: " + detectedObjects[0].Text);
                        Console.WriteLine("Recognised Text: " + detectedObjects[1].Text);
                        Console.WriteLine("Recognised Text: " + detectedObjects[2].Text);
                        Console.WriteLine("Time in MS: " + ms.ToString());
                    }
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine(" ");
                }



                //files[0].Delete();

                // Perform further processing with the image file
            }
            else
            {
                Console.WriteLine("No image files found in the Downloads folder.");
            }
        }




    }
}
