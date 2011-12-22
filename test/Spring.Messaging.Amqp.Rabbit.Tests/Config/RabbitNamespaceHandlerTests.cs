﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Spring.Core.IO;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Config;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.Messaging.Amqp.Rabbit.Tests.Test;
using Spring.Objects.Factory.Xml;

namespace Spring.Messaging.Amqp.Rabbit.Tests.Config
{

    /// <summary>
    /// RabbitNamespaceHandler Tests
    /// </summary>
    [TestFixture]
    [Category(TestCategory.Unit)]
    public class RabbitNamespaceHandlerTests
    {
        private XmlObjectFactory beanFactory;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            NamespaceParserRegistry.RegisterParser(typeof(RabbitNamespaceHandler));
            var resourceName =
                @"assembly://Spring.Messaging.Amqp.Rabbit.Tests/Spring.Messaging.Amqp.Rabbit.Tests.Config/"
                + typeof(RabbitNamespaceHandlerTests).Name + "-context.xml";
            var resource = new AssemblyResource(resourceName);
            beanFactory = new XmlObjectFactory(resource);
        }

        [Test]
	    public void testQueue() 
        {
		    var queue = beanFactory.GetObject<Queue>("foo");
		    Assert.IsNotNull(queue);
		    Assert.AreEqual("foo", queue.Name);
	    }

        [Test]
        public void testAliasQueue() 
        {
		    var queue = beanFactory.GetObject<Queue>("spam");
		    Assert.IsNotNull(queue);
		    Assert.AreNotSame("spam", queue.Name);
		    Assert.AreEqual("bar", queue.Name);
	    }

        [Test]
        public void testAnonymousQueue() 
        {
		    var queue = beanFactory.GetObject<Queue>("bucket");
		    Assert.IsNotNull(queue);
		    Assert.AreNotSame("bucket", queue.Name);
		    Assert.True(queue is AnonymousQueue);
	    }

        [Test]
        public void testExchanges() 
        {
		    Assert.IsNotNull(beanFactory.GetObject<DirectExchange>("direct-test"));
		    Assert.IsNotNull(beanFactory.GetObject<TopicExchange>("topic-test"));
		    Assert.IsNotNull(beanFactory.GetObject<FanoutExchange>("fanout-test"));
		    Assert.IsNotNull(beanFactory.GetObject<HeadersExchange>("headers-test"));
	    }

        [Test]
        [Ignore("Need to fix...")]
        public void testBindings() 
        {
		    var bindings = beanFactory.GetObjectsOfType(typeof(Binding));
		    // 4 for each exchange type
		    Assert.AreEqual(16, bindings.Count);
	    }

        [Test]
        public void testAdmin()
        {
		    Assert.IsNotNull(beanFactory.GetObject<RabbitAdmin>("admin-test"));
	    }
    }
}