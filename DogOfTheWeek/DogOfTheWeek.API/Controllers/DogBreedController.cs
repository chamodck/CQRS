using DogOfTheWeek.Application.Common.Exceptions;
using DogOfTheWeek.Application.Common.Models;
using DogOfTheWeek.Application.Handlers.DogBreed.Commands.CreateDogBreed;
using DogOfTheWeek.Application.Handlers.DogBreed.Commands.DeleteDogBreed;
using DogOfTheWeek.Application.Handlers.DogBreed.Commands.UpdateDogBreed;
using DogOfTheWeek.Application.Handlers.DogBreed.Models;
using DogOfTheWeek.Application.Handlers.DogBreed.Queries.GetAllDogBreeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogOfTheWeek.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DogBreedController : ApiControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<DogBreedsVM>> GetAll()
    {
        try
        {
            var results = await Mediator.Send(new GetAllDogBreedsQuery());
            return Ok(new Response<DogBreedsVM>(results));
        }
        catch (ValidationException ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), ex.Message, ex.Errors));
        }
        catch (Exception ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<int>> Create(CreateDogBreedCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(new Response<long>(result));
        }
        catch (ValidationException ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), ex.Message, ex.Errors));
        }
        catch (Exception ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> Update(int id, UpdateDogBreedCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return Ok(new Response<string>("Successfully update DogBreed"));
        }
        catch (ValidationException ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), ex.Message, ex.Errors));
        }
        catch (Exception ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await Mediator.Send(new DeleteDogBreedCommand(id));

            return Ok(new Response<string>("Successfully delete DogBreed"));
        }
        catch (ValidationException ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), ex.Message, ex.Errors));
        }
        catch (Exception ex)
        {
            return new JsonResult(new Response<StatusCodeResult>(new StatusCodeResult(500), $"{ex.Message}"));
        }
    }
}
