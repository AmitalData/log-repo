#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{

    

    public class IIGMessagePM
    {

        [Flags] ///Indicates that an enumeration can be treated as a bit field; that is, a set !!!!!!!
        public enum InteractiveMode
        {
            none = 0,
            Interactive = 1,
            DCA = 2
        }

        [Flags] //Indicates that an enumeration can be treated as a bit field; that is, a set !!!
        public enum EnvironmentType
        {
            none = 0,
            Test = 1,
            Community = 2,
            Pilot = 4,
            Production = 8
        }
        [Flags] //Indicates that an enumeration can be treated as a bit field; that is, a set !!!
        public enum InOutType
        {
            none = 0,
            In = 1,
            Out = 2
        }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public InOutType InOut { get; set; }
        public string Desc { get; set; }
        public InteractiveMode Interactive { get; set; }
        public string MalamClass { get; set; }
        public string XSDName { get; set; }
        public EnvironmentType Environment { get; set; }
        public bool  ToSign { get; set; }

        public string ToSufix(EnvironmentType environment)
        {

            switch (environment)
            {
                case EnvironmentType.none:
                    break;
                case EnvironmentType.Test:
                    return ".TST.xml";
                    break;
                case EnvironmentType.Community:
                    return ".KHL.xml";
                    break;
                case      EnvironmentType.Pilot:
                    return ".PLT.xml";
                    break;
                case EnvironmentType.Production:
                    return ".PRD.xml";
                    break;
                default:
                    break;
            }
            return ".TST.xml";
        }

        public string ImporterService { get; set; }

        public string DCAServiceAddress { get; set; }



        public string ClassName { get; set; }
        string _PrefixFileName = "";

        public string PrefixFileName
        {
            get { return _PrefixFileName; }
            set { _PrefixFileName = value; }
        }
        
    }
}
#endif