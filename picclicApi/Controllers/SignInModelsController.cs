using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using picclicApi.Models;
using picclicApi.Shared;

namespace picclicApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SignInModelsController : ApiController
    {
        private picclicApiContext db = new picclicApiContext();
        private readonly HashAndVerifyPsw _pswHashing = new HashAndVerifyPsw();

        // GET: api/SignInModels
        public IQueryable<SignInModel> GetSignInModels()
        {
            return db.SignInModels;
        }

        // GET: api/SignInModels/5
        [ResponseType(typeof(SignInModel))]
        public async Task<IHttpActionResult> GetSignInModel(string userName)
        {
            var signInModel = await db.SignInModels.FirstOrDefaultAsync(x=>x.UserName==userName);

            if (signInModel == null)
            {
                return NotFound();
            }

            return Ok(signInModel);
        }

        // POST: api/SignInModels
        [ResponseType(typeof(SignInModel))]
        public async Task<IHttpActionResult> PostSignInModel(SignInModel signInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //db.SignInModels.Add(signInModel);

            try
            {
                var user = db.SignInModels.SingleOrDefault(u => u.UserName == signInModel.UserName);
                if (user != null)
                {
                    var verifyPsw = _pswHashing.VerifyHash(signInModel.Password, user.Password);
                    if (verifyPsw)
                    {
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            catch (DbUpdateException)
            {
                if (SignInModelExists(signInModel.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new
            {
                id = signInModel.UserId,
                username = signInModel.UserName
            }, signInModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SignInModelExists(string id)
        {
            return db.SignInModels.Count(e => e.UserName == id) > 0;
        }
    }
}