using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SlotMachine.Services
{
    public interface IDispatcherService
    {
        Task InvokeAsync(Action action);
    }

    public class DispatcherService : IDispatcherService
    {
        public Task InvokeAsync(Action action)
        {
            return Application.Current.Dispatcher.InvokeAsync(action).Task;
        }
    }
}
