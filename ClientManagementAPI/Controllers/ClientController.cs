﻿using AutoMapper;
using ClientManagementAPI.Data;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using ClientManagementAPI.Repository;
using ClientManagementAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClientManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IClientRepository _dbClient;
        private readonly ICompanyRepository _dbCompany;
        private readonly IMapper _mapper;
        public ClientController(IClientRepository dbClient, ICompanyRepository dbCompany, IMapper mapper)
        {
            _dbClient = dbClient;
            _dbCompany = dbCompany;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet(Name = "GetClients")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetClients()
        {
            try
            {
                IEnumerable<Client> clientList = await _dbClient.GetAllAsync();

                _response.Result = _mapper.Map<List<ClientDTO>>(clientList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name = "GetClient")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClient(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var client = await _dbClient.GetAsync(u => u.Id == id);

                if (client == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                _response.Result = _mapper.Map<ClientDTO>(client);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost(Name = "CreateClient")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateClient([FromBody] ClientCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if (await _dbClient.GetAsync(u => u.Email.ToLower() == createDTO.Email.ToLower()) != null)
                {
                    _response.ErrorMessages = new List<string>() { "Email address already exists" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if(await _dbCompany.GetAllAsync(co => co.Id == createDTO.CompanyId) == null)
                {
                    _response.ErrorMessages = new List<string>() { "Company with the same ID already exists" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(createDTO);
                }

                Client client = _mapper.Map<Client>(createDTO);

                await _dbClient.CreateAsync(client);

                _response.Result = _mapper.Map<ClientDTO>(client);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetClient", new { id = client.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteClient")]
        [Authorize (Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteClient(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }
                var client = await _dbClient.GetAsync(u => u.Id == id);

                if (client == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound();
                }

                await _dbClient.RemoveAsync(client);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateClient")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateClient(int id, [FromBody] ClientUpdateDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                if (await _dbCompany.GetAllAsync(co => co.Id == updateDTO.CompanyId) == null)
                {
                    _response.ErrorMessages = new List<string>() { "Invalid company ID" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                Client model = _mapper.Map<Client>(updateDTO);

                await _dbClient.UpdateClientAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialClient")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdatePartialClient(int id, JsonPatchDocument<ClientUpdateDTO> patchDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                if (patchDTO == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                var client = await _dbClient.GetAsync(u => u.Id == id, false);

                ClientUpdateDTO clientDTO = _mapper.Map<ClientUpdateDTO>(client);

                if (client == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                patchDTO.ApplyTo(clientDTO, ModelState);

                Client model = _mapper.Map<Client>(clientDTO);


                await _dbClient.UpdateClientAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
