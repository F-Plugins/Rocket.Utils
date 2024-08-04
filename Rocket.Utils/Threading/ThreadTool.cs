using Rocket.Core.Logging;
using Rocket.Core.Utils;
using SDG.Unturned;
using System;
using System.Threading;
using System.Threading.Tasks;
using Action = System.Action;

namespace Feli.Rocket.Utils.Threading;

public static class ThreadTool
{
    public static async Task RunOnMainThreadAsync(Action action)
    {
        await RunOnMainThreadAsync(() =>
        {
            action();
            return true;
        });
    }

    public static async Task RunOnMainThreadAsync<T>(Func<T> func)
    {
        var source = new TaskCompletionSource<T>();

        TaskDispatcher.QueueOnMainThread(() =>
        {
            try
            {
                source.SetResult(func());
            }
            catch (Exception ex) 
            {
                source.SetException(ex);
            }
        });

        await source.Task;
    }

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, (exception) =>
        {
            Logger.LogException(exception, "There was an error while running game thread action");
        });
    }

    public static void QueueOnMainThread(Action action, Action<Exception> exceptionHandler)
    {
        void handle()
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
        }

        if (Thread.CurrentThread.IsGameThread())
        {
            handle();
            return;
        }

        TaskDispatcher.QueueOnMainThread(handle);
    }

    public static void RunOnThreadPool(Func<Task> task)
    {
        RunOnThreadPool(task, (exception) => QueueOnMainThread(() => Logger.LogException(exception, "There was an error while running the task")));
    }

    public static void RunOnThreadPool(Func<Task> task, Action<Exception> exceptionHandler)
    {
        Task.Run(async () =>
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
            }
        });
    }
}
