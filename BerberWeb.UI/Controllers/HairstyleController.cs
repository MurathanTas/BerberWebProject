using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BerberWeb.UI.Controllers
{
    public class HairstyleController : Controller
    {
        private const string ApiUrl = "https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle";
        private const string ApiKey = "";
        private const string ApiHost = "hairstyle-changer.p.rapidapi.com";

        // Saç stili seçme ve fotoğraf yükleme sayfası
        public IActionResult Index()
        {
            ViewBag.HairStyles = GetHairStyles();
            return View();
        }

        // Resmi işleyen ve API'ye gönderen metod
        [HttpPost]
        public IActionResult ProcessImage(IFormFile uploadedImage, string style)
        {
            if (uploadedImage == null || uploadedImage.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen bir fotoğraf yükleyin.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Resmi byte array'e çevirme
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    uploadedImage.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                // API isteği oluşturma ve gönderme
                var response = SendApiRequest(imageBytes, uploadedImage.FileName, style);

                if (response.IsSuccessful)
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    string base64Image = responseData?.data?.image;

                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        ViewBag.ProcessedImage = $"data:image/png;base64,{base64Image}";
                        return View("Result");
                    }

                    TempData["ErrorMessage"] = "İşlenmiş resim alınamadı.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Resim işleme başarısız oldu. Hata: {response.Content}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // API çağrısını yapan yardımcı metod
        private RestResponse SendApiRequest(byte[] imageBytes, string fileName, string style)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("x-rapidapi-key", ApiKey);
            request.AddHeader("x-rapidapi-host", ApiHost);
            request.AddHeader("Content-Type", "multipart/form-data");

            request.AddFile("image_target", imageBytes, fileName);
            request.AddParameter("hair_type", style);

            return client.Execute(request);
        }

        // Saç stilleri sözlüğünü döndüren yardımcı metod
        private Dictionary<int, string> GetHairStyles()
        {
            return new Dictionary<int, string>
            {
                { 101, "Kakül (Varsayılan)" },
                { 201, "Uzun Saç" },
                { 301, "Kakül + Uzun Saç" },
                { 401, "Orta Uzunlukta Saç" },
                { 402, "Hafif Saç Artışı" },
                { 403, "Yoğun Saç Artışı" },
                { 502, "Hafif Kıvırcık" },
                { 503, "Yoğun Kıvırcık" },
                { 603, "Kısa Saç" },
                { 801, "Sarı Saç" },
                { 901, "Düz Saç" },
                { 1001, "Yağsız Saç" },
                { 1101, "Saç Çizgisi Dolgusu" },
                { 1201, "Düzgün Saç" },
                { 1301, "Saç Boşluklarını Doldurma" }
            };
        }
    }
}

