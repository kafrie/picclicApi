using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using picclicApi.Models;
using picclicApi.Shared;
using System.Web.Http.Cors;

namespace picclicApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public async Task<IHttpActionResult> GetSignUpUserModel(string id)
        {
            var signUpUserModel = await db.SignUpUserModels.FindAsync(id);

            if (signUpUserModel == null)
            {
                return NotFound();
            }

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
            
           

            

            try
            {
                var updateSignUp = new SignUpUserModel
                {
                    UserId = signUpUserModel.UserId,
                    Name = signUpUserModel.Name,
                    Surname = signUpUserModel.Surname,
                    UserName = signUpUserModel.UserName,
                    Email = signUpUserModel.Email
                };
                db.SignUpUserModels.Attach(updateSignUp);
                var updateSignupEntry = db.Entry(updateSignUp);

                foreach (var value in updateSignupEntry.OriginalValues.PropertyNames)
                {
                    updateSignupEntry.Property(value).IsModified = value != "Password";
                }

                await db.SaveChangesAsync();

                var updateSignIn = new SignInModel
                {
                    UserId = signUpUserModel.UserId,
                    UserName = signUpUserModel.UserName
                };
                db.SignInModels.Attach(updateSignIn);
                var updateSigninEntry = db.Entry(updateSignUp);
                foreach (var value in updateSigninEntry.OriginalValues.PropertyNames)
                {
                    updateSigninEntry.Property(value).IsModified = value != "Password";
                }

                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SignUpUserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    var t = ex.Message;
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
                var userId = Guid.NewGuid().ToString();
                var hashedPsw = _pswHashing.ComputeHash(signUpUserModel.Password);

                db.SignInModels.Add(new SignInModel
                {
                    UserId = userId,
                    UserName = signUpUserModel.UserName,
                    Password = hashedPsw
                });

                db.SignUpUserModels.Add(new SignUpUserModel
                {
                    Name = signUpUserModel.Name,
                    Surname = signUpUserModel.Surname,
                    UserId = userId,
                    UserName = signUpUserModel.UserName,
                    Email = signUpUserModel.Email,
                    Password = hashedPsw
                });
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
                if (SignUpUserModelExists(signUpUserModel.UserName))
                {
                    return StatusCode(HttpStatusCode.Conflict);
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