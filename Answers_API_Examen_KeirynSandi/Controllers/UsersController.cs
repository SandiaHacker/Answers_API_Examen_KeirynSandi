using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Answers_API_Examen_KeirynSandi.Models;
using Answers_API_Examen_KeirynSandi.ModelsDTOs;

namespace Answers_API_Examen_KeirynSandi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AnswersDbContext _context;

        public UsersController(AnswersDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("GetUserInfoByEmail")]
        public ActionResult<IEnumerable<UsuarioDTO>> GetUserInfoByEmail(string pEmail)
        {
            //acá vamos a imitar la misma consulta qyuee hicimos en SSMS
            //pero usando Linq

            var query = (from u in _context.Users
                         join ur in _context.UserRoles
                         on u.UserRoleId equals ur.UserRoleId
                         where u.BackUpEmail == pEmail
                         select new
                         {
                             id = u.UserId,
                             correo = u.BackUpEmail,
                             nombre = u.UserName,
                             telefono = u.PhoneNumber,
                             contrasennia = u.UserPassword,
                             rolid = u.UserRoleId,
                         }
                         ).ToList();

            List<UsuarioDTO> list = new List<UsuarioDTO>();

            foreach (var item in query)
            {
                UsuarioDTO nuevoUsuario = new UsuarioDTO()
                {
                    UsuarioID = item.id,
                    Correo = item.correo,
                    Nombre = item.nombre,
                    Telefono = item.telefono,
                    Contrasennia = item.contrasennia,
                    RolID = item.rolid
                };
                list.Add(nuevoUsuario);
            }

            if (list == null) { return NotFound(); }

            return list;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpPost("AddUserFromApp")]
        public async Task<ActionResult<UsuarioDTO>> AddUserFromApp(UsuarioDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //normalmente usamos herramientas como auto mapper para hacer la transformación del
            //DTO al modelo nativo (en este caso User). Pero para entender mejor o por mayor
            //control áca haremos el mapeo manualmente

            User NuevoUsuarioNativo = new()
            {
                BackUpEmail = user.Correo,
                UserName = user.Nombre,
                PhoneNumber = user.Telefono,
                UserPassword = user.Contrasennia,
                UserRoleId = user.RolID,
                UserRole = null
            };
            _context.Users.Add(NuevoUsuarioNativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = NuevoUsuarioNativo.UserId }, NuevoUsuarioNativo);

        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
