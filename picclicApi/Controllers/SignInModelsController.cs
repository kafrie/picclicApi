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
                var user = db.SignInModels.SingleOrDefault(u => u.UserId == signInModel.UserId);
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
                if (SignInModelExists(signInModel.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = signInModel.UserId }, signInModel);
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
            return db.SignInModels.Count(e => e.UserId == id) > 0;
        }
    }
}