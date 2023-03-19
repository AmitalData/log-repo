using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Workflow.BL.FieldsMapping
{
    public static partial class EntityFieldsMapping
    {
        public static string GetLoggedUserId(int tenant)
        {
            string email = "system@tenant" + tenant + ".com";
            if (HttpContext.Current != null)
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            return loggedContact?.Id;
        }

        public static string GetEntityObjectTableName(string entityObjectTableId, int tenant)
        {
            if (!string.IsNullOrEmpty(entityObjectTableId))
            {
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(entityObjectTableId, tenant, false);
                return objectTable != null ? (objectTable.FullNameTextCode != null ? objectTable.FullNameTextCode.DefaultText : objectTable.Name) : null;
            }
            return null;
        }

        public static string GetUserName(string userId, int tenant)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                UserRepository userRepository = new UserRepository(tenant);
                User user = userRepository.GetSingleUser(userId, tenant, false);
                return user?.Contact?.EnglishName;
            }
            return null;
        }
        
        public static string GetCardName(string cardId, int tenant)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                CardRepository cardRepository = new CardRepository(tenant);
                Card card = cardRepository.GetSingleCardByIdAndTenant(cardId, tenant, false);
                return card?.EnglishName;
            }
            return null;
        }

        public static string GetSearchFields(string[] values)
        {
            if(values != null && values.Length > 0)
            {
                string searchFields = null;

                for(int i = 0; i < values.Length; i++)
                {
                    searchFields = AppendToSearchFields(searchFields, values[i]);
                }

                return searchFields;
            }
            return null;
        }

        private static string AppendToSearchFields(string searchFields, string value)
        {
            if (string.IsNullOrEmpty(searchFields))
            {
                return value;
            }

            return searchFields + (string.IsNullOrEmpty(value) ? "" : ",") + value;
        }
    }
}