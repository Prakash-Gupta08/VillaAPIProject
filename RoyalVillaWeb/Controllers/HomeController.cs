using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;
using VillaWebAPI.DTO;

namespace RoyalVillaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
            
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDto> villaList = new();
            try
            {
                var res = await _villaService.GetAllAsync<ApiResponse<List<VillaDto>>>("");
                if (res != null && res.Success && res.Data != null)
                {
                    villaList = res.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(villaList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
