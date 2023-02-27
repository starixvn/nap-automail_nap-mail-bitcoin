using Fanex.Data.Repository;
using Fanex.Bitcoin.Business.Criterias;
using Fanex.Bitcoin.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Bitcoin.Business.Services
{
    public interface DataService
    {
        Tuple<IEnumerable<BitcoinModel>, IEnumerable<BitcoinModel>> Query(DateTime fromDate, DateTime toDate);
    }

    public class DataServiceImp : DataService
    {
        private readonly IDynamicRepository repository;

        public DataServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public Tuple<IEnumerable<BitcoinModel>, IEnumerable<BitcoinModel>> Query(DateTime fromDate, DateTime toDate)
        {
            var criteria = new BitcoinCriteria
            {
                FromDate = fromDate,
                ToDate = toDate
            };

            var data = repository.FetchMultiple<BitcoinModel, BitcoinModel>(criteria);
            var data1 = data.Item1.ToList();
            var data2 = data.Item2.ToList();

            return new Tuple<IEnumerable<BitcoinModel>, IEnumerable<BitcoinModel>>(data1, data2);
        }
    }
}