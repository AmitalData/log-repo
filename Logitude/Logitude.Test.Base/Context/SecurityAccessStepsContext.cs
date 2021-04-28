using System;

namespace Logitude.Test.Base.Context
{
    public class SecurityAccessStepsContext<T> where T : class, new()
    {
        public SecurityAccessStepsContext()
        {
            FirstUserPMData = new T();
            SecondUserPMData = new T();
        }

        public T FirstUserPMData { get; set; }

        public T SecondUserPMData { get; set; }

        public Action act;
    }
}