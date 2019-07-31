//using Microsoft.AspNet.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace Logitude.Server.Tools
//{
//    public class SignalRHubEventPublisher
//    {
//        //public static void PublishChannelEvent(LogitudeHubChannelEvent channelEvent)
//        //{
//        //    IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<LogitudeGeneralHub>();

//        //    _context.Clients.Group(channelEvent.ChannelName).OnChannelEvent(channelEvent);


//        //    //return Task.FromResult(0);
//        //}


//        //public static void PublishAllClientsEvent(LogitudeHubChannelEvent channelEvent)
//        //{
//        //    IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<LogitudeGeneralHub>();

//        //    _context.Clients.All.OnEvent(channelEvent);

//        //    //return Task.FromResult(0);
//        //}


//        public static void PublishLogitudeHubEvent(LogitudeHubChannelEvent channelEvent)
//        {

//            IHubContext _context = GlobalHost.ConnectionManager.GetHubContext("LogitudeHub");
//            //IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<LogitudeHub>();
//            if (!string.IsNullOrEmpty(channelEvent.ChannelName))
//            {
//                _context.Clients.Group(channelEvent.ChannelName).OnChannelEvent(channelEvent);
//            }
//            else
//            {
//                _context.Clients.All.OnEvent(channelEvent);
//            }

//            //return Task.FromResult(0);
//        }



//    }
//}