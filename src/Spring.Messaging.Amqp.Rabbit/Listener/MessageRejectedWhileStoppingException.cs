﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageRejectedWhileStoppingException.cs" company="The original author or authors.">
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

namespace Spring.Messaging.Amqp.Rabbit.Listener
{
    /// <summary>
    /// Exception that indicates a rejected message on shutdown. Used to trigger a rollback for an
    /// external transaction manager in that case.
    /// </summary>
    /// <author>Dave Syer</author>
    /// <author>Joe Fitzgerald (.NET)</author>
    public class MessageRejectedWhileStoppingException : AmqpException
    {
        /// <summary>Initializes a new instance of the <see cref="MessageRejectedWhileStoppingException"/> class.</summary>
        public MessageRejectedWhileStoppingException() : base("Message listener container was stopping when a message was received") { }
    }
}
