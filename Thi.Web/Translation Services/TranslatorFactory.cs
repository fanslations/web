using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Web
{
    public interface ITranslator
    {
        string Translate(string text, string from, string to);
    }

    public class TranslatorIdentity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsManual { get; set; }
        public ITranslator Instance { get; set; }
    }

    public class TranslatorFactory
    {
        public static ITranslator GetTranslator(int translatorID)
        {
            if (translatorID == BabelFishTranslator.Identity.ID)
            {
                return new BabelFishTranslator();
            }
            if (translatorID == BabylonTranslator.Identity.ID)
            {
                return new BabylonTranslator();
            }
            if (translatorID == BaiduTranslator.Identity.ID)
            {
                return new BaiduTranslator();
            }
            if (translatorID == BingTranslator.Identity.ID)
            {
                return new BingTranslator();
            }
            if (translatorID == ExciteTranslator.Identity.ID)
            {
                return new ExciteTranslator();
            }
            if (translatorID == FreeTranslationTranslator.Identity.ID)
            {
                return new FreeTranslationTranslator();
            }
            if (translatorID == GoogleTranslator.Identity.ID)
            {
                return new GoogleTranslator();
            }
            if (translatorID == HonyakuTranslator.Identity.ID)
            {
                return new HonyakuTranslator();
            }
            if (translatorID == InfoSeekTranslator.Identity.ID)
            {
                return new InfoSeekTranslator();
            }
            if (translatorID == LecTranslator.Identity.ID)
            {
                return new LecTranslator();
            }
            if (translatorID == SystranetTranslator.Identity.ID)
            {
                return new SystranetTranslator();
            }
            if (translatorID == YoudaoTranslator.Identity.ID)
            {
                return new YoudaoTranslator();
            }
            return new GoogleTranslator();
        }

        public static IList<TranslatorIdentity> TranslatorList()
        {
            var translators = new List<TranslatorIdentity>
            {
                new TranslatorIdentity {ID = 0, Name = "Glossary"},
                BabelFishTranslator.Identity,
                BabylonTranslator.Identity,
                BaiduTranslator.Identity,
                BingTranslator.Identity,
                ExciteTranslator.Identity,
                FreeTranslationTranslator.Identity,
                GoogleTranslator.Identity,
                HonyakuTranslator.Identity,
                InfoSeekTranslator.Identity,
                LecTranslator.Identity,
                SystranetTranslator.Identity,
                YoudaoTranslator.Identity
            };

            return translators;
        }
    }


}
