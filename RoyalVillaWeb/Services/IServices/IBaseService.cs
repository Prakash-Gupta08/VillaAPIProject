using RoyalVillaWeb.Models;
using RoyalVilla.DTO;
using VillaWebAPI.DTO;


namespace RoyalVillaWeb.Services.IServices
{
    public interface IBaseService
    {
    
        ApiResponse<object> ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
