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
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers.DomainControllers
{
    public class UserTypesController : ApiController
    {
        private IUnitOfWork unitOfWork;

        public UserTypesController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        // GET: api/UserTypes
        public IEnumerable<UserType> GetUserTypes()
        {
            return unitOfWork.UserTypeRepository.GetAll();
        }

        // GET: api/UserTypes/5
        [ResponseType(typeof(UserType))]
        public IHttpActionResult GetUserType(int id)
        {
            UserType userType = unitOfWork.UserTypeRepository.Get(id);
            if (userType == null)
            {
                return NotFound();
            }

            return Ok(userType);
        }

        // PUT: api/UserTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserType(int id, UserType userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userType.Id)
            {
                return BadRequest();
            }

            try
            {
                unitOfWork.UserTypeRepository.Update(userType);
                unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypeExists(id))
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

        // POST: api/UserTypes
        [ResponseType(typeof(UserType))]
        public IHttpActionResult PostUserType(UserType userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitOfWork.UserTypeRepository.Add(userType);
            unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = userType.Id }, userType);
        }

        // DELETE: api/UserTypes/5
        [ResponseType(typeof(UserType))]
        public IHttpActionResult DeleteUserType(int id)
        {
            UserType userType = unitOfWork.UserTypeRepository.Get(id);
            if (userType == null)
            {
                return NotFound();
            }
            
            unitOfWork.UserTypeRepository.Remove(userType);
            unitOfWork.Complete();

            return Ok(userType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTypeExists(int id)
        {
            return unitOfWork.UserTypeRepository.Get(id) != null;
        }
    }
}