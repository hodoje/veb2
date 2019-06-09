using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class PricelistsController : ApiController
    {
		private IUnitOfWork unitOfWork;
		private IMapper mapper;

		public PricelistsController(IUnitOfWork uow, IMapper imapper)
		{
			unitOfWork = uow;
			mapper = imapper;
		}

		// GET: api/Pricelists
		public IEnumerable<Pricelist> GetPricelists()
        {
			return unitOfWork.PricelistRepository.GetAll();
        }

        // GET: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult GetPricelist(int id)
        {
			Pricelist pricelist = unitOfWork.PricelistRepository.Get(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return Ok(pricelist);
        }

		[HttpGet]
		[Route("api/pricelists/getActivePricelist")]
		public IHttpActionResult GetLatestPricelist()
		{
			Pricelist pricelist = unitOfWork.PricelistRepository.GetActivePricelist();
			AdminPricelistDto pricelistDto = mapper.Map<Pricelist, AdminPricelistDto>(pricelist);
			List<TicketTypePricelist> pltts = unitOfWork.TicketTypePricelistRepository.FindIncludeTicketType(pltt => pltt.PricelistId == pricelist.Id).ToList();
			foreach(TicketTypePricelist pltt in pltts)
			{
				if (pltt.TicketType.Name == "Hourly")
				{
					pricelistDto.Hourly = pltt.BasePrice;
				}
				else if (pltt.TicketType.Name == "Daily")
				{
					pricelistDto.Daily = pltt.BasePrice;
				}
				else if (pltt.TicketType.Name == "Monthly")
				{
					pricelistDto.Monthly = pltt.BasePrice;
				}
				else if (pltt.TicketType.Name == "Yearly")
				{
					pricelistDto.Yearly = pltt.BasePrice;
				}
			}
			if (pricelist == null)
			{
				return NotFound();
			}
			return Ok(pricelistDto);
		}

        // PUT: api/Pricelists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPricelist(int id, Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricelist.Id)
            {
                return BadRequest();
            }

            try
            {
				unitOfWork.PricelistRepository.Update(pricelist);
				unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricelistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pricelists
        [ResponseType(typeof(AdminPricelistDto))]
        public IHttpActionResult PostPricelist(AdminPricelistDto pricelistDto)
        {
			Pricelist pricelist = mapper.Map<AdminPricelistDto, Pricelist>(pricelistDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			unitOfWork.PricelistRepository.Add(pricelist);
			unitOfWork.Complete();

			foreach(TicketType tt in unitOfWork.TicketTypeRepository.GetAll())
			{
				TicketTypePricelist pltt = new TicketTypePricelist(tt.Id, pricelist.Id);
				if(tt.Name == "Hourly")
				{
					pltt.BasePrice = pricelistDto.Hourly;
				}
				else if(tt.Name == "Daily")
				{
					pltt.BasePrice = pricelistDto.Daily;
				}
				else if(tt.Name == "Monthly")
				{
					pltt.BasePrice = pricelistDto.Monthly;
				}
				else if(tt.Name == "Yearly")
				{
					pltt.BasePrice = pricelistDto.Yearly;
				}
				unitOfWork.TicketTypePricelistRepository.Add(pltt);
			}
			unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = pricelist.Id }, pricelist);
        }

        // DELETE: api/Pricelists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult DeletePricelist(int id)
        {
            Pricelist pricelist = unitOfWork.PricelistRepository.Get(id);
            if (pricelist == null)
            {
                return NotFound();
            }

			unitOfWork.PricelistRepository.Remove(pricelist);
			unitOfWork.Complete();

            return Ok(pricelist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PricelistExists(int id)
        {
            return unitOfWork.PricelistRepository.Get(id) != null;
        }
    }
}