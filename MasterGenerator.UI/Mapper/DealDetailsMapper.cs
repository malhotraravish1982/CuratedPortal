using MasterGenerator.Data.Entity;

namespace MasterGenerator.UI.Mapper
{
    public class DealDetailsMapper
    {
        public static List<DealDetails> MapFromRangeData(IList<IList<object>> values, List<Project> projects)
        {
            var dealDetailList = new List<DealDetails>();
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value[0].ToString()))
                {
                    var projectId = Convert.ToInt32(value[0]);
                    var projectDealDetail = projects.Where(p => p.Id == projectId).FirstOrDefault();
                    if (projectDealDetail != null)
                    {
                        DealDetails dealDetails = new()
                        {
                            ProjectId = value.Count >= 1 ? projectId : 0,
                            CustomerName = value.Count >= 2 ? value[1].ToString().Trim() : String.Empty,
                            ProjectName = value.Count >= 3 ? value[2].ToString().Trim() : String.Empty,
                            ItemSKU = value.Count >= 4 ? value[3].ToString().Trim() : String.Empty,
                            BatchNumber = value.Count >= 5 ? value[4].ToString().Trim() : String.Empty,
                            CustomerSKU = value.Count >= 6 ? value[5].ToString().Trim() : String.Empty,
                            ItemName = value.Count >= 7 ? value[6].ToString().Trim() : String.Empty,
                            Qty = value.Count >= 8 ? value[7].ToString().Trim() : String.Empty,
                            Currency = value.Count >= 9 ? value[8].ToString().Trim() : String.Empty,
                            ApproxSellPriceExVAT = value.Count >= 10 ? value[9].ToString().Trim() : String.Empty
                        };
                        dealDetailList.Add(dealDetails);
                    }
                }
            }
            return dealDetailList;
        }
    }
}
