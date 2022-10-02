
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CFIFILEMRepository //: IRepository<CFIFILEM>
    {
        private AmitalContext currentContext;
        public CFIFILEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIFILEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public int UpdateLOGITUDE_FILE(int tenant, long fileNo ,string myLOGITUDE_FILE)
        {


        //https://www.devart.com/dotconnect/oracle/docs/Parameters.html
            var openReaderSingleResult = new OpenReaderSingleResult(this.currentContext);
            string UserId = openReaderSingleResult.GetSchemaUserId(tenant);
            var res1 = openReaderSingleResult.ExecuteReaderSingleResult<int>(
                //$"update  {UserId}.CFIFILEM set  LOGITUDE_FILE ='{myLOGITUDE_FILE}' where FILE_NO={fileNo}"
                $"update  {UserId}.CFIFILEM set  LOGITUDE_FILE =:p1 where FILE_NO=:p2",
                (cmd) =>
                {
                 //   new List<Devart.Data.Oracle.OracleParameter>() {
                 //    new Devart.Data.Oracle.OracleParameter("p1", myLOGITUDE_FILE),
                 //    new Devart.Data.Oracle.OracleParameter("p2",fileNo)
                 //}
                    var p1=cmd.CreateParameter();
                    p1.ParameterName = "p1";p1.DbType = System.Data.DbType.String;
                    p1.Value = myLOGITUDE_FILE;

                    var p2 = cmd.CreateParameter();
                    p2.ParameterName = "p2"; p2.DbType = System.Data.DbType.Double;
                    p2.Value = fileNo;


                }
, //new List<object> {myLOGITUDE_FILE,fileNo },
                (dataReader) =>
                {
                    Int32? val = null;
                    val = dataReader.GetInt32(0);
                    return val;

                });
            //logBoxDocuments = res1.GetValueOrDefault();
            return res1.GetValueOrDefault();

        }
        //public CFIFILEM GetSingle(string FILENO, long CUSTOMFILE)
        //{
        //    return (from a in context.CFIFILEMs
        //            where a.FILENO == FILENO && a.CUSTOMFILE == CUSTOMFILE
        //            select a).FirstOrDefault();
        //}

        //public IQueryable<CFIFILEM> GetAll()
        //{
        //    return from a in context.CFIFILEMs
        //           select a;
        //}

        //public List<CFIFILEM> GetImportFilesByCustomFile(long CUSTOMFILE)
        //{
        //    return (from a in context.CFIFILEMs
        //            where a.CUSTOMFILE == CUSTOMFILE
        //            select a).ToList();
        //}

        //public void Add(CFIFILEM entity)
        //{
        //    context.CFIFILEMs.Add(entity);
        //}

        //public void Remove(CFIFILEM entity)
        //{
        //    //if (entity.EntityState == System.Data.EntityState.Unchanged)
        //    {
        //        context.CFIFILEMs.Attach(entity);
        //    }
        //    context.CFIFILEMs.Remove(entity);
        //}

        //public void Update(CFIFILEM entity)
        //{
        //    //if (entity.EntityState == System.Data.EntityState.Unchanged)
        //    {
        //        context.CFIFILEMs.Attach(entity); context.SetAsModified(entity);
        //    }

        //}

        //public List<CFIFILEM> All()
        //{
        //    return context.CFIFILEMs.ToList();
        //}

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIFILEM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        //public CFIFILEM GetSingle(EntityKeyFields entityKeys)
        //{
        //    var keys = entityKeys as CFIFILEMKeys;
        //    return this.GetSingle(keys.FILENO, keys.CUSTOMFILE);
        //}
    }
}

