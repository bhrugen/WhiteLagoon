using Microsoft.AspNetCore.Mvc;
using Syncfusion.Presentation;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IVillaService villaService, IWebHostEnvironment webHostEnvironment)
        {
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _villaService.GetAllVillas(),
                Nights=1,
                CheckInDate =DateOnly.FromDateTime(DateTime.Now),
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate) 
        {
           
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = _villaService.GetVillasAvailabilityByDate(nights,checkInDate),
                Nights = nights
            };

            return PartialView("_VillaList",homeVM);
        }

        [HttpPost]
        public IActionResult GeneratePPTExport(int id)
        {
            var villa = _villaService.GetVillaById(id);
            if (villa is null)
            {
                return RedirectToAction(nameof(Error));
            }

            string basePath = _webHostEnvironment.WebRootPath;
            string filePath = basePath + @"/Exports/ExportVillaDetails.pptx";


            using IPresentation presentation = Presentation.Open(filePath);

            ISlide slide = presentation.Slides[0];


            IShape? shape = slide.Shapes.FirstOrDefault(u=>u.ShapeName== "txtVillaName") as IShape;
            if(shape is not null)
            {
                shape.TextBody.Text = villa.Name;
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }


            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy : {0} adults", villa.Occupancy);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Villa Size: {0} sqft", villa.Sqft);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night", villa.Price.ToString("C"));
            }


            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaAmenitiesHeading") as IShape;
            if(shape is not null)
            {
                List<string> listItems = villa.VillaAmenity.Select(x => x.Name).ToList();

                shape.TextBody.Text = "";

                foreach (var item in listItems)
                {
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);

                    paragraph.ListFormat.Type = ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';
                    textPart.Font.FontName = "system-ui";
                    textPart.Font.FontSize = 18;
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);

                }

            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "imgVilla") as IShape;
            if(shape is not null)
            {
                byte[] imageData;
                string imageUrl;
                try
                {
                    imageUrl= string.Format("{0}{1}", basePath, villa.ImageUrl);
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                catch (Exception)
                {
                    imageUrl = string.Format("{0}{1}", basePath, "/images/placeholder.png");
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                slide.Shapes.Remove(shape); 
                using MemoryStream imageStream = new(imageData);
                IPicture newPicture = slide.Pictures.AddPicture(imageStream, 60,120,300,200);

            }



            MemoryStream memoryStream = new();
            presentation.Save(memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream,"application/pptx","villa.pptx");


        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}