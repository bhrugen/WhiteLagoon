using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IVillaNumberService
    {
        IEnumerable<VillaNumber> GetAllVillaNumbers();
        VillaNumber GetVillaNumberById(int id);
        void CreateVillaNumber(VillaNumber villaNumber);
        void UpdateVillaNumber(VillaNumber villaNumber);
        void DeleteVillaNumber(int id);

        List<VillaNumber> GetVillaNumbersByAvailability(int villaId, List<int> availableVillaNumbers);
        List<VillaNumber> GetAllVillaNumbersByVillaId(int villaId);
		bool CheckRoomNumberExists(int villa_Number);
	}
}
