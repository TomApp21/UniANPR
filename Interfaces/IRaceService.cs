using static UniANPR.Services.Race.RaceService;

namespace UniANPR.Interfaces
{
    public interface IRaceService : IHostedService
    {
        //int AddSubscriber_RaceTrack
        bool CheckIfTrackNameExists(string newTrackName);
        bool AddNewTrack(string trackName);



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

    }
}
