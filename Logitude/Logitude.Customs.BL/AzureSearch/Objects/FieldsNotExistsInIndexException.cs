using System;
using System.Collections.Generic;

namespace Logitude.Customs.BL.AzureSearch.Objects
{
    public class FieldsNotExistsInIndexException : Exception
    {
        public List<string> MissingFields { get; }

        public FieldsNotExistsInIndexException(List<string> missingFields)
            : base($"The following fields do not exist in the index: {string.Join(", ", missingFields)}")
        {
            MissingFields = missingFields;
        }
    }
}