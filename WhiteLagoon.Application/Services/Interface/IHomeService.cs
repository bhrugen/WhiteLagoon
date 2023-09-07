using WhiteLagoon.Application.ViewModels;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IHomeService
    {
        HomeVM GetHomeViewModel();
        HomeVM GetVillasByDate(int nights, DateOnly checkInDate);
    }
}
