using AutoMapper;
using ClientManagementAPI.Data;
using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using ClientManagementAPI.Repository;
using ClientManagementAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClientManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICompanyRepository _dbClient;
        private readonly IMapper _mapper;
        public CompanyController(ICompanyRepository dbClient, IMapper mapper)
        {
            _dbClient = dbClient;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet(Name = "GetCompanies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCompanies()
        {
            try
            {
                IEnumerable<Company> clientList = await _dbClient.GetAllAsync();

                _response.Result = _mapper.Map<List<CompanyDTO>>(clientList);
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

        [HttpGet("{id:int}", Name = "GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                var company = await _dbClient.GetAsync(u => u.Id == id);

                if (company == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                _response.Result = _mapper.Map<CompanyDTO>(company);
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

        [HttpPost(Name = "CreateCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCompany([FromBody] CompanyCreateDTO createDTO)
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
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(createDTO);
                }

                Company company = _mapper.Map<Company>(createDTO);

                await _dbClient.CreateAsync(company);

                _response.Result = _mapper.Map<CompanyDTO>(company);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCompany", new { id = company.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCompany(int id)
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
                var company = await _dbClient.GetAsync(u => u.Id == id);

                if (company == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound();
                }

                await _dbClient.RemoveAsync(company);

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

        [HttpPut("{id:int}", Name = "UpdateCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCompany(int id, [FromBody] CompanyUpdateDTO updateDTO)
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

                Company model = _mapper.Map<Company>(updateDTO);

                await _dbClient.UpdateCompanyAsync(model);

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

        [HttpPatch("{id:int}", Name = "UpdatePartialCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartialCompany(int id, JsonPatchDocument<CompanyUpdateDTO> patchDTO)
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

                var company = await _dbClient.GetAsync(u => u.Id == id, false);

                CompanyUpdateDTO companyDTO = _mapper.Map<CompanyUpdateDTO>(company);

                if (company == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound(_response);
                }

                patchDTO.ApplyTo(companyDTO, ModelState);

                Company model = _mapper.Map<Company>(companyDTO);


                await _dbClient.UpdateCompanyAsync(model);

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
