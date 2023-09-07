using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Application.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
	[Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IVillaService _villaService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);            
        }       

        public IActionResult Create()
        {
			AmenityVM amenityVM = new()
			{
                VillaList = _villaService.GetVillaSelectListItems(),
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid && amenityVM.Amenity !=null)
            {                
                _amenityService.CreateAmenity(amenityVM.Amenity);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _villaService.GetVillaSelectListItems();

            return View(amenityVM);
        }

        public IActionResult Update(int amenityId)
        {
            var amenity = _amenityService.GetAmenityById(amenityId);

            if (amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var amenityVM = new AmenityVM
            {
                VillaList = _villaService.GetVillaSelectListItems(),
                Amenity = amenity
            };

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid && amenityVM.Amenity != null)
            {
                _amenityService.UpdateAmenity(amenityVM.Amenity);
                TempData["success"] = "The amenity has been updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _villaService.GetVillaSelectListItems();

            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            var amenity = _amenityService.GetAmenityById(amenityId);

            if (amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var amenityVM = new AmenityVM
            {
                VillaList = _villaService.GetVillaSelectListItems(),
                Amenity = amenity
            };

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            if(amenityVM== null || amenityVM.Amenity == null)
            {
                TempData["error"] = "The amenity could not be deleted.";
                return View();
            }

            _amenityService.DeleteAmenity(amenityVM.Amenity.Id);
            TempData["success"] = "The amenity has been deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}