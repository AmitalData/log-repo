using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unifreight.Data.AmitalModel;

namespace Logitude.Update
{
    public partial class MappUnifreightTables : Form
    {
        public MappUnifreightTables()
        {
            InitializeComponent();
        }
        string[,] classes;
        void SetClases()
        {
            this.classes = new string[5, 2];
            this.classes[0, 0] = "Keys"; // file name
            this.classes[0, 1] = @"C:\LWC\log-repo2\Logitude\Unifreight.Data\AmitalModel\EntityKeys"; // file location
            this.classes[1, 0] = "Repository";
            this.classes[1, 1] = @"C:\LWC\log-repo2\Logitude\Unifreight.Data\AmitalModel\Repsitories";
            this.classes[2, 0] = "DataMapping";
            this.classes[2, 1] = @"C:\LWC\log-repo2\Logitude\Unifreight.BL\EntityDataMappings";
            this.classes[3, 0] = "PM";
            this.classes[3, 1] = @"C:\LWC\log-repo2\Logitude\Unifreight.BL\EntityPMs\UGenerated";
            this.classes[4, 0] = "QueryService";
            this.classes[4, 1] = @"C:\LWC\log-repo2\Logitude\Unifreight.BL\EntityQueryServices";
        }
        private void MappUnifreightTables_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string _CopyFrom = "CTBTAXTYPE";
            SetClases();
            var c = AmitalContext.GetContext(1);
            var unifreightTables = c.GetTableNames("Unifreight.Data.AmitalModel.EntityPOCOs");
            var tableName = TableName.Text;
            // TODO : IMPROVE READI
           /* PropertyInfo[] myPropertyInfo = c.GetType().GetProperties();  
            string tableNameProp = "";
            foreach (PropertyInfo prop in myPropertyInfo)
            {
                if (prop.Name.Contains(tableName))
                {
                    tableNameProp = prop.Name;
                }
            }
            if (tableNameProp != "")
            {
                //c.
            }*/
            var unifreighttable = unifreightTables.Any(s => s.Contains(tableName));
            if (unifreighttable)
            {
                string _CopyTo = tableName;
                for (int i = 0; i < 5; i++)
                {
                    var sourceFile = Path.Combine(this.classes[i, 1], _CopyFrom + this.classes[i, 0] + ".cs");
                    var newFile = sourceFile.Replace(_CopyFrom + this.classes[i, 0], _CopyTo + this.classes[i, 0]);
                    var PartialClassSourceText = new StringBuilder(File.ReadAllText(sourceFile));

                    PartialClassSourceText = PartialClassSourceText.Replace(_CopyFrom, _CopyTo);
                    PartialClassSourceText = PartialClassSourceText.Replace("TAXTYPE", KeyTextBox.Text);
                    File.WriteAllText(newFile, PartialClassSourceText.ToString());
                }
            }
            else
            {
                MessageBox.Show("Table is not found", "error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
