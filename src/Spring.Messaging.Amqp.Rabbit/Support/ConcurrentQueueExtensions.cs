﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentQueueExtensions.cs" company="The original author or authors.">
//   Copyright 2002-2012 the original author or authors.
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
//   the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
//   
//   Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
//   an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
//   specific language governing permissions and limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
#endregion

namespace Spring.Messaging.Amqp.Rabbit.Support
{
    /// <summary>
    /// Concurrent Queue Extensions
    /// </summary>
    public static class ConcurrentQueueExtensions
    {
        internal static readonly TimeSpan MaxValue = TimeSpan.FromMilliseconds(int.MaxValue);

        /// <summary>The take.</summary>
        /// <param name="queue">The queue.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The T.</returns>
        public static T Take<T>(this ConcurrentQueue<T> queue)
        {
            T result;
            var success = queue.TryDequeue(out result);
            if (success)
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>The poll.</summary>
        /// <param name="queue">The queue.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="element">The element.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The System.Boolean.</returns>
        /// <exception cref="ThreadInterruptedException"></exception>
        public static bool Poll<T>(this ConcurrentQueue<T> queue, TimeSpan duration, out T element)
        {
            var deadline = DateTime.UtcNow.Add(duration);

            T result;
            while (true)
            {
                if (queue.Count > 0)
                {
                    var success = queue.TryDequeue(out result);
                    if (success)
                    {
                        break;
                    }
                }

                if (duration.Ticks <= 0)
                {
                    element = default(T);
                    return false;
                }

                try
                {
                    Thread.Sleep(Cap(duration > MaxValue ? MaxValue : duration));
                    duration = deadline.Subtract(DateTime.UtcNow);
                }
                catch (ThreadInterruptedException e)
                {
                    throw e.PreserveStackTrace();
                }
            }

            element = result;
            return true;
        }

        internal static TimeSpan Cap(TimeSpan waitTime) { return waitTime > MaxValue ? MaxValue : waitTime; }

        /// <summary>Lock the stack trace information of the given <paramref name="exception"/>
        /// so that it can be rethrow without losing the stack information.</summary>
        /// <remarks><example><code>try
        ///     {
        ///         //...
        ///     }
        ///     catch( Exception e )
        ///     {
        ///         //...
        ///         throw e.PreserveStackTrace(); //rethrow the exception - preserving the full call stack trace!
        ///     }</code>
        /// </example>
        /// </remarks>
        /// <param name="exception">The exception to lock the statck trace.</param>
        /// <returns>The same <paramref name="exception"/> with stack traced locked.</returns>
        private static T PreserveStackTrace<T>(this T exception) where T : Exception
        {
            _preserveStackTrace(exception);
            return exception;
        }

        private static readonly Action<Exception> _preserveStackTrace = (Action<Exception>)Delegate.CreateDelegate(
            typeof(Action<Exception>), 
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic));
    }
}