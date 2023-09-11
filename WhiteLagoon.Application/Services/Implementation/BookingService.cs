using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using Color = Syncfusion.Drawing.Color;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public BookingService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "User,Villa");
        }

        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public void UpdateBooking(Booking booking)
        {
            _unitOfWork.Booking.Update(booking);
            _unitOfWork.Save();
        }

        public void CancelBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCancelled;
                UpdateBooking(booking);
            }
        }

        public void CheckInBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCheckedIn;
                UpdateBooking(booking);
            }
        }

        public void CheckOutBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking != null)
            {
                booking.Status = SD.StatusCompleted;
                UpdateBooking(booking);
            }
        }

        public IEnumerable<Booking> GetBookingsByUserId(string userId)
        {
            return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User, Villa");
        }

        public IEnumerable<Booking> GetAllBookings(string? status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                return _unitOfWork.Booking.GetAll(u => !string.IsNullOrEmpty(u.Status) && u.Status.ToLower().Equals(status.ToLower()), includeProperties: "User, Villa");
            }

            return _unitOfWork.Booking.GetAll(includeProperties: "User, Villa");
        }

        public double CalculateTotalCost(int villaId, DateOnly checkInDate, int nights)
        {
            var villa = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (villa != null)
            {
                return villa.Price * nights;
            }
            return 0;
        }

        public IEnumerable<Booking> GetBookedVillasWithStatus(IEnumerable<string> statuses)
        {
            return _unitOfWork.Booking.GetAll(u => statuses.Contains(u.Status));
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == bookingId);

            if (booking != null)
            {
                booking.StripeSessionId = sessionId;
                booking.StripePaymentIntentId = paymentIntentId;
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId)
        {
            return _unitOfWork.Booking
                .GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn)
                .Select(u => u.VillaNumber);
        }
        public void UpdateStatus(int bookingId, string status, int villaNumber)
        {
            // Get the booking from the database
            var booking = _unitOfWork.Booking.Get(u => u.Id == bookingId);

            if (booking != null)
            {
                // Update the booking status
                booking.Status = status;

                if (villaNumber > 0)
                {
                    // Update the villa number if it's provided
                    booking.VillaNumber = villaNumber;
                }

                // Save the changes
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save(); // Assuming you have an async Save method
            }
        }


        public byte[] GenerateInvoiceStream(int id,  string downloadType)
        {
            string basePath = _webHostEnvironment.WebRootPath;

            WordDocument document = new ();

            // Load the template.
            string dataPath = basePath + @"/exports/BookingDetails.docx";
            using FileStream fileStream = new(dataPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            document.Open(fileStream, FormatType.Automatic);

            //Update Template
            Booking bookingFromDb = _unitOfWork.Booking.Get(c=>c.Id== id);

            TextSelection textSelection = document.Find("xx_customer_name", false, true);
            WTextRange textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Name;

            textSelection = document.Find("xx_customer_phone", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Phone;

            textSelection = document.Find("xx_customer_email", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Email;

            textSelection = document.Find("XX_BOOKING_NUMBER", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING ID - " + bookingFromDb.Id;
            textSelection = document.Find("XX_BOOKING_DATE", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING DATE - " + bookingFromDb.BookingDate.ToShortDateString();


            textSelection = document.Find("xx_payment_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.PaymentDate.ToShortDateString();
            textSelection = document.Find("xx_checkin_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckInDate.ToShortDateString();
            textSelection = document.Find("xx_checkout_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckOutDate.ToShortDateString(); ;
            textSelection = document.Find("xx_booking_total", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.TotalCost.ToString("c");

            WTable table = new(document);

            table.TableFormat.Borders.LineWidth = 1f;
            table.TableFormat.Borders.Color = Color.Black;
            table.TableFormat.Paddings.Top = 7f;
            table.TableFormat.Paddings.Bottom = 7f;
            table.TableFormat.Borders.Horizontal.LineWidth = 1f;

            int rows = bookingFromDb.VillaNumber > 0 ? 3 : 2;
            table.ResetCells(rows, 4);

            WTableRow row0 = table.Rows[0];

            row0.Cells[0].AddParagraph().AppendText("NIGHTS");
            row0.Cells[0].Width = 80;
            row0.Cells[1].AddParagraph().AppendText("VILLA");
            row0.Cells[1].Width = 220;
            row0.Cells[2].AddParagraph().AppendText("PRICE PER NIGHT");
            row0.Cells[3].AddParagraph().AppendText("TOTAL");
            row0.Cells[3].Width = 80;

            WTableRow row1 = table.Rows[1];

            row1.Cells[0].AddParagraph().AppendText(bookingFromDb.Nights.ToString());
            row1.Cells[0].Width = 80;
            row1.Cells[1].AddParagraph().AppendText(bookingFromDb.Villa.Name);
            row1.Cells[1].Width = 220;
            row1.Cells[2].AddParagraph().AppendText((bookingFromDb.TotalCost / bookingFromDb.Nights).ToString("c"));
            row1.Cells[3].AddParagraph().AppendText(bookingFromDb.TotalCost.ToString("c"));
            row1.Cells[3].Width = 80;

            if (bookingFromDb.VillaNumber > 0)
            {
                WTableRow row2 = table.Rows[2];

                row2.Cells[0].Width = 80;
                row2.Cells[1].AddParagraph().AppendText("Villa Number - " + bookingFromDb.VillaNumber.ToString());
                row2.Cells[1].Width = 220;
                row2.Cells[3].Width = 80;
            }

            WTableStyle tableStyle = document.AddTableStyle("CustomStyle") as WTableStyle;
            tableStyle.TableProperties.RowStripe = 1;
            tableStyle.TableProperties.ColumnStripe = 2;
            tableStyle.TableProperties.Paddings.Top = 2;
            tableStyle.TableProperties.Paddings.Bottom = 1;
            tableStyle.TableProperties.Paddings.Left = 5.4f;
            tableStyle.TableProperties.Paddings.Right = 5.4f;


            ConditionalFormattingStyle firstRowStyle = tableStyle.ConditionalFormattingStyles.Add(ConditionalFormattingType.FirstRow);
            firstRowStyle.CharacterFormat.Bold = true;
            firstRowStyle.CharacterFormat.TextColor = Color.FromArgb(255, 255, 255, 255);
            firstRowStyle.CellProperties.BackColor = Color.Black;

            table.ApplyStyle("CustomStyle");

            TextBodyPart bodyPart = new(document);
            bodyPart.BodyItems.Add(table);

            document.Replace("<ADDTABLEHERE>", bodyPart, false, false);


            using (DocIORenderer renderer = new())
            using (MemoryStream stream = new())
            {
                if (downloadType == "word")
                {
                    document.Save(stream, FormatType.Docx);
                }
                else
                {
                    PdfDocument pdfDocument = renderer.ConvertToPDF(document);
                    pdfDocument.Save(stream);
                }

                return stream.ToArray();
            }
        }
        
    }

}
