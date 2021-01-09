using Logitude.Test.Base.Models.Login;

namespace Logitude.SecurityTests.Models
{
    public  class SecurityAccessStepsContext<T>  where T : class , new()
    {
        public SecurityAccessStepsContext()
        {
            FirstUserPMData = new T();
            SecondUserPMData = new T();
        }


        public T FirstUserPMData { get; set; }
        public T SecondUserPMData { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
  }

