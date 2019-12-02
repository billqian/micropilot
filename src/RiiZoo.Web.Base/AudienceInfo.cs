using System;
using System.Collections.Generic;
using System.Text;

namespace RiiZoo.Web
{
    public class AudienceInfo
    {
        public string Issuer { get; set; } = "Riizoo";
        public string Audience { get; set; } = "everyone";
        public string Secret { get; set; } = "12345678901234561234567890123456";
    }

    public interface IAudienceInfoLoader
    {
        AudienceInfo LoadAudienceInfo();
    }

    public class BlankAudienceInfoLoader : IAudienceInfoLoader
    {
        public AudienceInfo LoadAudienceInfo()
        {
            return new AudienceInfo();
        }
    }
}
