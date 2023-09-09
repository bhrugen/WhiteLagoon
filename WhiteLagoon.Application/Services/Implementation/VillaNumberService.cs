using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
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

        public bool CheckVillaNumberExists(int villa_Number)
        {
            return _unitOfWork.VillaNumber
               .Any(u => u.Villa_Number == villa_Number);
        }

        public void CreateVillaNumber(VillaNumber villaNumber)
        {
            ArgumentNullException.ThrowIfNull(villaNumber);

            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();
        }

        public bool DeleteVillaNumber(int id)
        {
            try
            {
                var villaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == id);

                if (villaNumber != null)
                {
                    
                    _unitOfWork.VillaNumber.Remove(villaNumber);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Villa number with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public IEnumerable<VillaNumber> GetAllVillaNumbers()
        {
            return _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        }

        public VillaNumber GetVillaNumberById(int id)
        {
            return _unitOfWork.VillaNumber.Get(u => u.Villa_Number == id, includeProperties: "Villa");
        }

        public void UpdateVillaNumber(VillaNumber villaNumber)
        {
            ArgumentNullException.ThrowIfNull(villaNumber);

            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();
        }

        
    }
}
