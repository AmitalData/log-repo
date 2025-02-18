using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class WCOFluent
    {
        private List<WCOErrorPointerModel> _List;
        public WCOFluent(List<WCOErrorPointerModel> list)
        {
            _List = list;
        }
        WCOFluent GetNode(WCOErrorPointerModel myElement)
        {
            _List =GetMyChildren(myElement);
            return this;
        }
        public List<WCOErrorPointerModel>  GetCopyList()
        {
            return _List.Select(rec => rec.CreateNew()).ToList(); 
        }
        public WCOErrorPointerModel GetTagID(int level, string TagId)
        {
            if(_List==null)
            {
                _List = new List<WCOErrorPointerModel>();
            }
            var elm = _List.FirstOrDefault(rec => rec.Level == level & rec.WCOID == TagId);
            if (elm != null)
            {
                return elm.CreateNew();
            }
            return null;
        }
        public WCOFluent GetNode(int level, string WCOID)
        {
            if (_List == null)
            {
                _List = new List<WCOErrorPointerModel>();
            }

            var elm = _List.FirstOrDefault(rec => rec.Level == level & rec.WCOID == WCOID);
            GetNode(elm);
            return this;
        }
        List<WCOErrorPointerModel> GetMyChildren(WCOErrorPointerModel myElement ,bool only1stDgree=false)
        {
            if (myElement == null) return null;
            if (_List == null) return null;

            var elmIndex = myElement.IndexSeq;
            var elmLevel = myElement.Level;
            var siblingElment = _List.OrderBy(rec => rec.IndexSeq).FirstOrDefault(rec => rec.Level == myElement.Level & rec.IndexSeq > myElement.IndexSeq);
            var nextSiblingElmentindex = _List.Last().IndexSeq+1;
            if (siblingElment != null)
            {
                nextSiblingElmentindex = siblingElment.IndexSeq-1;
            }
            int numOfElment = nextSiblingElmentindex - elmIndex; //-(1+1)
            var allChilds =  //_List.GetRange((elmIndex + 1), numOfElment);
                _List.Where(rec => rec.IndexSeq > elmIndex & rec.IndexSeq <= nextSiblingElmentindex).ToList();
            if (only1stDgree)
            {
                var only1stChilds = allChilds.OrderBy(rec => rec.IndexSeq).Where(rec => rec.Level == (elmLevel + 1)).ToList();
                return only1stChilds;
            }
            else
            {
                return allChilds;
            }

        }
        public override string ToString()
        {
            _List.OrderBy(rec => rec.IndexSeq).ToList().ForEach(rec =>NetCommonHelper.Logger.DevLog.Instance.WriteDebug(rec.ToString()));
            return base.ToString();
        }
    }
}
