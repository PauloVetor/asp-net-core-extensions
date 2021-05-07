﻿using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace NeuroSpeech.Workflows.Impl
{
    public class WorkflowExecutor<T,TInput, TOutput> : TaskOrchestration<TOutput, TInput>
        where T: Workflow<TInput, TOutput>
    {
        private readonly IServiceProvider sp;
        private readonly T workflow;

        public WorkflowExecutor(IServiceProvider sp)
        {
            this.sp = sp;
            this.workflow = sp.Build<T>();
        }
        public override Task<TOutput> RunTask(OrchestrationContext context, TInput input)
        {
            workflow.context = context;
            return workflow.RunTask(input);
        }

        public override void OnEvent(OrchestrationContext context, string name, string input)
        {
            workflow.context = context;
            foreach(var f in workflow.GetType().GetFields())
            {
                if(f.Name == name)
                {
                    if(f.GetValue(workflow) is IWorkflowEvent iwf)
                    {
                        iwf.SetEvent(input);
                        return;
                    }
                    throw new NotSupportedException();
                }
            }
        }
    }
}
