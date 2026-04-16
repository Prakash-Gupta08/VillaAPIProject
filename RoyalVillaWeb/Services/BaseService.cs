using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using VillaWebAPI.DTO;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse<object> ResponseModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            throw new NotImplementedException();
        }
    }
}
