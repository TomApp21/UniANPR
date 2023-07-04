using UniANPR.Models.Components;
using UniANPR.Models.Services;
using UniANPR.Models.Services;
using static UniANPR.Services.Race.RaceService;

namespace UniANPR.Interfaces
{
    public interface IRaceService : IHostedService
    {
        List<Track_SM> allTrackData { get; }

        //int AddSubscriber_RaceTrack
        bool CheckIfTrackNameExists(string newTrackName);
        bool AddNewTrack(string trackName);

        bool ScheduleNewRace(Race_VM raceToCreate);

        bool ProcessParticipantAwaitingRegistration(string participantId, int raceId, bool approveRacer);
        bool RegisterParticipantForRace(string numberPlate, string participantId, int raceId);


        List<Race_SM> GetEligibleRaces(string participantId);

         /// <summary>
        /// Register a delegate to receive all changes to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="newAllLiveVehicleDataHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns></returns>
        int AddSubscriber_TrackDataChanged(TrackDataChangeDelegate newHandler);

        /// <summary>
        /// Remove specified subscriber to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="subscriberId">Id of data to remove</param>
        void RemoveSubscriber_TrackDataChanged(int subscriberId);

        /// <summary>
        /// Register a delegate to receive all changes to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="newAllLiveVehicleDataHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns></returns>
        int AddSubscriber_RaceParticipantDataChanged(RaceParticipantDataChangedDelegate newHandler);

        /// <summary>
        /// Remove specified subscriber to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="subscriberId">Id of data to remove</param>
        void RemoveSubscriber_RaceParticipantChanged(int subscriberId);


        
         /// <summary>
        /// Register a delegate to receive all changes to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="newAllLiveVehicleDataHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns></returns>
        int AddSubscriber_PendingRaceDataChanged(PendingRaceDataChangeDelegate newHandler);

        /// <summary>
        /// Remove specified subscriber to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="subscriberId">Id of data to remove</param>
        void RemoveSubscriber_PendingRaceDataChanged(int subscriberId);


        int AddSubscriber_ActiveRaceLapDataChanged(ActiveRaceLapDataChangeDelegate newHandler);


        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        void RemoveSubscriber_ActiveRaceLapDataChanged(int subscriberId);

    }
}
