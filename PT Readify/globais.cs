using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PT_Readify
{
    internal class globais
    {//perfil
        static public bool confirmacao = false;
        static public string profilepassword = "";
        static public string profileEmail = "";
        static public bool iisAdmin = false;
        static public int id_utilizador = 0;
        

        // Lista completa de prefixos da Europa
        static public string[] prefixosEuropa = {
            "+351 (Portugal)", "+34 (Espanha)", "+33 (França)", "+44 (Reino Unido)",
            "+49 (Alemanha)", "+39 (Itália)", "+32 (Bélgica)", "+31 (Países Baixos)",
            "+41 (Suíça)", "+43 (Áustria)", "+30 (Grécia)", "+353 (Irlanda)",
            "+45 (Dinamarca)", "+46 (Suécia)", "+47 (Noruega)", "+358 (Finlândia)",
            "+48 (Polónia)", "+420 (Chéquia)", "+36 (Hungria)", "+40 (Roménia)",
            "+359 (Bulgária)", "+385 (Croácia)", "+421 (Eslováquia)", "+386 (Eslovénia)",
              "+372 (Estónia)", "+371 (Letónia)", "+370 (Lituânia)", "+352 (Luxemburgo)",
             "+356 (Malta)", "+357 (Chipre)", "+354 (Islândia)", "+376 (Andorra)",
             "+378 (San Marino)", "+379 (Vaticano)", "+423 (Liechtenstein)", "+377 (Mónaco)"
            };
    }
    [Serializable]
    public class Config
    {
        public string Theme { get; set; } // "Claro" or "Escuro"
        public bool FullscreenReading { get; set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public int AutoLogoutMinutes { get; set; }
        public string Language { get; set; } // "pt" or "en"
        public string OriginalLanguage { get; set; }

        public static Config Default()
        {
            return new Config
            {
                Theme = "Claro",
                FullscreenReading = false,
                FontName = "Arial",
                FontSize = 12,
                AutoLogoutMinutes = 15,
                Language = "pt",
                OriginalLanguage = "pt"
            };
        }
    }
    public static class ConfigManager
    {
        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PT_Readify");
        private static readonly string ConfigFile = Path.Combine(AppFolder, "config.xml");

        private static Config _current;
        public static Config Current
        {
            get
            {
                if (_current == null) _current = Load();
                return _current;
            }
            private set => _current = value;
        }

        public static Config Load()
        {
            try
            {
                if (!Directory.Exists(AppFolder)) Directory.CreateDirectory(AppFolder);
                if (!File.Exists(ConfigFile))
                {
                    var def = Config.Default();
                    Save(def);
                    return def;
                }

                using (var stream = File.OpenRead(ConfigFile))
                {
                    var serializer = new XmlSerializer(typeof(Config));
                    var cfg = (Config)serializer.Deserialize(stream);
                    // Ensure non-null and valid ranges:
                    if (cfg.FontSize <= 0) cfg.FontSize = Config.Default().FontSize;
                    if (cfg.AutoLogoutMinutes < 0) cfg.AutoLogoutMinutes = Config.Default().AutoLogoutMinutes;
                    if (string.IsNullOrWhiteSpace(cfg.Language)) cfg.Language = Config.Default().Language;
                    if (string.IsNullOrWhiteSpace(cfg.OriginalLanguage)) cfg.OriginalLanguage = cfg.Language;
                    return cfg;
                }
            }
            catch
            {
                // If anything fails, return defaults
                var def = Config.Default();
                Save(def);
                return def;
            }
        }

        public static void Save(Config cfg)
        {
            try
            {
                if (!Directory.Exists(AppFolder)) Directory.CreateDirectory(AppFolder);
                using (var stream = File.Create(ConfigFile))
                {
                    var serializer = new XmlSerializer(typeof(Config));
                    serializer.Serialize(stream, cfg);
                }
                Current = cfg;
            }
            catch
            {
                // Fails silently - callers may show message if desired
            }
        }

        public static void RestoreDefaults()
        {
            var def = Config.Default();
            Save(def);
        }
    }
}
