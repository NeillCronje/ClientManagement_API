using ClientManagementAPI.Data;
using ClientManagementAPI.Logging;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogging _logger;

        public ClientController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClientDTO>> GetClients()
        {
            _logger.Log("Getting all clients", "");
            return Ok(ClientStore.clientList);
        }

        [HttpGet("{id:int}", Name = "GetClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ClientDTO> GetClient(int id)
        {
            _logger.Log($"Getting client by ID {id}", "");

            if (id == 0)
            {
                _logger.Log($"Getting client error with ID {id}", "error");
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(u => u.Id == id);

            if (client == null)
            {
                _logger.Log($"Getting client error not found by ID {id}", "error");
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDTO> CreateClient([FromBody] ClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log("Creating error ModelState invalid", "error");

                ModelState.Values.SelectMany(v => v.Errors).ToList()
                    .ForEach(x => _logger.Log($"{x.ErrorMessage}\n", "error"));

                return BadRequest(ModelState);
            }

            _logger.Log($"Create clients from {clientDto}", "");
            if (ClientStore.clientList.FirstOrDefault(u => u.Email.ToLower() == clientDto.Email.ToLower()) != null)
            {
                ModelState.AddModelError("DuplicateEmail", "Email address already exists");
                _logger.Log($"Creating client error duplicate email addresses {clientDto.Email}", "error");
                return BadRequest(ModelState);
            }
            if (clientDto == null)
            {
                _logger.Log($"Creating client error clientDto empty", "error");
                return BadRequest(clientDto);
            }
            if (clientDto.Id > 0)
            {
                _logger.Log($"Creating client error clientDto has an ID", "error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            clientDto.Id = ClientStore.clientList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            ClientStore.clientList.Add(clientDto);

            return CreatedAtRoute("GetClient", new { id = clientDto.Id }, clientDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteClient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteClient(int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log("Deleting error ModelState invalid", "error");

                ModelState.Values.SelectMany(v => v.Errors).ToList()
                    .ForEach(x => _logger.Log($"{x.ErrorMessage}\n", "error"));

                return BadRequest(ModelState);
            }

            _logger.Log($"Deleting client by ID {id}", "");
            if (id == 0)
            {
                _logger.Log($"Creating client error ID {id}", "error");
                return BadRequest();
            }
            var client = ClientStore.clientList.FirstOrDefault(u => u.Id == id);

            if (client == null)
            {
                _logger.Log($"Creating client error client not found", "error");
                return NotFound();
            }

            _logger.Log($"Deleting client successfully with ID {id}", "");
            ClientStore.clientList.Remove(client);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateClient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClientDTO> UpdateClient(int id, [FromBody] ClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log("Updating error ModelState invalid", "error");

                ModelState.Values.SelectMany(v => v.Errors).ToList()
                    .ForEach(x => _logger.Log($"{x.ErrorMessage}\n", "error"));

                return BadRequest(ModelState);
            }

            _logger.Log($"Updating client ID {id} with {clientDto}", "");
            if (clientDto == null || id != clientDto.Id)
            {
                _logger.Log($"Updating client error ID {id} with {clientDto}", "error");
                return BadRequest();
            }
            var client = ClientStore.clientList.FirstOrDefault(u => u.Id == id);

            client.Address = clientDto.Address;
            client.Age = clientDto.Age;
            client.City = clientDto.City;
            client.Email = clientDto.Email;
            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.MiddleName = clientDto.MiddleName;
            client.PostalCode = clientDto.PostalCode;
            client.Region = clientDto.Region;


            if (client == null)
            {
                _logger.Log($"Updating client error not found with ID {id}", "error");
                return NotFound();
            }

            _logger.Log($"Updated client successfully ID {id} with {clientDto}", "");
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialClient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClientDTO> UpdatePartialClient(int id, JsonPatchDocument<ClientDTO> patchDto)
        {
            _logger.Log($"Updating client ID {id} with {patchDto}", "");

            if (!ModelState.IsValid)
            {
                _logger.Log("Updating partial error ModelState invalid", "error");

                ModelState.Values.SelectMany(v => v.Errors).ToList()
                    .ForEach(x => _logger.Log($"{x.ErrorMessage}\n", "error"));

                return BadRequest(ModelState);
            }

            if (patchDto == null || id == 0)
            {
                _logger.Log($"Updating client error with ID {id} with {patchDto}", "error");
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(u => u.Id == id);

            if (client == null)
            {
                _logger.Log($"Updating client error not found with ID {id}", "error");
                return NotFound();
            }

            patchDto.ApplyTo(client, ModelState);



            _logger.Log($"Successfully updated partial of client {patchDto}", "error");
            return NoContent();
        }
    }
}
