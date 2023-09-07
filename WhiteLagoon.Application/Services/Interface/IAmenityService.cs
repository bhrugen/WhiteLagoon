using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IAmenityService
    {
        IEnumerable<Amenity> GetAllAmenities();
        void CreateAmenity(Amenity amenity);
        void UpdateAmenity(Amenity amenity);
        Amenity GetAmenityById(int amenityId);
        void DeleteAmenity(int amenityId);
    }

}
