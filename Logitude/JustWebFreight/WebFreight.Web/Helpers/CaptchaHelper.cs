using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class CaptchaHelper
    {
        private string GenerateCaptchaImage(string code)
        {
            int fontsize = 17;
            System.Drawing.Font font = new System.Drawing.Font(
              new FontFamily("Times New Roman"),
                  ((float)fontsize),
                  FontStyle.Bold | FontStyle.Italic,    // + obviously doesn't work, but what am I meant to do?
                  GraphicsUnit.Pixel
              );

            int height = 30;

            int width = 90;

            Bitmap bmp = new Bitmap(width, height);

            RectangleF rectf = new RectangleF(10, 5, 0, 0);

            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);


            //    // Fill in the background.
            Rectangle rect = new Rectangle(0, 0, 100, 30);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawString(code, font, Brushes.Green, rectf);

            g.DrawRectangle(new Pen(Color.Transparent), 1, 1, width - 2, height - 2);

            g.Flush();


            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);

            g.Dispose();


            byte[] byteImage = ms.ToArray();
            string base64String = Convert.ToBase64String(byteImage); //here you should get a base64 string
            base64String = "data:image/" + "Jpeg" + ";base64," + base64String;


            return base64String;
        }

        public bool CheckCaptchaCodeValidated(string code, string Key , string userCaptchaKey = null , bool withoutCheckUsed = false)
        {

            bool result = false;
            if (!string.IsNullOrEmpty(Key))
            {
                CaptchaKeyRepository captchaKeyRepository = new CaptchaKeyRepository();
                CaptchaKey captchaKey = !withoutCheckUsed ? captchaKeyRepository.GetSingleCaptchaKey(Key): captchaKeyRepository.GetSingleCaptchaKeyById(Key);
                if (captchaKey != null)
                {
                    if (captchaKey.Id == userCaptchaKey || string.IsNullOrEmpty(userCaptchaKey))
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            if (captchaKey.Code.ToUpper() == code.ToUpper()) result = true;
                        }
                    }

                    captchaKey.IsUsed = true;
                    captchaKeyRepository.Update(captchaKey);
                    captchaKeyRepository.SubmitChanges();
                }
            }
            return result;
        }

        public void AddCaptchaKey(string email, UserData data, string activity)
        {
            CaptchaKeyRepository captchaKeyRepository = new CaptchaKeyRepository();
            CaptchaKey captchaKey = new CaptchaKey()
            {
                Id = Guid.NewGuid().ToString(),
                IP = AuthenticationUtil.GetIP4Address(),
                Code = RandomString(6),
                CreateDate = DateTime.Now,
                Email = email,
                Activity = activity,
                IsUsed = false,
            };
            captchaKeyRepository.Add(captchaKey);
            captchaKeyRepository.SubmitChanges();

            data.HasError = true;
            data.InValidCaptcha = true;
            data.CaptchaImage = GenerateCaptchaImage(captchaKey.Code);
            data.CaptchaKey = captchaKey.Id;


        }

        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}