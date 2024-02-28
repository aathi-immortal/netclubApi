using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetClubApi.Model;

namespace NetClubApi.Modules.CourtModule
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourtController : ControllerBase
    {
        private readonly ICourtBussinessLayer _courtBussinessLayer;

        public CourtController(ICourtBussinessLayer courtBussinessLayer)
        {
            _courtBussinessLayer = courtBussinessLayer;
        }

        [HttpPost]
        [Authorize]

        public async Task<string> CreateCourt(CourtModel court)
        {
            return await _courtBussinessLayer.CreateCourt(court);
        }


    }


}
