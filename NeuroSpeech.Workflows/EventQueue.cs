﻿#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NeuroSpeech.Workflows
{
    public class EventQueue
    {
        private Queue<string>? pending;
        private TaskCompletionSource<string>? taskSoruce;
        private CancellationTokenSource? cancelSource;
        
        public EventQueue()
        {
        }

        public void Reset()
        {
            cancelSource?.Cancel();
            taskSoruce = null;
            cancelSource = null;
        }

        public void SetEvent(string result)
        {
            if (taskSoruce == null)
            {
                pending ??= new Queue<string>();
                pending.Enqueue(result);
                return;
            }
            this.taskSoruce.TrySetResult(result);
            cancelSource?.Cancel();
        }

        public (Task<string> waiter, CancellationToken token) Request()
        {
            if(pending?.Count > 0)
            {
                return (Task.FromResult(pending.Dequeue()), default);
            }
            taskSoruce = new TaskCompletionSource<string>();
            cancelSource = new CancellationTokenSource();
            return (taskSoruce.Task, cancelSource.Token);
        }
    }
}
