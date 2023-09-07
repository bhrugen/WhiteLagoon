using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Amenity> GetAllAmenities()
        {
            return _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
        }

        public void CreateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Add(amenity);
            _unitOfWork.Save();
        }

        public void UpdateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Update(amenity);
            _unitOfWork.Save();
        }

        public Amenity GetAmenityById(int amenityId)
        {
            return _unitOfWork.Amenity.Get(u => u.Id == amenityId);
        }

        public void DeleteAmenity(int amenityId)
        {
            var amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId);

            if (amenity != null)
            {
                _unitOfWork.Amenity.Remove(amenity);
                _unitOfWork.Save();
            }
        }
    }

}
