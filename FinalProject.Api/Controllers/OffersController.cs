﻿using AutoMapper;
using FinalProject.Base;
using FinalProject.Business;
using FinalProject.DTO;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : CustomBaseController
    {
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public OffersController(IOfferService offerService, IMapper mapper)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Offer> offers = await _offerService.GetAllAsync();

            List<OfferListDto> offerListDtos = _mapper.Map<List<OfferListDto>>(offers);

            return CreateActionResult(CustomResponseDto<List<OfferListDto>>.Success(200,offerListDtos,"Tüm Teklifler Listelendi"));
        }


        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActiveAsync()
        {
            List<Offer> offers = await _offerService.GetActiveAsync();

            List<OfferListDto> offerListDtos = _mapper.Map<List<OfferListDto>>(offers);

            return CreateActionResult(CustomResponseDto<List<OfferListDto>>.Success(200, offerListDtos, "Active Teklifler Listelendi"));
        }

        [HttpGet("GetPassive")]
        public async Task<IActionResult> GetPassiveAsync()
        {
            List<Offer> offers = await _offerService.GetActiveAsync();

            List<OfferListDto> offerListDtos = _mapper.Map<List<OfferListDto>>(offers);

            return CreateActionResult(CustomResponseDto<List<OfferListDto>>.Success(200, offerListDtos, "Passive Teklifler Listelendi"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
        {
            Offer offer =await _offerService.GetByIDAsync(id);

            OfferDto offerDto = _mapper.Map<OfferDto>(offer);

            return CreateActionResult(CustomResponseDto<OfferDto>.Success(200, offerDto, $"{id} numaralı teklif Listelendi"));
        }

        [HttpPost]
        public IActionResult Add([FromBody]OfferAddDto offerAddDto)
        {
            Offer offer=_mapper.Map<Offer>(offerAddDto);

            _offerService.Add(offer);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "Eklem İşlemi Başarılı"));
        }

        [HttpPut]
        public IActionResult Update([FromBody]OfferUpdateDto offerUpdateDto)
        {
            Offer offer = _mapper.Map<Offer>(offerUpdateDto);

            _offerService.Update(offer);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "Güncelleme İşlemi Başarılı"));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            _offerService.Delete(id);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204,"Silme İşlemi Başarılı"));
        }
    }
}