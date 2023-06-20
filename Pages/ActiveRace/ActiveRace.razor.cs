using UniANPR.Models.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using DemoANPR.Models.Components;

namespace UniANPR.Pages.ActiveRace
{
    [Authorize]
    public partial class ActiveRace
    {
        #region Injections

        //[Inject]
        //private IVehicleFaultAlarmsService _thisVehicleFaultAlarmsService { get; set; }

        //[Inject]
        //private IVehicleService _thisVehicleService { get; set; }

        #endregion
        #region Private declarations

        private int _vehicleId;
        private int _subscriberId;
        private List<Lap_VM> _allLapData;

        private Race_VM _raceDetails;

        #endregion

        #region Intialization
        /// <summary>
        /// Initialise static data
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {

        }

        /// <summary>
        /// Do initialisation of the page after initial user render
        /// (get first real data, attach to events etc)
        /// </summary>
        protected override void OnFirstRender()
        {

        }

        /// <summary>
        /// On component being closed, unsubscribe from the service's change event
        /// </summary>
        protected override void OnDispose()
        {

        }
        #endregion


 
        #region Handle new fault data received [SINGLE VEHICLE] 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latestFaults"></param>
        //private void VehicleFaultAndAlarmChangeDelegate(List<Race> latestFaults)
        //{
        //    InvokeAsync(() =>
        //    {
        //        _vehicleFaults = (from lf in latestFaults
        //                          orderby lf.ReceivedUTCTimestamp descending
        //                          select new VehicleFault_VM
        //                          {
        //                              ReceivedUTCTimestamp = lf.ReceivedUTCTimestamp,
        //                              FaultCode = lf.FaultCode,
        //                              FaultCodeString = ((int)lf.FaultCode).ToString(),
        //                              FaultStatus= lf.FaultStatus,
        //                              FaultStatusString = lf.FaultStatus.ToString(),
        //                              AcknowledgeRequired = lf.AcknowledgeRequired == FaultAcknowledgement.AcknowledgeRequired,
        //                          }).ToList();

        //        StateHasChanged();
        //    });
        //}

        #endregion



    }
}
