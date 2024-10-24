using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlicundeApi.Models
{
    public class Fees
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }
        public DateTime TimestampUTC { get; set; }
        public string Country { get; set; }
        public decimal? ImbalanceFeeValue { get; set; }
        public decimal? HourlyImbalanceFee { get; set; }
        public decimal? PeakLoadFee { get; set; }
        public decimal? VolumeFee { get; set; }
        public decimal WeeklyFee { get; set; }
    }
}
