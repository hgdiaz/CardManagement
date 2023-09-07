using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Features.Cards.Queries;
using Application.Features.Cards.Commands;
using Application.Exceptions.Card;

namespace Cardmanagement.API.Controllers
{

    public class CardController : BaseApiController<CardController>
    {

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetAllCardsQuery());
            return Ok(list);
        }

        [HttpGet("{number}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string number)
        {
            try
            {
                var item = await _mediator.Send(new GetCardByNumberQuery() { Number = number });
                return Ok(item);
            }
            catch (CardDataNotValidException ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }
            catch (NoCardNumberException ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }

        }

        //[Authorize]
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Post(AddCardCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result.Id > 0)
                { return Created("Post", result.Id); }
                else
                { return BadRequest(); }
            }
            catch (CardExistsException ex)
            {
                return Conflict(new
                {
                    ex.Message
                });
            }
            catch (CardDataNotValidException ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(EditCardCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                { return NoContent(); }
                else
                { return BadRequest(); }
            }
            catch (NoCardIdException ex)
            {
                return Conflict(new
                {
                    ex.Message
                });
            }
            catch (CardExistsException ex)
            {
                return Conflict(new
                {
                    ex.Message
                });
            }
            catch (CardDataNotValidException ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCardCommand() { Id = id });
                if (result.Success)
                { return NoContent(); }
                else
                { return BadRequest(); }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
