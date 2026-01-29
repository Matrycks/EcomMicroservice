using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Messaging.Interfaces
{
    public interface IMessageDispatcher<T> where T : class
    {
        public Task Dispatch(T evt);
    }
}