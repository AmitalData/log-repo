using System.Data;


namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class SplittedDataArguments
    {
        public DataRow TableRow;
        public DataTable DataTable;
        public string CoulmnName;
        public char Delimiter;

        public class Builder
        {
            SplittedDataArguments splittedDataArguments = new SplittedDataArguments();

            public Builder()
            {
            }

            public Builder TableRow(DataRow tableRow)
            {
                splittedDataArguments.TableRow = tableRow;
                return this;
            }

            public Builder DataTable(DataTable dataTable)
            {
                splittedDataArguments.DataTable = dataTable;
                return this;
            }

            public Builder CoulmnName(string coulmnName)
            {
                splittedDataArguments.CoulmnName = coulmnName;
                return this;
            }

            public Builder Delimiter(char delimiter)
            {
                splittedDataArguments.Delimiter = delimiter;
                return this;
            }
            public SplittedDataArguments Build()
            {
                return splittedDataArguments;
            }
        } 
    }
}
