using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class MAWBStackQuery
    {
        MAWBStackRepository repository;

        public MAWBStackQuery()
        {
            repository = new MAWBStackRepository(); 
        }

        public MAWBStackQuery(int tenant)
        {
            repository = new MAWBStackRepository(tenant);
        }

        public MAWBStackQuery(MAWBStackRepository repository)
        {
            this.repository = repository;
        }

        public MAWBStackPM GetSinglePM(string id, int tenant)
        {
            var mAWBStack = (from a in repository.context.MAWBStacks
                             where a.Tenant == tenant && a.Id == id
                             select new MAWBStackPM()
                             {
                                 Id = a.Id,
                                 AirlineId = a.AirlineId,
                                 Tenant = a.Tenant,
                                 Number = a.Number,
                                 InsertionDate = a.InsertionDate,
                                 Notes = a.Notes,
                                 AssignedToId = a.AssignedToId,
                                 IsUsed = a.IsUsed,
                             }).FirstOrDefault();

            return mAWBStack;
        }

        public MAWBStackPM GetSingleMAWBStackPMByNumber(int number, int tenant)
        {
            var mAWBStack = (from a in repository.context.MAWBStacks
                             where a.Tenant == tenant && a.Number == number && a.IsUsed == false
                             select new MAWBStackPM()
                             {
                                 Id = a.Id,
                                 AirlineId = a.AirlineId,
                                 Tenant = a.Tenant,
                                 Number = a.Number,
                                 InsertionDate = a.InsertionDate,
                                 Notes = a.Notes,
                                 AssignedToId = a.AssignedToId,
                                 IsUsed = a.IsUsed,
                             }).FirstOrDefault();

            return mAWBStack;
        }

        public IQueryable<MAWBStackPM> GetMAWBStackPMsByTenant(int tenant)
        {
            IQueryable<MAWBStackPM> mawbStacks = from a in repository.context.MAWBStacks
                                                 where a.Tenant == tenant && a.IsUsed == false
                                                 select new MAWBStackPM()
                                                 {
                                                     Id = a.Id,
                                                     AirlineId = a.AirlineId,
                                                     Tenant = a.Tenant,
                                                     Number = a.Number,
                                                     InsertionDate = a.InsertionDate,
                                                     Notes = a.Notes,
                                                     AssignedToId = a.AssignedToId,
                                                     IsUsed = a.IsUsed,
                                                 };
            return mawbStacks;
        }
        
        public List<MAWBStackPM> GetMAWBStackPMsByAirlineIdAndCustomerId(string airlineId, string customerId, int tenant)
        {
            List<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                       where a.Tenant == tenant && a.AirlineId == airlineId && a.IsUsed == false && (a.AssignedToId == null || a.AssignedToId == customerId)
                                       select new MAWBStackPM()
                                       {
                                           Id = a.Id,
                                           AirlineId = a.AirlineId,
                                           Tenant = a.Tenant,
                                           Number = a.Number,
                                           InsertionDate = a.InsertionDate,
                                           Notes = a.Notes,
                                           AssignedToId = a.AssignedToId,
                                           IsUsed = a.IsUsed,
                                       }).ToList();

            CardRepository cardRepository = new CardRepository(tenant);
            foreach (MAWBStackPM stack in query)
            {
                if (stack.AssignedToId != null)
                {
                    stack.AssignedToShipperName = cardRepository.GetSingleCard(stack.AssignedToId, tenant).EnglishName;
                }
            }

            return query;
        }

        public bool HasMAWBStackPMsForAirlineIdAndCustomerId(string airlineId, string customerId, int tenant)
        {
            return (from a in repository.context.MAWBStacks
                    where a.Tenant == tenant && a.AirlineId == airlineId && a.IsUsed == false && a.AssignedToId == customerId
                    select a).Any();
        }

        public List<MAWBStackPM> GetMAWBStackPMsByAirlineId(string airlineId, int tenant)
        {
            List<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                             where a.Tenant == tenant && a.AirlineId == airlineId && a.IsUsed == false
                                             select new MAWBStackPM()
                                             {
                                                 Id = a.Id,
                                                 AirlineId = a.AirlineId,
                                                 Tenant = a.Tenant,
                                                 Number = a.Number,
                                                 InsertionDate = a.InsertionDate,
                                                 Notes = a.Notes,
                                                 AssignedToId = a.AssignedToId,
                                                 IsUsed = a.IsUsed,
                                             }).ToList();

            CardRepository cardRepository = new CardRepository(tenant);
            foreach (MAWBStackPM stack in query)
            {
                if (stack.AssignedToId != null)
                {
                    stack.AssignedToShipperName = cardRepository.GetSingleCard(stack.AssignedToId, tenant).EnglishName;
                }
            }

            return query;
        }



        public List<MAWBStackPM> GetAllMAWBStackPMsByAirlineId(string airlineId, int tenant)
        {
            List<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                       where a.Tenant == tenant && a.AirlineId == airlineId 
                                       select new MAWBStackPM()
                                       {
                                           Id = a.Id,
                                           AirlineId = a.AirlineId,
                                           Tenant = a.Tenant,
                                           Number = a.Number,
                                           InsertionDate = a.InsertionDate,
                                           Notes = a.Notes,
                                           AssignedToId = a.AssignedToId,
                                           IsUsed = a.IsUsed,
                                       }).ToList();

            CardRepository cardRepository = new CardRepository(tenant);
            foreach (MAWBStackPM stack in query)
            {
                if (stack.AssignedToId != null)
                {
                    stack.AssignedToShipperName = cardRepository.GetSingleCard(stack.AssignedToId, tenant).EnglishName;
                }
            }


            return query;
        }

        public IQueryable<MAWBStackPM> GetNotAssignedMAWBStackPMsByAirlineId(string airlineId, int tenant)
        {
            IQueryable<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                             where a.Tenant == tenant && a.AirlineId == airlineId && a.IsUsed == false && a.AssignedToId == null
                                             select new MAWBStackPM()
                                             {
                                                 Id = a.Id,
                                                 AirlineId = a.AirlineId,
                                                 Tenant = a.Tenant,
                                                 Number = a.Number,
                                                 InsertionDate = a.InsertionDate,
                                                 Notes = a.Notes,
                                                 AssignedToId = a.AssignedToId,
                                                 IsUsed = a.IsUsed,
                                                 AirlineName = a.Airline.Card.EnglishName,
                                             }).AsQueryable();
            return query;
        }

        public IQueryable<MAWBStackPM> GetAssignedMAWBStackPMsByCustomerId(string customerId, int tenant)
        {
            IQueryable<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                             where a.Tenant == tenant && a.IsUsed == false && a.AssignedToId == customerId
                                             select new MAWBStackPM()
                                             {
                                                 Id = a.Id,
                                                 AirlineId = a.AirlineId,
                                                 Tenant = a.Tenant,
                                                 Number = a.Number,
                                                 InsertionDate = a.InsertionDate,
                                                 Notes = a.Notes,
                                                 AssignedToId = a.AssignedToId,
                                                 IsUsed = a.IsUsed,
                                                 AirlineName = a.Airline.Card.EnglishName,
                                             }).AsQueryable();
            return query;
        }

        public IQueryable<MAWBStackPM> GetAllNotAssignedMAWBStackPMs(int tenant)
        {
            IQueryable<MAWBStackPM> query = (from a in repository.context.MAWBStacks
                                             where a.Tenant == tenant && a.IsUsed == false && a.AssignedToId == null
                                             select new MAWBStackPM()
                                             {
                                                 Id = a.Id,
                                                 AirlineId = a.AirlineId,
                                                 Tenant = a.Tenant,
                                                 Number = a.Number,
                                                 InsertionDate = a.InsertionDate,
                                                 Notes = a.Notes,
                                                 AssignedToId = a.AssignedToId,
                                                 IsUsed = a.IsUsed,
                                                 AirlineName = a.Airline.Card.EnglishName,
                                             }).AsQueryable();
            return query;
        }
    }
}
