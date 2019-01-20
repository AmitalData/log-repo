using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
///using UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference;

namespace AmitalCustomsWindowsService.Tester.CustomMessage
{
    public partial class Send380Form : Form
    {
        public Send380Form()
        {
            InitializeComponent();
        }


        private void textBoxSourceFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBoxSourceFile.Text = openFileDialog1.FileName;

        }



        private void btnSendDCA_Click(object sender, EventArgs e)
        {
            //test380();
        }

#if false
        public void test380()
        {
            try
            {

                var fileReadAllText = File.ReadAllText(this.textBoxSourceFile.Text);
                byte[] myContent = stringToBase64ByteArray(fileReadAllText); ;
                {

                    //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                    //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                    //var myRequestHeader = new RequestHeader(){ ConsumerId = ;
                    var ExternalId = Guid.NewGuid().ToString();
                    var myRequest = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity()
                    {

                        RequestContentHeader = new RequestContentHeader() { RecieverID = new int[] { 1 }, SenderID = 1 },

                        documentID = 0,
                        documentIDSpecified = false,
                        ///RelatedEntity = new ConnectedEntity()
                        //{
                        //    entityIdExternalReferenceID = "entityIdExternalReferenceID",
                        //    entityIdKey1 = "entityIdKey1",
                        //    entityPath = "entityPath",
                        //    entityType = 380
                        //}
                        ///,


                        Attachment = new Attachment
                        {
                            documentType = "380",
                            IsAttachment = true.ToString(),
                            //"לא התקבלו כל שדות המטה-דטא חובה הבאים: : 3,39,55,87 עבור סוג מסמך : 380"
                            //"צרופה לא תקינה סוג המסמך : <NULL> שם :  נתוני שדה נוסף : 3 שגויים - הערך : IL אינו מסוג : Int"
                            AdditionalData = new AttachmentAdditionalData[] 
                { 
                    new  AttachmentAdditionalData (){fieldID = 3,fieldData=this.textBoxCntry.Text    } ,//ארץ חשבון 
                    new  AttachmentAdditionalData (){fieldID = 39,fieldData=this.textBoxAccNum.Text } ,///מספר חשבון
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=this.textBoxDate.Text   },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=this.checkBoxOriginal.Checked.ToString()   } ,//האם מסמך מקורי
                },
                            fileName = this.textBoxSourceFile.Text + ".txt",

                            //documentType = "1",
                            attachmentID = this.textBoxAttachmentID.Text,
                            externalAttachmentID = this.textBoxAttachmentID.Text,
                            Remark = "mY Remark ",
                            content = myContent
                        }
                    };







                    
                    var tempXml = XmlGenericUtil<D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntity>.SerializeObject(myRequest);
                    //\\bmw\Unifreight\work_dir\GetDOC_MSG2715_2716_AddAttachmentResponse_In.IL512320953.2014-05-18_16-51-14.A3ACB642-2A38-4B10-B656-77F5F46EE088.PRD.xml

                    ResponseHeader myResponseHeader = null;
                    D_NG_2716_MSG22001_AddAttachmentResponse myResponse = null;
                    // MoreParams myMoreParams = new MoreParams();
                    // var myProxy = myUnifreightSdkGateway.GetChannel<IGlobalScannedAttachmentToEntityOperation>();
                    // myResponseHeader = myProxy
                    //     .AddAGlobalScannedAttachmentToEntity(
                    //     ExternalId,
                    //     ConfigurationManager.AppSettings["ConsumerID"],
                    //myRequest,
                    //ref myMoreParams,
                    //out myResponse);
                }
            }
            catch (System.Exception e)
            {

                //throw;
                Debug.Fail(e.ToString().Substring(0, 100));
            }
        }

        
#endif
        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.UTF8Encoding.UTF8.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.UTF8Encoding.UTF8.GetBytes(s);
            return ret;
        }
    }
}
