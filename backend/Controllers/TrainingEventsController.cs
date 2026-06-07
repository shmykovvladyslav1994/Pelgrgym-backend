using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("training-events")]
[Authorize]
public class TrainingEventsController : ControllerBase
    {
    private readonly AppDbContext _db;

    public TrainingEventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateTrainingEventDto dto)
    {
        var trainingEvent = new TrainingEvent
        {
            Date = DateTime.UtcNow,
            Result = dto.Result,
            TrainingId = dto.TrainingId,
        };

        _db.TrainingEvents.Add(trainingEvent);
        await _db.SaveChangesAsync();

        return Ok(trainingEvent);
    }

    [HttpGet("{trainingId}")]
    public IActionResult GetEvents(int trainingId)
    {
        var events = _db.TrainingEvents.Where(e => e.TrainingId == trainingId).OrderBy(e => e.Date).ToList();

        return Ok(events);
    }
}

