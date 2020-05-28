using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate
{
    public class PerformanceTimerLogger
    {
        Stopwatch stepTimer;
        Stopwatch allProccessTimer;
        StringBuilder loggBuilder;
        public PerformanceTimerLogger()
        {
            this.loggBuilder = new StringBuilder();
            this.stepTimer = new Stopwatch();
            this.allProccessTimer = new Stopwatch();

         
        }

        public void Start()
        {
            this.loggBuilder = new StringBuilder();
            this.stepTimer.Start();
            this.allProccessTimer.Start();
        }
        public void Stop()
        {
            this.loggBuilder = new StringBuilder();
            this.stepTimer.Stop();
            this.allProccessTimer.Stop();
        }
        public void LogMessage(string message)
        {
            var newLine = string.Format("{0},{1}", message, this.stepTimer.Elapsed.ToString());//
            this.loggBuilder.AppendLine(newLine);
            this.stepTimer.Restart();
            ////before your loop
            //var csv = new StringBuilder();

            ////in your loop
            //var first = reader[0].ToString();
            //var second = image.ToString();
            ////Suggestion made by KyleMit
            //var newLine = string.Format("{0},{1}", first, second);
            //csv.AppendLine(newLine);

            ////after your loop
            //File.WriteAllText(filePath, csv.ToString());
        }

        public void WriteLogToCSVFile()
        {
          

            var newLine = string.Format("{0},{1}", ",Total", this.allProccessTimer.Elapsed.ToString());
            this.loggBuilder.AppendLine(newLine);

            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"performancelog.csv");
            File.WriteAllText(filePath, this.loggBuilder.ToString());

            this.Stop();


        }
    }
}