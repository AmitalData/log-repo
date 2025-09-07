using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class HorseQuery
    {
        HorseRepository repository;



        public HorseQuery(int tenant)
        {
            repository = new HorseRepository(tenant);
        }

        public HorseQuery(HorseRepository repository)
        {
            this.repository = repository;
        }

        public HorsePM GetSinglePM(string id, int tenant)
        {
            HorsePM horse = (from a in repository.context.Horses.Include("HorseGender")
                             where a.Tenant == tenant && a.Id == id
                             select new HorsePM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 Name = a.Name,
                                 YearOfBirth = a.YearOfBirth,
                                 Color = a.Color,
                                 GenderName = a.HorseGender == null ? null : a.HorseGender.Name,
                                 Breed = a.Breed,
                                 Discipline = a.Discipline,
                                 TravelBehavior = a.TravelBehavior,
                                 MicochipNumber = a.MicochipNumber,
                                 PassportNumber = a.PassportNumber,
                                 CountryOfBirthId = a.CountryOfBirthId,
                                 CurrentStable = a.CurrentStable,
                                 Owner = a.Owner,
                                 Remarks = a.Remarks,
                                 Inactive = a.Inactive,
                                 CreateDate = a.CreateDate,
                                 CreatedByUserId = a.CreatedByUserId,
                                 UpdateDate = a.UpdateDate,
                                 UpdatedByUserId = a.UpdatedByUserId,
                                 SearchFields = a.SearchFields,
                                 GenderCode = a.GenderCode,
                             }).FirstOrDefault();

            return horse;
        }

        public IQueryable<HorsePM> GetHorsePMsByTenant(int tenant)
        {
            IQueryable<HorsePM> ports = (from a in repository.context.Horses.Include("HorseGender")
                                         where a.Tenant == tenant
                                         select new HorsePM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             Name = a.Name,
                                             YearOfBirth = a.YearOfBirth,
                                             Color = a.Color,
                                             GenderName = a.HorseGender == null ? null : a.HorseGender.Name,
                                             Breed = a.Breed,
                                             Discipline = a.Discipline,
                                             TravelBehavior = a.TravelBehavior,
                                             MicochipNumber = a.MicochipNumber,
                                             PassportNumber = a.PassportNumber,
                                             CountryOfBirthId = a.CountryOfBirthId,
                                             CurrentStable = a.CurrentStable,
                                             Owner = a.Owner,
                                             Remarks = a.Remarks,
                                             Inactive = a.Inactive,
                                             CreateDate = a.CreateDate,
                                             CreatedByUserId = a.CreatedByUserId,
                                             UpdateDate = a.UpdateDate,
                                             UpdatedByUserId = a.UpdatedByUserId,
                                             SearchFields = a.SearchFields,
                                             GenderCode = a.GenderCode,
                                         });
            return ports;
        }

        public IQueryable<HorseList> GetIQueryableEntityList(IQueryable<Horse> iQueryable)
        {
            IQueryable<HorseList> result = from a in iQueryable.Include("CountryOfBirth").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("HorseGender")
                                           select new HorseList()
                                            {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               Name = a.Name,
                                               YearOfBirth = a.YearOfBirth,
                                               Color = a.Color,
                                               GenderName = a.HorseGender == null ? null : a.HorseGender.Name,
                                               Breed = a.Breed,
                                               Discipline = a.Discipline,
                                               TravelBehavior = a.TravelBehavior,
                                               MicochipNumber = a.MicochipNumber,
                                               PassportNumber = a.PassportNumber,
                                               CountryOfBirthId = a.CountryOfBirthId,
                                               CountryOfBirthCode  = a.CountryOfBirth ==  null ? null : a.CountryOfBirth.Code,
                                               CountryOfBirthName = a.CountryOfBirth == null ? null : a.CountryOfBirth.EnglishName,
                                               CurrentStable = a.CurrentStable,
                                               Owner = a.Owner,
                                               Remarks = a.Remarks,
                                               Inactive = a.Inactive,
                                               CreateDate = a.CreateDate,
                                               CreatedByUserId = a.CreatedByUserId,
                                               UpdateDate = a.UpdateDate,
                                               UpdatedByUserId = a.UpdatedByUserId,
                                               SearchFields = a.SearchFields,
                                               CreatedByUserName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact.EnglishName,
                                               UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                               GenderCode = a.GenderCode,
                                           };
            return result;
        }
    }
}
