using Pitch.Domain.Enums;

namespace Order.API.ViewModels.Dashboard.Response
{
    public class PitchTypeDashboardModel
    {
        public PitchTypeEnum PitchType { get; set; }
        public int Revenue { get; set; }
    }
}
