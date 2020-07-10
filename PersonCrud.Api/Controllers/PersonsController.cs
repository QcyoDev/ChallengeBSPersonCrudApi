using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PersonCrud.Api.Data;
using PersonCrud.Api.Dtos;
using PersonCrud.Api.Enums;
using PersonCrud.Api.Interfaces;
using PersonCrud.Api.Models;

namespace PersonCrud.Api.Controllers
{
    /// <summary>
    /// API para mantenimiento del recurso persona
    /// </summary>
    [Produces("application/json")]
    [Route("api/personas")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStatisticsService _statisticsService;

        public PersonsController(AppDbContext context, IMapper mapper, IStatisticsService statisticsService)
        {
            _context = context;
            _mapper = mapper;
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Obtener todas las personas
        /// </summary>
        /// <returns>Listado de personas</returns>
        /// <response code="200">Devuelve el listado de personas</response>    
        /// <response code="500">Si ocurre error consultando listado de personas</response>    
        // GET: api/People
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonGetDto>>> GetPersons()
        {
            try
            {
                var people = await _context.Persons.Include(p => p.ContactDetails).ToListAsync();
                var peopleDto = _mapper.Map<List<PersonGetDto>>(people);

                return peopleDto;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Obtener una persona por el ID
        /// </summary>
        /// <param name="id">Id de la persona a buscar</param>
        /// <returns>Devuelve una persona</returns>
        /// <response code="200">Devuelve los datos de la persona</response>    
        /// <response code="404">Si no existe persona con ese Id</response>    
        /// <response code="500">Si se produce error consultando la persona</response>    
        // GET: api/People/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonGetDto>> GetPerson(int id)
        {
            try
            {
                var person = await _context.Persons.Where(p => p.PersonId == id).Include(p => p.ContactDetails).FirstOrDefaultAsync();

                var personDto = _mapper.Map<PersonGetDto>(person);

                if (person == null)
                {
                    return NotFound();
                }

                return personDto;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Actualizar persona
        /// </summary>
        /// <param name="id">Id de la persona</param>
        /// <param name="personDto">Datos de la persona</param>
        /// <returns></returns>
        /// <response code="400">Si datos no validos</response>    
        /// <response code="404">Si no hay usuario con ese id</response>    
        /// <response code="204">Si se actualizo usuario correctamente</response>    
        /// <response code="500">Si se produce error actualizando persona</response>    
        // PUT: api/People/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutPerson(int id, PersonPutDto personDto)
        {
            try
            {
                if (id != personDto.PersonId)
                {
                    return BadRequest();
                }

                var person = _mapper.Map<Person>(personDto);
                _context.Entry(person).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(id))
                        return NotFound();
                    else
                        return StatusCode(500);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Agregar una nueva persona
        /// </summary>
        /// <remarks>
        /// Las personas se identifican por Tipo de documento, numero de documento, pais y sexo. <br/>
        /// No puede haber personas repetidas.<br/>
        /// No pueden crearse personas menores a 18 años.<br/>
        /// El sexo corresponde a la siguiente enumeracion:<br/>
        /// masculino = 0,<br/> femenino = 1<br/><br/> 
        /// La persona puede tener mas de un dato de contacto, los que se le pueden pasar como una coleccion en la propiedad contactDetails.
        /// 
        ///     "contactDetails": [
        ///         {
        ///             "contactType": 0,
        ///             "contactInfo": "string"
        ///         },
        ///         {
        ///             "contactType": 0,
        ///             "contactInfo": "string"
        ///         }
        ///     ]
        ///     
        /// contactType es una enumeracion que corresponde a los siguientes codigos:<br/>
        /// Phone = 0,<br/> CellPhone = 1,<br/> Adress = 2,<br/> Email = 3.
        /// </remarks>
        /// <param name="personDto">Objeto de tipo persona</param>
        /// <returns>La persona creada</returns>
        /// <response code="201">Retorna la persona creada</response>
        /// <response code="400">Si los datos no son correctos</response>
        /// <response code="500">Si ocurrio error al agregar nueva persona</response>
        // POST: api/People
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonGetDto>> PostPerson(PersonPostDto personDto)
        {
            try
            {
                var person = _mapper.Map<Person>(personDto);

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                var personGetDto = _mapper.Map<PersonGetDto>(person);

                return CreatedAtAction(nameof(GetPerson), new { id = personGetDto.PersonId }, personGetDto);
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException is SqlException)
                {
                    if (((SqlException)e.InnerException).Number == 2601)
                        return BadRequest("No se puede insertar personas duplicadas, revisar documentacion");
                }
                return StatusCode(500);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Eliminar persona
        /// </summary>
        /// <param name="id">Id de la persona a eliminar</param>
        /// <returns>La persona eliminada</returns>
        /// <response code="200">si se elimino correctamente</response>    
        /// <response code="404">Si no hay usuario con ese id</response>    
        /// <response code="500">Si se produjo error eliminando persona</response>    
        // DELETE: api/People/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonGetDto>> DeletePerson(int id)
        {
            try
            {
                var person = await _context.Persons.Where(p => p.PersonId == id).Include(p => p.ContactDetails).FirstOrDefaultAsync();
                if (person == null)
                {
                    return NotFound();
                }

                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();

                var personDto = _mapper.Map<PersonGetDto>(person);
                return personDto;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Crear relacion entre dos personas.
        /// </summary>
        /// <remarks>
        /// Se crea relacion entre dos personas, donde id1 es padre de id2.<br/><br/>
        /// Las relaciones corresponden a la enumarcion:<br/>
        /// Padre = 0, <br/>
        /// Hermano = 1, <br/>
        /// Tio = 2, <br/>
        /// Primo = 3 <br/>
        /// </remarks>
        /// <param name="id1">Persona 1</param>
        /// <param name="relacion">Relacion de persona 1 con 2</param>
        /// <param name="id2">Persona 2</param>
        /// <returns></returns>
        [HttpPost("{id1}/{relacion}/{id2}")]
        public async Task<ActionResult> SetRelation(int id1, Relations relacion ,int id2)
        {
            try
            {
                Relationship rel = new Relationship { PersonId = id1, RelatedPersonId = id2, RelationType = relacion };

                _context.Relationship.Add(rel);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Obtener relaciones entre personas
        /// </summary>
        /// <remarks>
        /// Devuelve la relacion entre dos personas
        /// </remarks>
        /// <param name="id1">Id persona 1</param>
        /// <param name="id2">Id persona 2</param>
        /// <returns></returns>
        [HttpGet("relaciones/{id1}/{id2}")]
        public async Task<ActionResult<string>> GetPerson(int id1, int id2)
        {
            var relationship = await _context.Relationship.Where(r => r.PersonId == id1 && r.RelatedPersonId == id2).FirstOrDefaultAsync();

            if (relationship == null)
            {
                return NoContent();
            }

            return relationship.RelationType.ToString();
        }

        /// <summary>
        /// Obtener cifras totalizadoras.
        /// </summary>
        /// <remarks>
        /// Devuelve cifras totalizadoras de cantidad de mujeres, hombres y porcentaje de argentinos sobre el total.
        /// </remarks>
        /// <returns>Listado de personas</returns>
        /// <response code="200">Devuelve el listado de personas</response>    
        /// <response code="500">Si ocurre error consultando listado de personas</response>    
        // GET: api/statistics
        [HttpGet("estadisticas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            try
            {
                var malesTotal = await _statisticsService.GetTotalByGenderAsync(Gender.masculino);
                var femalesTotal = await _statisticsService.GetTotalByGenderAsync(Gender.femenino);
                var argentinesPercent = await _statisticsService.GetPercentByCountryAsync("Argentina");

                return new StatisticsDto { TotalMales = malesTotal, TotalFemales = femalesTotal, ArgentinesPercent = argentinesPercent };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
    }
}
