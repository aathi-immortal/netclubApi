using Microsoft.AspNetCore.Mvc;
using NetClubApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetClubApi.Modules.CourtModule
{
    [Route("api/[controller]/action")]
    [ApiController]
    public class CourtController : ControllerBase
    {
        private readonly ICourtBussinessLayer _courtBussinessLayer;

        public CourtController(ICourtBussinessLayer courtBussinessLayer)
        {
            _courtBussinessLayer = courtBussinessLayer;
        }

        [HttpPost]
        public async Task<string> CreateCourt(CourtModel court)
        {
            return await _courtBussinessLayer.CreateCourt(court);
        }

        [HttpGet]
        public async Task<List<CourtModel>> GetAllCourts()
        {
            return await _courtBussinessLayer.GetAllCourts();
        }
    }
}