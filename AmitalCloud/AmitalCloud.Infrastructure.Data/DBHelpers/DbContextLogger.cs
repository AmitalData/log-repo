using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Data.Entity;
using System.Text;

namespace AmitalCloud.Infrastructure.Data.DBHelpers
{
    internal class DbContextLogger : IDbContextLogger
    {
        StringBuilder _StringBuilder;
        Action _DisposeMe;
        private DbContextLogger()
        {
        }
        internal DbContextLogger(Database database)
        {
            // TODO: Complete member initialization
            _StringBuilder = new StringBuilder();
            database.Log += LogMe; ;
            this._DisposeMe = ()
                =>
            {
                try
                {
                    database.Log -= LogMe;
                }
                catch
                {


                }

            };
        }

        private void LogMe(string mess)
        {
            if (_StringBuilder == null)
                _StringBuilder = new StringBuilder();
            if (mess == Environment.NewLine)
            {
                return;
            }
            _StringBuilder.AppendLine(mess);
        }


        public override string ToString()
        {
            return _StringBuilder.ToString();
        }
        public string ToString(int lastCharacter)
        {
            var s = this.ToString();
            var sLast = s.Substring(Math.Max(0, s.Length - lastCharacter));
            return sLast;
        }
        public void Dispose()
        {
            _StringBuilder = null;
            _DisposeMe();
            _DisposeMe = null;
        }




        public void AddExplainLog(string ExplainLog)
        {
            LogMe(ExplainLog);
        }
    }
}
