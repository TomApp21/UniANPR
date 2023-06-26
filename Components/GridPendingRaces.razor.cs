using DemoANPR.Models.Components;
using DemoANPR.Models.Services;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using UniANPR.Enum;
using UniANPR.Interfaces;
using UniANPR.Models;
using UniANPR.Models.Services;
using UniANPR.Utility;

namespace UniANPR.Components
{
  public partial class GridPendingRaces
    {
        #region Injections

        [Inject] 
        IRaceService _thisRaceService { get; set; }

        #endregion

        #region Declarations

        int subscriberId = 0;

        private PopupCreateRace _thisCreateRacePopupRef { get; set; }
        private PopupCreateTrack _thisCreateTrackPopupRef { get; set; }
        TelerikNotification NotificationReference { get; set; }

        private List<Race_VM> _pendingRaceData { get; set; }

        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            subscriberId = _thisRaceService.AddSubscriber_PendingRaceDataChanged(HandleNewPendingRaceDataReceived);
        }

        protected override void OnFirstRender()
        {
        }

        protected override void OnDispose()
        {
        }

        #endregion

        #region Handle Pending Race Data Changed

        /// <summary>
        /// Published site data has changed (or set on first subscription)
        /// Create the map and layer data from teh published site details
        /// </summary>
        /// <param name="siteConfigurationData"></param>
        private void HandleNewPendingRaceDataReceived(List<Race_SM> pendingRaceData)
        {
            InvokeAsync(() =>
            {
                
                _pendingRaceData = (from rd in pendingRaceData
                                     select new Race_VM()
                                     {
                                         RaceId = rd.RaceId,
                                         RaceTrackId = rd.RaceTrackId,
                                         RaceTrackName = _thisRaceService.allTrackData.Where(x => x.TrackId == rd.RaceTrackId).FirstOrDefault().TrackName,
                                         RequiredLaps = rd.RequiredLaps,
                                         Spots = rd.Spots,
                                         StartTime = rd.StartTime,
                                         EndTime = rd.EndTime,
                                         RaceName = rd.RaceName,
                                         RaceStatus = (RaceStatus)rd.RaceStatus,
                                         RegisteredParticipants = rd.ActiveParticipants,
                                     }).ToList();

                StateHasChanged();
            });
        }

        #endregion

        #region Event Handlers

        protected void EditPendingRace()
        {
            _thisCreateRacePopupRef.ShowCreateRaceForm();
        }

        protected void DeletePendingRace()
        {
            //_thisCreateTrackPopupRef.ShowCreateTrackForm();
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
