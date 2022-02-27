using Newtonsoft.Json;
using Stimulsoft.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CommunicationWorkerRole.Stimulsoft.fonts
{
    public static class StimulsoftFontsService
    {
        private static readonly string fontsPath = "Stimulsoft\\fonts\\";
        private static string path;
        private static List<FontFamily> defaultFonts;

        public static void AddFonts()
        {
            path = Path.Combine(Environment.CurrentDirectory, fontsPath);
            File.WriteAllText("pathxxxxx.json", JsonConvert.SerializeObject(path));
            defaultFonts = StiFontCollection.GetFontFamilies();

            AddCenturyGothicFont();
            AddDidotFont();
            AddGaramondFont();
            AddLucidaBrightFont();
            AddMonacoFont();
            AddOptimaFont();
            AddPerpetuaFont();
        }

        private static void AddFont(string filePath)
        {
            StiFontCollection.AddFontFile(Path.Combine(path, filePath));
        }

        private static bool FontExists(string fontName)
        {
            return defaultFonts.Any(x => x.Name.ToLower() == fontName);
        }

        private static void AddPerpetuaFont()
        {
            if (FontExists("perpetua")) return;

            AddFont("perpetua\\PER_____.ttf");
            AddFont("perpetua\\PERB____.ttf");
            AddFont("perpetua\\PERBI___.ttf");
            AddFont("perpetua\\PERI____.ttf");
        }

        private static void AddOptimaFont()
        {
            if (FontExists("optima")) return;

            AddFont("optima\\OPTIMA.ttf");
            AddFont("optima\\OPTIMA_B.ttf");
            AddFont("optima\\Optima_Italic.ttf");
            AddFont("optima\\Optima_Medium.ttf");
            AddFont("optima\\Optima_Medium.ttf");
        }

        private static void AddMonacoFont()
        {
            if (FontExists("monaco")) return;

            AddFont("monaco\\Monaco.ttf");
        }

        private static void AddLucidaBrightFont()
        {
            if (FontExists("lucida bright")) return;

            AddFont("lucida-bright\\LBRITE.ttf");
            AddFont("lucida-bright\\LBRITED.ttf");
            AddFont("lucida-bright\\LBRITEDI.ttf");
            AddFont("lucida-bright\\LBRITEI.ttf");
        }

        private static void AddGaramondFont()
        {
            if (FontExists("garamond")) return;

            AddFont("garamond\\GARA.ttf");
            AddFont("garamond\\GARABD.ttf");
            AddFont("garamond\\GARAIT.ttf");
        }

        private static void AddDidotFont()
        {
            if (FontExists("gfs didot")) return;

            AddFont("didot\\GFSDidot-Regular.ttf");
        }

        private static void AddCenturyGothicFont()
        {
            if (FontExists("century gothic")) return;

            AddFont("century-gothic\\GOTHIC.ttf");
            AddFont("century-gothic\\GOTHICB.ttf");
            AddFont("century-gothic\\GOTHICBI.ttf");
            AddFont("century-gothic\\GOTHICI.ttf");
        }


    }
}
