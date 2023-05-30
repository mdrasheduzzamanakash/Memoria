using Memoria.DataService.IConfiguration;
using Memoria.DataService.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MemoriaMVC.BackgroudTask
{
    public class TrashCleanupTask : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private Timer _timer;

        public TrashCleanupTask(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _services.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var notesToBeDeleted = await unitOfWork.Notes.TrashedNotesOlderThan30Days();
                await unitOfWork.Notes.RemoveRange(notesToBeDeleted); 
                await unitOfWork.CompleteAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
