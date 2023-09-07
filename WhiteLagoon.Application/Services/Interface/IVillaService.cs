﻿using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas();
        void CreateVilla(Villa villa);
        void UpdateVilla(Villa villa);
        Villa GetVillaById(int villaId);
        bool DeleteVilla(int villaId);

        IEnumerable<SelectListItem> GetVillaSelectListItems();

        int GetAvailableRoomCount(int villaId, List<VillaNumber> villaNumbersList, DateOnly checkInDate, int nights, List<Booking> bookedVillas);


    }
}
