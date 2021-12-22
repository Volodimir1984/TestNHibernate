using System.Threading.Tasks;
using log4net;
using MassTransit;

namespace TestNHibernate
{
    public class ExceptionConsumer : IConsumer<Fault>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExceptionConsumer));

        public Task Consume(ConsumeContext<Fault> context)
        {
            foreach (var exception in context.Message.Exceptions)
            {
                _logger.Error(exception.Message);
            }

            return Task.CompletedTask;
        }
    }
}
