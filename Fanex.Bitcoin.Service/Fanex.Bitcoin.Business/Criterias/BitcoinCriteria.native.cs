using Fanex.Data.Repository;
using System;

namespace Fanex.Bitcoin.Business.Criterias
{
    public class BitcoinCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "Acc_Rpt_Automail_Bitcoin";

        public override bool IsValid() => true;
    }
}