using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoManager.Contracts.Database;
using PhotoManager.Contracts.Logic;

namespace PhotoManager.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IPhotoConverter _photoConverter;
        private readonly ILogger<PhotoController> _logger;
        
        public PhotoController(ILogger<PhotoController> logger, IDatabaseContext databaseContext, 
            IPhotoConverter photoConverter)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _photoConverter = photoConverter;
        }
        
        [HttpPost]
        public async Task AddPhoto(IFormFile photoModel)
        {
            var photo = await _photoConverter.ConvertToPhoto(photoModel.OpenReadStream(), photoModel.FileName, DateTime.Now);
            await _databaseContext.ExecuteAsync(async context => { await context.PhotoRepository.SaveAsync(photo); });
        }
    }
}
