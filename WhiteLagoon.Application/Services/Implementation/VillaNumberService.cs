using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaNumberService : IVillaNumberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VillaNumber GetVillaNumberById(int id)
        {
            return _unitOfWork.VillaNumber.Get(u => u.VillaId == id);
        }

        public void CreateVillaNumber(VillaNumber villaNumber)
        {
            if (villaNumber == null)
            {
                throw new ArgumentNullException(nameof(villaNumber));
            }

            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();
        }

        public void UpdateVillaNumber(VillaNumber villaNumber)
        {
            if (villaNumber == null)
            {
                throw new ArgumentNullException(nameof(villaNumber));
            }

            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();
        }


        public void DeleteVillaNumber(int id)
        {
        var villaNumberToDelete = _unitOfWork.VillaNumber.Get(u => u.VillaId == id);

        if (villaNumberToDelete != null)
        {
            _unitOfWork.VillaNumber.Remove(villaNumberToDelete);
            _unitOfWork.Save();
        }
        else
        {
            throw new InvalidOperationException($"Villa number with ID {id} not found.");
        }
    }
        public IEnumerable<VillaNumber> GetAllVillaNumbers()
        {
            return _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        }

        public List<VillaNumber> GetVillaNumbersByAvailability(int villaId, List<int> availableVillaNumbers)
        {
            return _unitOfWork.VillaNumber
                .GetAll(u => u.VillaId == villaId && availableVillaNumbers.Contains(u.Villa_Number))
                .ToList();
        }

        public List<VillaNumber> GetAllVillaNumbersByVillaId(int villaId)
        {
            return _unitOfWork.VillaNumber
                .GetAll(u => u.VillaId == villaId)
                .ToList();
        }

		public bool CheckRoomNumberExists(int villa_Number)
		{
            return _unitOfWork.VillaNumber
                .Any(u => u.Villa_Number == villa_Number);
        }
	}

}
