using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using UniANPR.Interfaces;
using UniANPR.Models;
using UniANPR.Models.Services;
using UniANPR.Utility;

namespace UniANPR.Components
{
  public partial class ParticipantDashboardContainer
    {
        #region Injections

        [Inject] 
        IRaceService _thisRaceService { get; set; }

        #endregion

        #region Declarations

        private PopupCreateRace _thisCreateRacePopupRef { get; set; }
        private PopupCreateTrack _thisCreateTrackPopupRef { get; set; }

        private PopupRegisterForRace _thisRegisterRacePopupRef { get; set; }
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

        protected void RegisterForARace()
        {
            _thisRegisterRacePopupRef.ShowRaceRegistrationForm();
        }

        protected void HandleParticipantRaceRegistration(string registeredNumberplate, int raceId)
        {
            bool blnSuccess = _thisRaceService.RegisterParticipantForRace(registeredNumberplate, CurrentUserId, raceId);

            if (blnSuccess)
            {
                ShowNotification("Registered for successfully - please wait for race operator to approve request.", true);
                // Show Notification
            }
            else
            {
                 ShowNotification("Registration failed.", false);
            }
        }

        protected void HandleParticipantRaceRegistrationCancelled()
        {

        }

        #endregion

        #region Event Callbacks

        private void HandleRaceCreationConfirmed()
        {
            Race_VM raceToCreate = _thisCreateRacePopupRef.RaceData;
            
            bool blnSucess = _thisRaceService.ScheduleNewRace(raceToCreate);

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
                    ShowNotification("Race creation failed.", false);
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




    }
}
