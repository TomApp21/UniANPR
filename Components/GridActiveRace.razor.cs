using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using UniANPR.Enum;
using UniANPR.Interfaces;
using UniANPR.Models;
using UniANPR.Models.Components;
using UniANPR.Models.Services;
using UniANPR.Services.Race;
using UniANPR.Utility;

namespace UniANPR.Components
{
  public partial class GridActiveRace
    {
        #region Injections

        [Inject] 
        IRaceService _thisRaceService { get; set; }

        #endregion

        #region Declarations

        int subscriberId = 0;

        TelerikNotification NotificationReference { get; set; }

        private Race_VM RaceData { get; set; }

        protected List<Lap_VM> _allLapData { get; set; }

        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            _allLapData = new List<Lap_VM>();

            subscriberId = _thisRaceService.AddSubscriber_PendingRaceDataChanged(HandleNewPendingRaceDataReceived);
        }

        protected override void OnFirstRender()
        {
        }

        protected override void OnDispose()
        {
            if (subscriberId != 0 )
            {
                _thisRaceService.RemoveSubscriber_PendingRaceDataChanged(subscriberId);
            }
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

                //_pendingRaceData = (from rd in pendingRaceData
                //                    select new Race_VM()
                //                    {
                //                        RaceId = rd.RaceId,
                //                        RaceTrackId = rd.RaceTrackId,
                //                        RaceTrackName = _thisRaceService.allTrackData.Where(x => x.TrackId == rd.RaceTrackId).FirstOrDefault().TrackName,
                //                        RequiredLaps = rd.RequiredLaps,
                //                        Spots = rd.Spots,
                //                        StartTime = rd.StartTime,
                //                        EndTime = rd.EndTime,
                //                        RaceName = rd.RaceName,
                //                        RaceStatus = (RaceStatus)rd.RaceStatus,
                //                        RegisteredParticipants = rd.ActiveParticipants,
                //                        RaceParticipants = ConvertParticipantServiceToView(rd.Participants)
                //                    }).ToList();

                StateHasChanged();
            });
        }

        #endregion

        #region Event Handlers

        protected void DeletePendingRace()
        {
            //_thisCreateTrackPopupRef.ShowCreateTrackForm();
        }

        #endregion

        #region Event Callbacks

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


        private List<Participant_VM> ConvertParticipantServiceToView(List<Participant_SM> participants)
        {
            List<Participant_VM> raceParticipants = new List<Participant_VM>();
            
            raceParticipants = (from p in participants
                        select new Participant_VM()
                        {
                            ParticipantId = p.ParticipantId,
                            RaceId = p.RaceId,
                            Approved = p.Approved,
                            Numberplate = p.Numberplate,
                            ParticipantName = thisThreeSCUserSessionReader.DisplayNameForUserId(p.ParticipantId),
                            ParticipantFinished = p.ParticipantFinished
                        }).ToList();

            return raceParticipants;
        }

        
    }
}
