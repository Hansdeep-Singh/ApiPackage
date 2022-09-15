using AppContext.Interface;
using EfficacySend.Models;
using EfficacySend.Utilities;
using Logic.Efficacy.EncryptDecrypt;
using AppContext.Extentions;
using Microsoft.Extensions.DependencyInjection;

namespace AppContext.Service
{
    public class ApplicationContext : IApplicationContext, IDisposable
    {
        public ISessionService SessionService { get; }
        

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
        
        public ApplicationContext(ISessionService SessionService, IServiceProvider ServiceProvider)
        {
            tasks = new List<Task>();
            this.SessionService = SessionService;
            CreateService(this);
        }


       


        private Lazy<IHashingService> _hashingService;
        public IHashingService HashingService  =>_hashingService.Value;


        public void CreateService(IApplicationContext ctx)
        {
            _hashingService = new Lazy<IHashingService>(ctx.Create<IHashingService>);
        }


        public async Task<bool> SendEmail(Email email)
        {
            ISender sender = new Sender("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");
            return await sender.SendEmailAll(email);
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
