
using ApiContext.Context.Interface;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiWeb.ApiBroadcast.Efficacy.Interface;

using Logic.Services.ConfigService;
using Logic.Efficacy.EncryptDecrypt;
using ApiContext.Extentions;

namespace ApiContext.Context.Service
{
    public sealed class ApplicationContext : IApplicationContext,IDisposable
    {
        public ISessionService? SessionService { get; }

        private Lazy<IConfigService>? _configService;

        private Lazy<IEmailService>? _emailService;
        private Lazy<IHashingService?>? _hashingService;
        public IHashingService? HashingService => _hashingService?.Value!;
        public IConfigService? ConfigService => _configService?.Value;
        public IEmailService? EmailService=> _emailService?.Value;
      
        private readonly List<Task> tasks;
        private void TrackTasks(Task task)
        {
            lock (tasks)
            {
                tasks.RemoveAll(x => x.Status == TaskStatus.RanToCompletion ||
                x.Status == TaskStatus.Faulted ||
                x.Status == TaskStatus.Canceled);
                tasks.Add(task);
            }
        }

        public ApplicationContext(ISessionService SessionService)
        {
            tasks = new List<Task>();
            this.SessionService = SessionService;
            CreateService(this);
        }
        public void CreateService(IApplicationContext ctx) {
         
            _configService = new Lazy<IConfigService>(ctx.Create<IConfigService>);
            _emailService = new Lazy<IEmailService>(ctx.Create<IEmailService>);
          
        }

        public async Task PrepareToDispose()
        {
            await Task.WhenAll(tasks);
        }
        public void Dispose()
        {
            if (tasks.Count < 10)
            {
                var canToken = new CancellationTokenSource();
                Task.Run(async () => { await PrepareToDispose(); }, canToken.Token);
                tasks.Clear();
            }
            GC.SuppressFinalize(this);
        }
    }
}
