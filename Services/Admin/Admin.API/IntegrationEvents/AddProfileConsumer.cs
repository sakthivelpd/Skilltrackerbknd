


namespace SkillTracker.Services.Admin.API.Events;
public class AddProfileConsumer : IConsumer<AddProfileEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddProfileConsumer> _logger;

    public AddProfileConsumer(IMediator mediator, ILogger<AddProfileConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AddProfileEvent> context)
    {
        var command = JsonSerializer.Deserialize<CacheProfileCommand>(context.Message.Data);
        await _mediator.Send(command);
    }
}
