using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ScreenFieldsQuery
    {
        ScreenFieldsRepository repository;
        public ScreenFieldsQuery()
        {
            repository = new ScreenFieldsRepository(); 
        }

        public ScreenFieldsQuery(int tenant)
        {
            repository = new ScreenFieldsRepository(tenant);
        }

        public ScreenFieldsQuery(ScreenFieldsRepository screenFieldsRepository)
        {
            repository = screenFieldsRepository;
        }

        public List<ScreenFieldPM> GetScreenFieldPMsByTenant(int tenant)
        {




            List<ScreenFieldPM> screenfields = (from a in repository.context.ScreenFields.Include("ObjectField").Include("Screen").Include("ObjectField.ObjectTable")
                                                where a.Tenant == tenant || a.Tenant == 0
                                                select new ScreenFieldPM()
                                                {
                                                    Column = a.Column,
                                                    Id = a.Id,
                                                    ObjectFieldId = a.ObjectFieldId,
                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                    Row = a.Row,
                                                    ScreenId = a.ScreenId,
                                                    Tenant = a.Tenant,
                                                    ScreenCode = a.ScreenCode,
                                                    ObjectFieldObjectTableName = a.ObjectField.ObjectTable.Name,
                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                }).ToList();


            List<ScreenFieldPM> selectedScreenFields = new List<ScreenFieldPM>();
            foreach (ScreenFieldPM field in screenfields)
            {
                ScreenFieldPM existedField = (from a in selectedScreenFields
                                              where a.ScreenId == field.ScreenId && a.ObjectFieldCode == field.ObjectFieldCode
                                              select a).FirstOrDefault();

                if (existedField != null)
                {
                    if (existedField.Tenant == 0)
                    {
                        selectedScreenFields.Remove(existedField);
                        selectedScreenFields.Add(field);
                    }
                }
                else
                {
                    selectedScreenFields.Add(field);
                }
            }

            return selectedScreenFields;
        }

        public IQueryable<ScreenFieldPM> GetScreenFieldPMsByScreen(int tenant, string screenId)
        {
            IQueryable<ScreenFieldPM> screenfields = from a in repository.context.ScreenFields.Include("ObjectField").Include("Screen").Include("ObjectField.ObjectTable")
                                                     where a.Tenant == tenant && a.ScreenId == screenId
                                                     select new ScreenFieldPM()
                                                     {
                                                         Column = a.Column,
                                                         Id = a.Id,
                                                         ObjectFieldId = a.ObjectFieldId,
                                                         ObjectFieldName = a.ObjectField.FieldName,
                                                         Row = a.Row,
                                                         ScreenId = a.ScreenId,
                                                         Tenant = a.Tenant,
                                                         ScreenCode = a.ScreenCode,
                                                         ObjectFieldObjectTableName = a.ObjectField.ObjectTable.Name,
                                                         ObjectFieldCode = a.ObjectFieldCode,
                                                     };
            return screenfields;
        }



    }
}