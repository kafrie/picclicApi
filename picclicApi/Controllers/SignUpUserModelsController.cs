using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using picclicApi.Models;
using picclicApi.Shared;

namespace picclicApi.Controllers
{
    public class SignUpUserModelsController : ApiController
    {
        private picclicApiContext db = new picclicApiContext();
        private readonly HashAndVerifyPsw _pswHashing = new HashAndVerifyPsw();

        // GET: api/SignUpUserModels
        public IQueryable<SignUpUserModel> GetSignUpUserModels()
        {
            return db.SignUpUserModels;
        }

        // GET: api/SignUpUserModels/5
        [ResponseType(typeof(SignUpUserModel))]
        public async Task<IHttpActionResult> GetSignUpUserModel(string id, string pswToVerify)
        {
            var signUpUserModel = await db.SignUpUserModels.FindAsync(id);

            if (signUpUserModel == null)
            {
                return NotFound();
            }

            var verifyPsw = _pswHashing.VerifyHash(pswToVerify, signUpUserModel.Password);

            return Ok(signUpUserModel);
        }

        // PUT: api/SignUpUserModels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSignUpUserModel(string id, SignUpUserModel signUpUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != signUpUserModel.UserId)
            {
                return BadRequest();
            }

            db.Entry(signUpUserModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignUpUserModelExists(id))
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

        // POST: api/SignUpUserModels
        [ResponseType(typeof(SignUpUserModel))]
        public async Task<IHttpActionResult> PostSignUpUserModel(SignUpUserModel signUpUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var hashedPsw = _pswHashing.ComputeHash(signUpUserModel.Password);

                signUpUserModel = new SignUpUserModel
                {
                    Name = signUpUserModel.Name,
                    Surname = signUpUserModel.Surname,
                    UserId = signUpUserModel.UserId,
                    Email = signUpUserModel.Email,
                    Password = hashedPsw
                };

                db.SignUpUserModels.Add(signUpUserModel);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SignUpUserModelExists(signUpUserModel.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = signUpUserModel.UserId }, signUpUserModel);
        }

        // DELETE: api/SignUpUserModels/5
        [ResponseType(typeof(SignUpUserModel))]
        public async Task<IHttpActionResult> DeleteSignUpUserModel(string id)
        {
            SignUpUserModel signUpUserModel = await db.SignUpUserModels.FindAsync(id);
            if (signUpUserModel == null)
            {
                return NotFound();
            }

            db.SignUpUserModels.Remove(signUpUserModel);
            await db.SaveChangesAsync();

            return Ok(signUpUserModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SignUpUserModelExists(string id)
        {
            return db.SignUpUserModels.Count(e => e.UserId == id) > 0;
        }
    }
}