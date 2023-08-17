using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProtocolsAPI_JosephGF.Attributes;
using MyProtocolsAPI_JosephGF.Models;
using MyProtocolsAPI_JosephGF.ModelsDTOs;

namespace MyProtocolsAPI_JosephGF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Apikey]
    public class UsersController : ControllerBase
    {
        private readonly MyProtocolsDBContext _context;

        public UsersController(MyProtocolsDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        //este get valida el usuario que quiere ingresar en la app
        //GET: api/Users
        [HttpGet("ValidateLogin")]
        public async Task<ActionResult<User>> ValidateLogin(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(e => e.Email.Equals
            (username) && e.Password == password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("GetUserInfoByEmail")]
        public ActionResult<IEnumerable<UserDTO>> GetUserInfoByEmail(string Pemail) 
        {
            //aqui se crea un linq que combina info de 2 entidades
            //(user inner join userrole) y la agrega en el objeto dto de usuario
            var query = (from u in _context.Users
                         join ur in _context.UserRoles on 
                         u.UserRoleId equals ur.UserRoleId
                         where u.Email == Pemail && u.Active == true &&
                         u.IsBlocked == false 
                         select new 
                         {
                             idusuario = u.UserId,
                             correo = u.Email,
                             contrasenia = u.Password,
                             nombre = u.Name,
                             correorespaldo = u.BackUpEmail,
                             telefono = u.PhoneNumber,
                             direccion = u.Address,
                             activo = u.Active,
                             establoqueado = u.IsBlocked,
                             idrol = ur.UserRoleId,
                             descripcionrol = ur.Description
                         }).ToList();
            //creamos un objeto del tipo que retorna la funcion/ruta
            List<UserDTO> list = new List<UserDTO> ();
            foreach (var item in query)
            { 
                UserDTO NewItem = new UserDTO()
                {
                    IDUsuario = item.idusuario,
                    Correo = item.correo,
                    Contrasenia = item.contrasenia,
                    Nombre = item.nombre,
                    CorreoRespaldo = item.correorespaldo,
                    Telefono = item.telefono,
                    Direccion = item.direccion,
                    Activo = item.activo,
                    EstaBloqueado = item.establoqueado,
                    IdRol = item.idrol,
                    DescripcionRol = item.descripcionrol
                };
                list.Add (NewItem);
            }
            if (list == null) { return NotFound(); }
            return list;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO user)
        {
            if (id != user.IDUsuario)
            {
                return BadRequest();
            }


            //tenemos que hacer la convercion entre el dto que llega en formato
            //json  en el header y el objeto que entity framework entiende que es
            //de tipo user

            User? NewEFUser = GetUSerByID(id);
            if (NewEFUser != null)
            {
                NewEFUser.Email = user.Correo;
                NewEFUser.Name = user.Nombre;
                NewEFUser.BackUpEmail = user.CorreoRespaldo;
                NewEFUser.PhoneNumber = user.Telefono;
                NewEFUser.Address = user.Direccion;

                _context.Entry(NewEFUser).State = EntityState.Modified;
            }

            //User NewEFUser = new() 
            //{
            //    UserId = user.IDUsuario,
            //    Email = user.Correo,
            //    Name = user.Nombre,
            //    BackUpEmail = user.CorreoRespaldo,
            //    PhoneNumber = user.Telefono,
            //    Address = user.Direccion,

            //}; 

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

            return Ok();
        }

        //[HttpPut("{idPassword}")]
        //public async Task<IActionResult> PutUserPassword(int id, UserDTO user)
        //{
        //    if (id != user.IDUsuario)
        //    {
        //        return BadRequest();
        //    }


        //    //tenemos que hacer la convercion entre el dto que llega en formato
        //    //json  en el header y el objeto que entity framework entiende que es
        //    //de tipo user

        //    User? NewEFUser = GetUSerByID(id);
        //    if (NewEFUser != null)
        //    {
        //        NewEFUser.Password = user.Contrasenia;

        //        _context.Entry(NewEFUser).State = EntityState.Modified;
        //    }

        //    //User NewEFUser = new() 
        //    //{
        //    //    UserId = user.IDUsuario,
        //    //    Email = user.Correo,
        //    //    Name = user.Nombre,
        //    //    BackUpEmail = user.CorreoRespaldo,
        //    //    PhoneNumber = user.Telefono,
        //    //    Address = user.Direccion,

        //    //}; 

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok();
        //}

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'MyProtocolsDBContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
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
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private User? GetUSerByID(int id) 
        {
            var user = _context.Users.Find(id);
            return user;
        }
    }
}
