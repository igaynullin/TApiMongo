using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TApiMongo.Data.Entities;
using TApiMongo.Data.Repositories;
using TApiMongo.Web.Extentions;
using TApiMongo.Web.Mappers;
using TApiMongo.Web.ViewModels.Items;

namespace TApiMongo.Web.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]

    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IDataProtector _protector;

        public ItemsController(IItemRepository itemRepository, IDataProtector dataProtector)
        {
            _protector = dataProtector;
            _itemRepository = itemRepository;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List>), 200)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            try

            {
                var data = _itemRepository.GetAll();
                if (data == null)
                {
                    return NotFound("Items not found");
                }

                var mapper = new Mapper<List<Item>, List<List>>(ItemMapperMethods.MapEntityToListItemViewModel, null);
                var result = new List<List>();

                result = mapper.MapViewModel(data.ToList());
                IdProtector<List>.Protect(_protector, result);

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Get), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(nameof(id));
                id = _protector.Unprotect(id);
                if (int.TryParse(id, out var _id))
                    return BadRequest(nameof(id));

                var mapper = new Mapper<Item, Get>(ItemMapperMethods.MapEntityToGetViewModel, null);
                var data = _itemRepository.FirstOrDefault(e => e.ID == _id);
                var viewModel = mapper.MapViewModel(data);
                IdProtector<Get>.Protect(_protector, viewModel);
                return Ok(viewModel);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Post([FromBody]Post viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mapper = new Mapper<Item, Post>(null, ItemMapperMethods.MapViewModelToItemEntity);

                var entity = mapper.MapEntity(viewModel);
                _itemRepository.Add(entity);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Put(string id, [FromBody]Put viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(id);
                if (id != viewModel.ID)
                    return BadRequest(nameof(viewModel.ID));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                viewModel.ID = _protector.Unprotect(viewModel.ID);
                var mapper = new Mapper<Item, Put>(null, ItemMapperMethods.MapViewModelToItemEntity);

                var entity = mapper.MapEntity(viewModel);
                _itemRepository.SavePartial(entity);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(id);

                _itemRepository.Delete(new Item { ID = Convert.ToInt32(_protector.Unprotect(id)) });
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}