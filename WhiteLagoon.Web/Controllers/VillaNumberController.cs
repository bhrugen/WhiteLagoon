using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Application.ViewModels;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaService = villaService;
            _villaNumberService = villaNumberService;
        }

        public IActionResult Index()
        {
            var villaNumbers = _villaNumberService.GetAllVillaNumbers();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            var villas = _villaService.GetAllVillas(); // Replace with the actual method from your service
            var villaListItems = villas.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaList = villaListItems
            };

            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _villaNumberService.CheckRoomNumberExists(obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _villaNumberService.CreateVillaNumber(obj.VillaNumber);
                TempData["success"] = "The villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists.";
            }

            obj.VillaList = _villaService.GetAllVillas()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(obj);
        }


        public IActionResult Update(int villaNumberId)
        {
            VillaNumber villaNumber = _villaNumberService.GetVillaNumberById(villaNumberId);

            if (villaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _villaService.GetAllVillas()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                VillaNumber = villaNumber
            };

            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _villaNumberService.UpdateVillaNumber(villaNumberVM.VillaNumber);
                TempData["success"] = "The villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _villaService.GetAllVillas()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(villaNumberVM);
        }



        public IActionResult Delete(int villaNumberId)
        {
            VillaNumber villaNumber = _villaNumberService.GetVillaNumberById(villaNumberId);

            if (villaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaList = _villaService.GetAllVillas()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                VillaNumber = villaNumber
            };

            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber villaNumber = _villaNumberService.GetVillaNumberById(villaNumberVM.VillaNumber.Villa_Number);

            if (villaNumber == null)
            {
                TempData["error"] = "The villa number could not be deleted.";
                return RedirectToAction(nameof(Index));
            }

            _villaNumberService.DeleteVillaNumber(villaNumber.Villa_Number);

            TempData["success"] = "The villa number has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}