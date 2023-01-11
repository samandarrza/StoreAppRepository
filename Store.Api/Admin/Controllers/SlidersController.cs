using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Admin.Dtos.CategoryDtos;
using Store.Api.Admin.Dtos.ProductDtos;
using Store.Api.Admin.Dtos.SliderDtos;
using Store.Api.Helpers;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.Repositories;
using System.Data;

namespace Store.Api.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISliderRepository _sliderRepository;
        private readonly IWebHostEnvironment _env;

        public SlidersController(IMapper mapper, ISliderRepository sliderRepository,IWebHostEnvironment env)
        {
            _mapper = mapper;
            _sliderRepository = sliderRepository;
            _env = env;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] SliderPostDto postDto)
        {

            if (await _sliderRepository.IsExistAsync(x => x.Title == postDto.Title))
                return BadRequest(new { error = new { field = "Name", message = "Slider already exist!" } });


            Slider slider = _mapper.Map<Slider>(postDto);
            slider.Image = FileManager.Save(postDto.ImageFile, _env.WebRootPath, "uploads/sliders");

            await _sliderRepository.AddAsync(slider);
            await _sliderRepository.CommitAsync();

            return StatusCode(201, slider);
        }

    }
}
