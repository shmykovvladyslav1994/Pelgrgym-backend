using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("trainings")]
public class TrainingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public TrainingsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateTrainingDto dto)
    {
        // 1. userId из JWT
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        // 2. создаём training
        var training = new Training
        {
            Name = dto.Name,
            RestIntervalSec = dto.RestIntervalSec,
            CreatedAt = DateTime.UtcNow,

            WorkDays = dto.TrainingCycle.WorkDays,
            RestDays = dto.TrainingCycle.RestDays,
            IncrementOrder = dto.IncrementOrder,

            UserId = userId,

            Sets = dto.Sets.Select(s => new TrainingSet
            {
                Reps = s.Reps,

                ProgressionType = s.ProgressionRule.Type,
                ProgressionValue = s.ProgressionRule.Value,
            }).ToList()
        };

        // 3. сохраняем всё сразу
        _db.Trainings.Add(training);
        await _db.SaveChangesAsync();

        return Ok(new { training.Id });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        // userId из JWT
        var useId = int.Parse(User.FindFirst("userId")!.Value);

        // берём trainings пользователя
        var trainings = await _db.Trainings.Where(t => t.UserId == useId).Include(t => t.Sets).ToListAsync();

        // превращаем обратно в frontend JSON
        var result = trainings.Select(t => new
        {
            id = t.Id,
            name = t.Name,
            restIntervalSec = t.RestIntervalSec,
            createdAt = t.CreatedAt,
            incrementOrder = t.IncrementOrder,

            trainingCycle = new
            {
                workDays = t.WorkDays,
                restDays = t.RestDays
            },

            sets = t.Sets.Select(s => new
            {
                reps = s.Reps,

                progressionRule = new
                {
                    type = s.ProgressionType,
                    value = s.ProgressionValue,
                }
            })
        });

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst("UserId")!.Value);
        var training = await _db.Trainings.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (training == null)
        {
            return NotFound();
        } else
        {
            _db.Trainings.Remove(training);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}