using DemoANPR.Models.Components;
using DemoANPR.Models.Services;
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

        bool ProcessParticipantAwaitingRegistration(int participantId, int raceId, bool approveRacer);


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
        int AddSubscriber_PendingRaceDataChanged(PendingRaceDataChangeDelegate newHandler);

        /// <summary>
        /// Remove specified subscriber to VehicleIdsInTaskAllocationStateData
        /// </summary>
        /// <param name="subscriberId">Id of data to remove</param>
        void RemoveSubscriber_PendingRaceDataChanged(int subscriberId);

    }
}
