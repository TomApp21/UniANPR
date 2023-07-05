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
  public partial class GridActiveRaces
    {
        #region Injections

        [Inject] 
        IRaceService _thisRaceService { get; set; }

        #endregion

        #region Declarations

        int lapDataSubscriberId = 0;
        private int raceBuildUpSubscriberId = 0;

        TelerikNotification NotificationReference { get; set; }

        private Race_VM RaceData { get; set; }

        public List<Lap_VM> _allLapDataNow { get; set; }

        public List<Lap_VM> currentLapData { get; set; }
        

        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            _allLapDataNow = new List<Lap_VM>();

            RaceData = new Race_VM();

            //Race_SM activeRace = _thisRaceService.GetActiveRace();

            //if (activeRace.RaceId != 0 || activeRace.RaceId != null)
            //{
            //    RaceData = new Race_VM
            //    {
            //        RaceId = activeRace.RaceId,
            //        StartTime = activeRace.StartTime,
            //        EndTime = activeRace.EndTime,
            //        Spots = activeRace.Spots,
            //        RaceName = activeRace.RaceName,
            //        RaceParticipants = ConvertParticipantServiceToView(activeRace.Participants)
            //    };
            //}

            raceBuildUpSubscriberId = _thisRaceService.AddSubscriber_ActiveRaceBuildUpDataChanged(HandleNewBuildUpRaceDataReceived);
            lapDataSubscriberId = _thisRaceService.AddSubscriber_ActiveRaceLapDataChanged(HandleNewLapDataReceived);
        }

        protected override void OnFirstRender()
        {
        }

        protected override void OnDispose()
        {
            if (raceBuildUpSubscriberId != 0)
            {
                _thisRaceService.RemoveSubscriber_ActiveRaceBuildUpDataChanged(raceBuildUpSubscriberId);
            }

            if (lapDataSubscriberId != 0)
            {
                _thisRaceService.RemoveSubscriber_ActiveRaceLapDataChanged(lapDataSubscriberId);
            }
        }

        #endregion

        #region Handle Pending Race Data Changed

        /// <summary>
        /// Published site data has changed (or set on first subscription)
        /// Create the map and layer data from teh published site details
        /// </summary>
        /// <param name="siteConfigurationData"></param>
        private void HandleNewLapDataReceived(List<Lap_SM> lapData)
        {
            InvokeAsync(() =>
            {
                _allLapDataNow = (from ld in lapData
                                  select new Lap_VM()
                                  {
                                      RaceId = ld.RaceId,
                                      LapNumber = ld.LapNumber,
                                      ParticipantId = ld.ParticipantId,
                                      TimeCrossed = ld.TimeCrossed,
                                      ParticipantName = thisThreeSCUserSessionReader.DisplayNameForUserId(ld.ParticipantId),
                                  }).ToList();

                currentLapData = _allLapDataNow.Where(x => x.TimeCrossed == null).ToList();
                PopulateRestOfLapData();

                StateHasChanged();
            });
        }

        #endregion

        #region Handle Race Build Up Data Changed

        /// <summary>
        /// Published site data has changed (or set on first subscription)
        /// Create the map and layer data from teh published site details
        /// </summary>
        /// <param name="siteConfigurationData"></param>
        private void HandleNewBuildUpRaceDataReceived(Race_SM activeRace)
        {
            InvokeAsync(() =>
            {
                if (activeRace != null)
                {
                    RaceData = new Race_VM
                    {
                        RaceId = activeRace.RaceId,
                        RaceTrackId = activeRace.RaceTrackId,
                        RaceTrackName = _thisRaceService.allTrackData.Where(x => x.TrackId == activeRace.RaceTrackId).FirstOrDefault().TrackName,
                        RequiredLaps = activeRace.RequiredLaps,
                        Spots = activeRace.Spots,
                        StartTime = activeRace.StartTime,
                        EndTime = activeRace.EndTime,
                        RaceName = activeRace.RaceName,
                        RaceStatus = (RaceStatus)activeRace.RaceStatus,
                        RegisteredParticipants = activeRace.ActiveParticipants,
                        RaceParticipants = ConvertParticipantServiceToView(activeRace.Participants)
                    };
                }


                StateHasChanged();
            });
        }

        #endregion

        private void PopulateRestOfLapData()
        {
            foreach (Lap_VM lap in currentLapData)
            {
                lap.FastestLap = _thisRaceService.CalculateFastestLapForRacer(_allLapDataNow.Where(x => x.ParticipantId == lap.ParticipantId).ToList());
                lap.LastLapTime = _thisRaceService.CalculateLastLapTime(_allLapDataNow.Where(x => x.ParticipantId == lap.ParticipantId).ToList());
            }

        }

        #region


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
