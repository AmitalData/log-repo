using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class HelpResourceQuery
    {
        HelpResourceRepository repository;

        public HelpResourceQuery()
        {
            repository = new HelpResourceRepository();
        }

        public HelpResourceQuery(HelpResourceRepository HelpResourceRepository)
        {
            repository = HelpResourceRepository;
        }

        public HelpResourcePM GetSinglePM(string code, int tenant = 0)
        {
            HelpResourcePM entity;
            entity = (from a in repository.context.HelpResources
                      where a.Code == code
                      select new HelpResourcePM()
                      {                          
                          Code = a.Code,
                          SearchFields = a.SearchFields,
                          Name = a.Name,                          
                          Tenant = tenant,
                          Category = a.Category,
                          Type = a.Type,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          Duration = a.Duration,
                          FeatureCode = a.FeatureCode,
                          FileName = a.FileName,
                          IsNew = a.IsNew,
                          Language = a.Language,
                          VideoURL = a.VideoURL,
                          Inactive = a.Inactive,
                      }).FirstOrDefault();

            return entity;
        }
        
        public IQueryable<HelpResourceList> GetIQueryableEntityList(IQueryable<HelpResource> iQueryable)
        {
            IQueryable<HelpResourceList> result = from a in iQueryable
                                                  select new HelpResourceList()
                                                  {
                                                      Code = a.Code,
                                                      SearchFields = a.SearchFields,
                                                      Name = a.Name,
                                                      Tenant = a.Tenant,
                                                      Category = a.Category,
                                                      Type = a.Type,
                                                      CreateDate = a.CreateDate,
                                                      UpdateDate = a.UpdateDate,
                                                      Duration = a.Duration,
                                                      FeatureCode = a.FeatureCode,
                                                      FileName = a.FileName,
                                                      IsNew = a.IsNew,
                                                      Language = a.Language,
                                                      VideoURL = a.VideoURL,
                                                      Inactive = a.Inactive,
                                                  };

            List<HelpResourceList> result_List = result.ToList();
            foreach (HelpResourceList item in result_List)
            {
                item.CategoryName= this.FillCategoryName(item);
                item.TypeName = this.FillTypeName(item);
            }

            return result_List.AsQueryable();
        }

        private string FillTypeName(HelpResourceList a)
        {
            string result = null;

            switch (a.Type)
            {
                case "TUT":
                    {
                        result = "Tutorials";
                        break;
                    }

                case "HOW":
                    {
                        result = "How To";
                        break;
                    }

                case "REL":
                    {
                        result = "Release Notes";
                        break;
                    }

                case "VID":
                    {
                        result = "Videos";
                        break;
                    }
            }

            return result;
        }

        private string FillCategoryName(HelpResourceList a)
        {
            string result = null;

            switch (a.Category)
            {
                case "OPE":
                    {
                        result = "Operational";
                        break;
                    }

                case "ACC":
                    {
                        result = "Accounting";
                        break;
                    }

                case "AWB":
                    {
                        result = "E-AWB";
                        break;
                    }

                case "CRM":
                    {
                        result = "CRM";
                        break;
                    }
            }

            return result;
        }
    }
}
