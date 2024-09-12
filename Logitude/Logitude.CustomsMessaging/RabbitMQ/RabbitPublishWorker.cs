using Logitude.Server.Tools;
using Logitude.Server.Tools.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public static class RabbitPublishWorker
    {
        const int _TimeoutInMS = 10000;
        private static readonly ConcurrentQueue<RabbitQueue> _ConcurrentQueue;
        private static readonly ConcurrentDictionary<string, RabbitQueue> _ConcurrentDone;

        private static IConnection _connection=null;
        private static string _LastError;
        private static int _TimeoutX2;
        private static int _TimeoutFail;
        private static int _TimeoutMore5SEC;
        private static int _TimeoutMore1SEC;
        private static int _TimeoutMoreHalfSEC;
        private static int _TimeoutHalfSEC;
        private static DateTime _LastReportAt= DateTime.Now;
        private static int _TotalBasicPublish;

        static RabbitPublishWorker()
        {
            _ConcurrentQueue = new ConcurrentQueue<RabbitQueue>();
            _ConcurrentDone = new ConcurrentDictionary<string, RabbitQueue>();

            bool test = false;
            if (test)
            {
                var my = new RabbitQueue();
                _ConcurrentDone.AddOrUpdate(
                                my.MyGuid, my,
                                (keyToUpdate, existingValue) =>
                                {
                                    return my;
                                });
                
            }
            



           var thread = new Thread(() =>
            {
                Worker();
            });


            thread.Start();
            Thread.Sleep(10);

        }

        private static void Worker()
        {
            IModel channel = null;
            int errCount = 0;
            DateTime lastWorkAt = DateTime.Now;
            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {
                
                try
                {
                    RabbitQueue rabbitQueue=null;
                    if (!_ConcurrentQueue.TryDequeue(out rabbitQueue))
                    {
                        Thread.Sleep(500);
                        CleanDoneQueue();
                        StatisticReport();
                        
                    }
                    else
                    {
                        if (DateTime.Now.Subtract(rabbitQueue.CreateAt) > TimeSpan.FromMilliseconds(2 * _TimeoutInMS))
                        {
                            _TimeoutX2++;
                            Debug.WriteLine("Dequeue timeout - enqueue without work");
                            continue;
                        }
                        if (channel?.IsClosed== true)
                        {
                            try
                            {
                                channel?.Dispose();
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
                            finally
                            {
                                channel = null;
                                Thread.Sleep(10);
                            }
                            
                        }
                        if (channel==null)
                        {
                            channel = GetConnection().CreateModel();
                        }
                        
                        try
                        {

                            _TotalBasicPublish ++;
                            RabbitPublishService.BasicPublish(
message: rabbitQueue.message,
communicationLogId: rabbitQueue.communicationLogId,
InterfaceTypeCode: rabbitQueue.InterfaceTypeCode,
 rabbitMQCode: rabbitQueue.rabbitMQCode,
 messagePriority: rabbitQueue.messagePriority,
 channel: channel

  );

                            _ConcurrentDone.AddOrUpdate(
                                rabbitQueue.MyGuid, rabbitQueue,
                                (keyToUpdate, existingValue) =>
                                {
                                    return rabbitQueue;
                                });
                                
                        }
                        catch(Exception ex1)   
                        {
                            rabbitQueue.Error = ex1.Message;
                            _ConcurrentQueue.Enqueue(rabbitQueue);
                            throw;
                        }
                        
                    }
                    errCount = 0;
                }
                catch (Exception e)
                {
                    _LastError = e.Message;
                    errCount++;
                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                    if (errCount > 20)
                    {
                        errCount = 20;
                    }
                    Thread.Sleep(errCount * 100);
                }

                Thread.Sleep(50);
            }
        }

        private static void StatisticReport()
        {
            if (DateTime.Now.Subtract(_LastReportAt)> TimeSpan.FromMinutes(5))
            {
                _LastReportAt = DateTime.Now;

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                    $"_TotalBasicPublish:{_TotalBasicPublish},_TimeoutHalfSEC:{_TimeoutHalfSEC},_TimeoutMoreHalfSEC{_TimeoutMoreHalfSEC},_TimeoutMore1SEC:{_TimeoutMore1SEC},_TimeoutMore5SEC:{_TimeoutMore5SEC},_TimeoutFail:{_TimeoutFail},_TimeoutX2:{_TimeoutX2}"
                    );

                _TotalBasicPublish = _TimeoutHalfSEC =
                    _TimeoutMoreHalfSEC = _TimeoutMore1SEC = 
                    _TimeoutMore5SEC = _TimeoutFail =_TimeoutX2 = 0;
            }
            
        }

        private static void CleanDoneQueue()
        {
            foreach (var item in _ConcurrentDone.ToList())
            {
                if (DateTime.Now.Subtract(item.Value.CreateAt) > TimeSpan.FromMinutes(5))
                {
                    _ConcurrentDone.TryRemove(item.Value.MyGuid, out _);
                }
            }
        }

        private static IConnection GetConnection()
        {


            if (_connection == null || _connection?.IsOpen != true)
            {
                try
                {
                    _connection?.Dispose();
                }
                catch
                {


                }

                var factory = RabbitmqHelper.GetConnectionFactory(true);
                _connection = factory.CreateConnection();

            }
            return _connection;



        }
        public static void DoOne(RabbitQueue rabbitQueue, int timeOutMill= _TimeoutInMS)
        {
            _ConcurrentQueue.Enqueue(rabbitQueue);
            var sw=Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeOutMill)
            {
                if (_ConcurrentDone.ContainsKey(rabbitQueue.MyGuid))
                {
                    _ConcurrentDone.TryRemove(rabbitQueue.MyGuid, out _);

                    if (sw.ElapsedMilliseconds > TimeSpan.FromSeconds(5).TotalMilliseconds)
                    {
                        _TimeoutMore5SEC++;
                    }
                    else if (sw.ElapsedMilliseconds > TimeSpan.FromSeconds(1).TotalMilliseconds)
                    {
                        _TimeoutMore1SEC++;
                    }
                    else if (sw.ElapsedMilliseconds > TimeSpan.FromSeconds(.5).TotalMilliseconds)
                    {
                        _TimeoutMoreHalfSEC++;
                    }
                    else
                    {
                        _TimeoutHalfSEC++;
                    }


                    return;
                }
                Thread.Sleep(200);
            }
            if (_ConcurrentDone.ContainsKey(rabbitQueue.MyGuid))
            {
                _ConcurrentDone.TryRemove(rabbitQueue.MyGuid, out _);
                return ;
            }
            _TimeoutFail++;
            Thread.Sleep(100);
            var n = _ConcurrentQueue.FirstOrDefault(r => r.MyGuid == rabbitQueue.MyGuid);
            throw new Exception($"RabbitPublishWorker timeout {timeOutMill} error:  {n?.Error?? _LastError}" );
            //return false;
        }
    }
    public class RabbitQueue
    {

        public RabbitQueue()
        {
            this.MyGuid=Guid.NewGuid().ToString();
            this.CreateAt = DateTime.Now;
        }
        public byte[] message { get; set; }


        public string communicationLogId { get; set; }

        public string InterfaceTypeCode { get; set; }

        public string rabbitMQCode { get; set; }

        public int messagePriority { get; set; }
        public string MyGuid { get; }
        public DateTime CreateAt { get; }

        public string Error { get; set; }
    }
}
