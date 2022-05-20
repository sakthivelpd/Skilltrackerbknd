namespace SkillTracker.Services.Admin.API.Events;
public class BaseEvent
    {
        public BaseEvent()
        {
            Id = Guid.NewGuid();
        }

        public BaseEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }

