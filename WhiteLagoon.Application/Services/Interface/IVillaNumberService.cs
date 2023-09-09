using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IVillaNumberService
    {
        IEnumerable<VillaNumber> GetAllVillaNumbers();
        IEnumerable<VillaNumber> GetAllVillaNumbersByVillaId(int villaId);

        void CreateVillaNumber(VillaNumber villaNumber);
        void UpdateVillaNumber(VillaNumber villaNumber);
        VillaNumber GetVillaNumberById(int id);
        
        bool DeleteVillaNumber(int id);

        bool CheckVillaNumberExists(int villa_Number);

    }
}
