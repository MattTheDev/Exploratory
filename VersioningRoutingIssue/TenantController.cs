using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TestSolution.Api.Controllers
{
    public class TenantController : Controller
    {
        /*********************************************************************************/
        /* Problem */
        /*********************************************************************************/

        // The versioned endpoint works.
        // The non-versioned endpoint throws a 400 error.
        [ApiVersion("2.0")] // Note - If this is 1.0 .. this works. However, I had to have a 2.0 version for backwards compatibility. Don't ask lol
        [HttpPost]
        [Route("api/v{version:apiVersion}/[controller]/{stockpileId:int}/[action]")]
        [Route("api/[controller]/{stockpileId:int}/[action]")]
        public async Task<IActionResult> CreateAsync([FromRoute]int stockpileId, [FromBody]TenantDto tenantDto)
        {
            try
            {
                await tenantAccessor.CreateAsync(tenantDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /*********************************************************************************/
        /* Solution */
        /*********************************************************************************/

        // Both work if we do this ...
        [ApiVersion("1.0")]
        [HttpPost]
        [Route("api/[controller]/{stockpileId:int}/create")]
        public async Task<IActionResult> CreateV1Async([FromRoute] int stockpileId, [FromBody] TenantDto tenantDto)
        {
            try
            {
                await tenantAccessor.CreateAsync(tenantDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [ApiVersion("2.0")]
        [HttpPost]
        [Route("api/v{version:apiVersion}/[controller]/{stockpileId:int}/create")]
        public async Task<IActionResult> CreateV2Async([FromRoute] int stockpileId, [FromBody] TenantDto tenantDto)
        {
            return await CreateV1Async(stockpileId, tenantDto);
        }
    }
}
