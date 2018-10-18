using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using System.Collections.ObjectModel;

namespace Logitude.Social.BL.EntityQueryServices
{
  public partial  class ConversationHeaderParticipantQueryService
    {



      public IQueryable<ConversationHeaderParticipantList> GetAllParticipantForHeader(string conversationHeaderId, int tenant)
      {
          IQueryable<ConversationHeaderParticipantList> myResult = from a in context.ConversationHeaderParticipants.Include("ParticipantUser").Include("ParticipantUser.Contact")
                                                                   where a.ConversationHeaderId == conversationHeaderId
                                                                   select new ConversationHeaderParticipantList()
                                                      {
                                                          Id = a.Id,
                                                          DeleteDate = a.DeleteDate,
                                                          CreateDate = a.CreateDate,
                                                          ParticipantUserId = a.ParticipantUserId,
                                                          ConversationHeaderId = a.ConversationHeaderId,
                                                          IsRead =a.IsRead,
                                                          IsDelete = a.IsDelete,
                                                          IsLeft = a.IsLeft,
                                                          
                                                      };
                                               
          return myResult;
      }


      public List<ConversationHeaderParticipantPM> GetAllParticipantListForHeader(string conversationHeaderId, int tenant)
      {
          IQueryable<ConversationHeaderParticipant> conversationHeaderParticipantList = (from a in context.ConversationHeaderParticipants.Include("ParticipantUser").Include("ParticipantUser.Contact")
                                                                                         where a.ConversationHeaderId == conversationHeaderId
                                                                                         select a);

          List<ConversationHeaderParticipantPM> conversationHeaderParticipantPMs = new List<ConversationHeaderParticipantPM>();

          foreach (ConversationHeaderParticipant conversationHeaderParticipant in conversationHeaderParticipantList)
          {
              EntityKeys = new ConversationHeaderParticipantKeys() { Id = conversationHeaderParticipant.Id };
              ConversationHeaderParticipantPM conversationParticipantPM = new ConversationHeaderParticipantPM();
              mapping.CustomPOCOToPM(conversationParticipantPM, conversationHeaderParticipant);
              mapping.POCOToPM(conversationParticipantPM, conversationHeaderParticipant);
              conversationParticipantPM.ParticipantName = conversationHeaderParticipant.ParticipantUser.Contact.EnglishName;
              conversationHeaderParticipantPMs.Add(conversationParticipantPM);
          }

          return conversationHeaderParticipantPMs;
      }


      public string  GetMyParticipantPMId(string conversationHeaderId, string userid, int tenant)
      {
          return (from a in context.ConversationHeaderParticipants
                  where a.ConversationHeaderId == conversationHeaderId && a.Tenant == tenant && a.ParticipantUserId == userid
                  select a.Id).FirstOrDefault();
        
      }


      public string GetAllImageIdParticipantListForHeader(string conversationHeaderId, int tenant)
      {
          string ParticipantListImageId = "";
          ColorIndexRepository colorIndexRepository = new ColorIndexRepository(tenant);

          IQueryable<ConversationHeaderParticipant> conversationHeaderParticipantList = (from a in context.ConversationHeaderParticipants.Include("ConversationHeader").Include("ParticipantUser").Include("ParticipantUser.Contact")
                                                                                         where a.ConversationHeaderId == conversationHeaderId

                                                                                         select a);
          int i = 0;
          foreach (ConversationHeaderParticipant conversationHeaderParticipant in conversationHeaderParticipantList)
          {
              i++;

              int? indexcolor = conversationHeaderParticipant.ParticipantUser.Contact.IndexColor;
              string color =  colorIndexRepository.GetSingleHasColor(indexcolor);
              string CodeOfName = "";

              List<string> Name = conversationHeaderParticipant.ParticipantUser.Contact.EnglishName.Split(' ').ToList<string>();
 
              if (Name.Count == 1)
              {
                  if (!string.IsNullOrEmpty(Name[0]))
                  {

                      if (Name[0].Length > 2) CodeOfName = Name[0].Remove(2);
                  
                      else CodeOfName = Name[0];
                   
                  }

              }
              else
                  if (Name.Count > 1)
                  {

                      if (!string.IsNullOrEmpty(Name[0]))
                      {
                          if (Name[0].Length > 1)  CodeOfName += Name[0].Remove(1);

                          else CodeOfName += Name[0];
                      
                      }

                      if (!string.IsNullOrEmpty(Name[1]))
                      {
                          if (Name[1].Length > 1) CodeOfName += Name[1].Remove(1);

                          else CodeOfName += Name[1];
                     
                      }
                      else
                      {
                          if (Name[0].Length > 2)  CodeOfName = Name[0].Remove(2);

                          else CodeOfName = Name[0];
                        
                      }

                  }



              if (i == conversationHeaderParticipantList.Count())
              {
                  if (conversationHeaderParticipantList.Count() == 2)
                  {
                      ParticipantListImageId += conversationHeaderParticipant.ParticipantUser.Contact.ImageDetailId + "_" + conversationHeaderParticipant.ParticipantUserId + "_" + CodeOfName +  "_" + color;
                  }
                  else
                  {
                      ParticipantListImageId += conversationHeaderParticipant.ParticipantUser.Contact.ImageDetailId + "_" + CodeOfName + "_" + color;
                  }
              }
              else
              {

                  if (conversationHeaderParticipantList.Count() == 2)
                  {
                      ParticipantListImageId += conversationHeaderParticipant.ParticipantUser.Contact.ImageDetailId + "_" + conversationHeaderParticipant.ParticipantUserId + "_" + CodeOfName +"_" + color +",";
                  }
                  else
                  {
                      ParticipantListImageId += conversationHeaderParticipant.ParticipantUser.Contact.ImageDetailId + "_" + CodeOfName + "_" + color + ",";
                  }
              }

          }


          return ParticipantListImageId;
      }









    }
}
