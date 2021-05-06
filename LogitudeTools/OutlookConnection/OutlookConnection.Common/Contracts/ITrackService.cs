using System;

namespace OutlookConnection.Common.Contracts
{
    public interface ITrackService
    {
        TrackResult ConnectImmediately(string EntryID, ITrackWorkSheetResultM myTrackWorkSheetResultM);
        TrackResult DisConnectImmediately(string EntryID);

        TrackResult DeleteMail(string EntryID);
        System.Threading.Tasks.Task<TrackResult> ConnectImmediatelyAsync(string EntryID, ITrackWorkSheetResultM myTrackWorkSheetResultM);
    }
    public class TrackResult
    {
        public bool Success { get; set; }
        public bool IsEmail { get; set; }
        public String ErrorMessage { get; set; }
    }
}
